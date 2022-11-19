using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP1.Migrations;

namespace TP1
{
    internal class Banco
    {
        private MyContext contexto;
        private Usuario usuarioLogueado;

        public Banco()
        {
            inicializarAtributos();
        }


        private void inicializarAtributos()
        {
            try
            {
                //creo un contexto
                contexto = new MyContext();
                //cargo los usuarios

                contexto.usuarios.Load();
                contexto.usuarios.Include(u => u.cajas).Load();
                contexto.usuarios.Include(u => u._plazosFijos).Load();
                contexto.usuarios.Include(u => u._tarjetas).Load();
                contexto.usuarios.Include(u => u._pagos).Load();
                contexto.cajas.Include(u => u._movimientos).Load();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar tabla de usuarios");
            }
        }

        public bool validarUsuario(int dni, string password)
        {

            foreach (Usuario u in contexto.usuarios)
            {
                if (u._dni == dni)
                {
                    if (u._bloqueado == false)
                    {
                        if (u._password == password)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Usuario bloqueado");
                        return false;
                    }
                }
            }

            return false;
        }

        public bool agregarIntentoFallido(int dni)
        {
            bool salida = false;

            foreach (Usuario u in contexto.usuarios)
                if (u._dni == dni)
                {
                    u._intentosFallidos = u._intentosFallidos + 1;
                    if (u._intentosFallidos >= 3)
                    {
                        u._bloqueado = true;
                    }
                    contexto.usuarios.Update(u);
                    salida = true;
                }
            if (salida)
                contexto.SaveChanges();
            return salida;
        }

        public bool limpiarIntentosFallidos(int dni)
        {
            bool salida = false;

            foreach (Usuario u in contexto.usuarios)
                if (u._dni == dni)
                {
                    u._intentosFallidos = 0;
                    contexto.usuarios.Update(u);
                    salida = true;
                }
            if (salida)
                contexto.SaveChanges();
            return salida;
        }

        public bool agregarUsuario(int dni, string nombre, string apellido, string mail, string password, bool bloqueado, bool admin, int intentosFallidos)
        {
            try
            {
                Usuario nuevo = new Usuario(dni, nombre, apellido, mail, password, bloqueado, admin, intentosFallidos);
                contexto.usuarios.Add(nuevo);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Usuario buscarUsuario(int dni)
        {
            Usuario usuario;

            foreach (Usuario u in contexto.usuarios)
            {
                if (u._dni == dni)
                {
                    return u;
                }
            }

            return null;
        }

        public bool agregarCA(Usuario usuario, string cbu)
        {

            try
            {
                CajaDeAhorro nuevo = new CajaDeAhorro(cbu, 0);
                usuario.cajas.Add(nuevo);
                contexto.usuarios.Update(usuario);

                contexto.SaveChanges();
                return true;
            }
            catch (Exception ex) { MessageBox.Show("agregarCA: " + ex.Message + ex.InnerException.Message); return false; }
        }

        public ICollection<CajaDeAhorro> buscarListaCA(Usuario usuario)
        {
            ICollection<CajaDeAhorro> lista = new List<CajaDeAhorro>();

            foreach (Usuario u in contexto.usuarios)
            {
                if (u._dni == usuario._dni)
                {
                    lista = u.cajas;
                }
            }

            return lista;

        }

        public bool AltaPlazoFijo(double monto, double tasa, string cbu)
        {

            PlazoFijo plazoFijo = new PlazoFijo(monto, DateTime.Now.AddMonths(1), tasa);
            try
            {

                if (afectarSaldoCA(cbu, plazoFijo._monto))
                {
                 
                    try
                    {
                        usuarioLogueado._plazosFijos.Add(plazoFijo);
                        contexto.usuarios.Update(usuarioLogueado); 
                        contexto.SaveChanges();
                        AltaMovimiento(cbu, "Alta de plazo fijo", plazoFijo._monto);
                        return true;
                    }
                    catch (Exception ex) {
                        MessageBox.Show("agregarCA: " + ex.Message + ex.InnerException.Message); 
                        return false; }
                }


                return false;
            }
            catch (Exception ex) { MessageBox.Show("No se pudo crear el plazo fijo: " + ex.Message); return false; }

        }



        public List<PlazoFijo> buscarListaPF(Usuario usuario)
        {
            foreach (Usuario u in contexto.usuarios)
            {
                if (u._dni == usuario._dni)
                {
                    return u._plazosFijos;
                }
            }

            return null;
        }


        public bool afectarSaldoCA(string cbu, double monto)
        {
            CajaDeAhorro ca = null;

            foreach (CajaDeAhorro caja in contexto.cajas)
            {
                if (caja._cbu.Equals(cbu))
                {
                    if (caja._saldo >= monto)
                    {
                        ca = caja;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            try { 
                ca._saldo = ca._saldo - monto;
                contexto.cajas.Update(ca);
                contexto.SaveChanges();
                return true;
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error: " + ex.Message);
                return false;
            }

        }

        public bool eliminarPlazoFijo(Usuario usuario, int id)
        {
            foreach (PlazoFijo p in usuario._plazosFijos)
            {
                if (p._id_plazoFijo == id && p._pagado)
                {
                    usuario._plazosFijos.Remove(p);
                    contexto.usuarios.Update(usuario);
                    contexto.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public bool agregarTarjeta(Usuario usuario, TarjetaDeCredito tarjeta)
        {

            try
            {
                usuario._tarjetas.Add(tarjeta);
                contexto.usuarios.Update(usuario);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception ex) { MessageBox.Show("agregarTC: " + ex.Message + ex.InnerException.Message); return false; }

        }

        public List<TarjetaDeCredito> buscarTarjetasUsuario(Usuario usuario)
        {
            foreach (Usuario u in contexto.usuarios)
            {
                if (u._dni == usuario._dni)
                {
                    return u._tarjetas;
                }
            }

            return null;
        }

        public List<TarjetaDeCredito> buscarTarjetasUsuario()
        {
            foreach (Usuario u in contexto.usuarios)
            {
                if (usuarioLogueado._dni == u._dni)
                {
                    return u._tarjetas;
                }
            }

            return null;
        }



        public bool aumentarSaldoCA(string cbu, double monto)
        {
            CajaDeAhorro ca = null;
            foreach (CajaDeAhorro caja in contexto.cajas)
            {
                if (caja._cbu.Equals(cbu))
                {
                    ca = caja;

                }
            }

            try
            {
                ca._saldo = ca._saldo + monto;
                contexto.cajas.Update(ca);
                contexto.SaveChanges();
                AltaMovimiento(ca, "Deposito en cuenta", monto);
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrió un error al realizar el depósito: " + ex.Message);
                return false;
            }
        }




        public bool AltaTransferencia(string cbuOrigen, string cbuDestino, double monto)
        {
            CajaDeAhorro cajaOrigen = null;
            foreach (CajaDeAhorro co in contexto.cajas)
            {
                if (co._cbu.Equals(cbuOrigen))
                {
                    cajaOrigen = co;
                }

            }

            try
            {
                if (cajaOrigen._saldo >= monto)
                {
                    cajaOrigen._saldo = cajaOrigen._saldo - monto;
                    AltaMovimiento(cajaOrigen, "Retiro Transferencia", monto);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;


            }

            CajaDeAhorro cajaDestino = null;
            foreach (CajaDeAhorro co in contexto.cajas)
            {
                if (co._cbu.Equals(cbuDestino))
                {
                    cajaDestino = co;
                }

            }

            try
            {
                cajaDestino._saldo = cajaDestino._saldo + monto;
                contexto.cajas.Update(cajaOrigen);
                contexto.cajas.Update(cajaDestino);
                contexto.SaveChanges();
                AltaMovimiento(cajaOrigen, "Depósito Transferencia", monto);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }


        }

        public bool confirmarAltaPago(double monto, string metodo, long numero)
        {
            TarjetaDeCredito tarjeta = null;
            CajaDeAhorro caja = null;

            if (metodo.Equals("TC"))
            {
                foreach (TarjetaDeCredito tc in contexto.tarjetas)
                {
                    if (long.Parse(tc._numero) == numero)
                    {
                        tarjeta = tc;

                    }


                }

                if (tarjeta != null)
                {
                    tarjeta._consumos = tarjeta._consumos + monto;
                    contexto.tarjetas.Update(tarjeta);
                    contexto.SaveChanges();

                    return true;
                }
                else { return false; }


            }
            else
            {
                foreach (CajaDeAhorro ca in contexto.cajas)
                {
                    if (long.Parse(ca._cbu) == numero)
                    {
                        caja = ca;
                    }

                }

                if (caja != null)
                {
                    if (caja._saldo >= monto)
                    {
                        caja._saldo = caja._saldo - monto;
                        contexto.cajas.Update(caja);
                        contexto.SaveChanges();
                        AltaMovimiento(caja, "Pago por caja", monto);
                        return true;
                    }
                    else { return false; }

                }
                else { return false; }
            }
        }

        public List<Pago> buscarPagosUsuario(Usuario usuario, bool pagado)
        {
            List<Pago> pagoEstado = new List<Pago>();
            List<Pago> lista = null;

            foreach (Usuario u in contexto.usuarios)
            {
                if (u._dni == usuario._dni)
                {
                    lista = u._pagos;
                }
            }

            foreach (Pago p in lista)
            {
                if (p._pagado == pagado)
                    pagoEstado.Add(p);
            }

            return pagoEstado;
        }

        public Pago buscarPago(int id)
        {
            MessageBox.Show("ID: " + id);
            foreach (Pago p in contexto.pagos)
            {
                if (p._id_pago == id)
                {
                    return p;
                }
            }
            return null;

        }

        public bool confirmarEstadoPago(int id)
        {
            Pago pago = null;
            foreach (Pago p in contexto.pagos)
            {
                if (p._id_pago == id)
                {
                    pago = p;
                }
            }

            if (pago != null)
            {
                pago._pagado = true;
                contexto.pagos.Update(pago);
                contexto.SaveChanges();
                return true;
            }
            else { return false; }

        }

        public bool eliminarPago(int id)
        {
            Pago pago = null;
            foreach (Pago p in contexto.pagos)
            {
                if (p._id_pago == id)
                {
                    pago = p;
                }
            }

            if (pago != null)
            {
                contexto.pagos.Remove(pago);
                contexto.SaveChanges();
                return true;
            }
            else { return false; }

        }


        //######################ANTIGUAS DE BANCO##########################

        public bool verificarLogueo(int dni, string password)
        {
            if (this.validarUsuario(dni, password))
            {
                this.limpiarIntentosFallidos(dni);
                usuarioLogueado = this.buscarUsuario(dni);
                return true;
            }
            else
            {
                this.agregarIntentoFallido(dni);
                return false;
            }

        }

        public bool altaUsuario(int dni, string nombre, string apellido, string mail, string password)
        {
            return this.agregarUsuario(dni, nombre, apellido, mail, password, false, false, 0);
        }

        public bool altaUsuarioAdmin(int dni, string nombre, string apellido, string mail, string password)
        {
            return this.agregarUsuario(dni, nombre, apellido, mail, password, false, true, 0); ;
        }

        public bool crearCajaAhorro()
        {
            string cbu = obtieneSecuencia(usuarioLogueado);

            try
            {
                return this.agregarCA(usuarioLogueado, cbu);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Banco: " + ex.Message);
                return false;
            }


        }

        public string obtieneSecuencia(Usuario usuario)
        {
            //Genera secuencia unica de CBU o Tarjeta
            //El usuario se pasa porque el Admin podria crear TJ o CBU
            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
            string fecha = now.ToString("yyyyMMddHHmmssfff");

            return usuario._id_usuario + fecha;

        }

        public List<CajaDeAhorro> buscarCajasUsuario()
        {
            return this.buscarListaCA(usuarioLogueado).ToList();
        }


        public List<PlazoFijo> buscarPlazosFijosUsuario()
        {
            return this.buscarListaPF(usuarioLogueado);
        }

        public List<PlazoFijo> buscarPlazosFijosAdmin()
        {
            return contexto.plazosFijos.ToList();
        }

        public bool bajaPlazoFijo(int id)
        {
            return this.eliminarPlazoFijo(usuarioLogueado, id);
        }

        public bool altaTarjeta()
        {
            string idNuevaTarjeta = this.obtieneSecuencia(usuarioLogueado);
            TarjetaDeCredito tc = new TarjetaDeCredito(idNuevaTarjeta, 1, 500000, 0);
            return this.agregarTarjeta(usuarioLogueado, tc);
        }

        public List<TarjetaDeCredito> buscarTarjetas()
        {
            return this.buscarTarjetasUsuario(usuarioLogueado);
        }

        public List<TarjetaDeCredito> buscarTarjetasAdmin()
        {
            return contexto.tarjetas.ToList();
        }


        public bool PagarTarjeta(string numero, string cbu)
        {
            try
            {
                foreach (CajaDeAhorro caja in usuarioLogueado.cajas)
                {
                    if (caja._cbu.Equals(cbu))
                    {
                        foreach (TarjetaDeCredito tc in usuarioLogueado._tarjetas)
                        {
                            if (tc._numero.Equals(numero))
                            {
                                if (tc._consumos <= caja._saldo)
                                {
                                    double consumosAux = tc._consumos;
                                    caja._saldo = caja._saldo - consumosAux;
                                    tc._consumos = 0;
                                    contexto.usuarios.Update(usuarioLogueado);
                                    contexto.SaveChanges();
                                    AltaMovimiento(caja, "Consumos de Tarjeta", consumosAux);
                                    return true;

                                }
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return false;
        }

        public bool retirarSaldo(string cbu, double monto)
        {
            try
            {
                if(this.afectarSaldoCA(cbu, monto))
                { 
                    AltaMovimiento(cbu, "Retiro en cuenta", monto);
                    return true;
                } else { return false; }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error inesperado: " + ex.Message);
                return false;
            }
        }

        public bool depositarSaldo(string cbu, double monto)
        {
            try
            {
                return this.aumentarSaldoCA(cbu, monto);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error inesperado: " + ex.Message);
                return false;
            }
        }



        public bool AltaPago(double monto, string metodo, string detalle, long numero)
        {
            try
            {
                Pago pago = new Pago(monto, false, metodo, detalle, numero);
                usuarioLogueado._pagos.Add(pago);
                contexto.usuarios.Update(usuarioLogueado);
                contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;

            }
        }

        public List<Pago> buscarPagosUsuario(bool pagado)
        {
            return this.buscarPagosUsuario(usuarioLogueado, pagado);
        }

        public List<Pago> buscarPagosAdmin(bool pagado)
        {
            List<Pago> lista = new List<Pago>();
            foreach(Pago p in contexto.pagos)
            {
                if (p._pagado==pagado)
                    lista.Add(p);
            }
            return lista;
        }

        public static bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        public bool modificarPago(int id)
        {
            //(double monto, string metodo, long numero)
            Pago pago = this.buscarPago(id);
            try
            {
                if (this.confirmarAltaPago(pago._monto, pago._metodo, pago._id_metodo))
                {
                    return this.confirmarEstadoPago(id);
                }
                else { return false; }
            }
            catch { return false; }
        }


        public bool AltaMovimiento(CajaDeAhorro caja, string detalle, double monto)
        {
            try
            {
                Movimiento movimiento = new Movimiento(caja._id_caja, detalle, monto, DateTime.Now);
                //contexto.movimientos.Add(movimiento);
                caja._movimientos.Add(movimiento);
                contexto.cajas.Update(caja);
                contexto.usuarios.Update(usuarioLogueado);
                contexto.SaveChanges();

                return true;
            }
            catch (Exception ex) { return false; }
        }

        public bool AltaMovimiento(string cbu, string detalle, double monto)
        {
            try
            {
                CajaDeAhorro? caja = contexto.cajas.Where(caja => caja._cbu == cbu).FirstOrDefault();
                Movimiento movimiento = new Movimiento(caja._id_caja, detalle, monto, DateTime.Now);
                //contexto.movimientos.Add(movimiento);
                caja._movimientos.Add(movimiento);
                contexto.cajas.Update(caja);
                contexto.usuarios.Update(usuarioLogueado);
                contexto.SaveChanges();

                return true;
            }
            catch (Exception ex) { return false; }
        }


        public List<List<string>> BuscarMovimiento(string cbuCaja, string detalle = "default", DateTime? fecha = null, float monto = 0)
        {


       



            List<List<string>> listaStringMovimientosFiltrados = new List<List<string>>();
            //Busca Id de caja de ahoro
            CajaDeAhorro? caja = usuarioLogueado.cajas.Where(caja => caja._cbu == cbuCaja).FirstOrDefault();

            MessageBox.Show(caja.ToString());

            foreach (Movimiento movimiento in contexto.movimientos)
            {
                MessageBox.Show(movimiento.ToString());

                if (caja._id_caja == movimiento._id_CajaDeAhorro) { 

                if (movimiento._detalle == detalle || movimiento._fecha.Date == fecha.Value.Date || movimiento._monto == monto)


                {
                    listaStringMovimientosFiltrados.Add(new List<string>() { caja._cbu, movimiento._detalle, movimiento._monto.ToString() });


                }

                }

            }






            return listaStringMovimientosFiltrados;
        }

        public List<Usuario> listarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            foreach(Usuario u in contexto.usuarios)
            {
                if (u!=usuarioLogueado)
                {
                    lista.Add(u);
                }
            }
            return lista;

        }

        public bool eliminarUsuario(int id)
        {
            Usuario usuarioAux = null;

            foreach(Usuario u in contexto.usuarios)
            {
                if(u._id_usuario==id)
                {
                    usuarioAux= u;
                }
            }

            try { 
                contexto.usuarios.Remove(usuarioAux);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error eliminando el usuario");
                return false;
            }
        }

        public bool desbloquearUsuario(int id)
        {
            Usuario usuarioAux = null;

            foreach (Usuario u in contexto.usuarios)
            {
                if (u._id_usuario==id)
                {
                    usuarioAux= u;
                }
            }

            try
            {
                usuarioAux._bloqueado = false;
                contexto.usuarios.Update(usuarioAux);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error eliminando el usuario");
                return false;
            }
        }

        public List<CajaDeAhorro> buscarCajasAdmin()
        {
            return contexto.cajas.ToList();
        }

        public bool eliminarTitularCaja(int id, string cbu)
        {
            CajaDeAhorro caja = null;
            Usuario usuario = null;
            foreach(CajaDeAhorro ca in contexto.cajas)
            {
                if (ca._cbu.Equals(cbu))
                {
                    foreach(Usuario u in ca.titulares)
                    {
                        if(u._id_usuario==id)
                        { 
                            caja= ca;
                            usuario = u;
                        }

                    }
                }
            }

            if (caja!=null)
            {
                //caja.titulares.Remove(usuario);
                usuario.cajas.Remove(caja);
                contexto.usuarios.Update(usuario);
                contexto.SaveChanges();
                return true;
            }
            return false;
        }

        public bool agregarTitularCaja(int id, string cbu)
        {
            CajaDeAhorro caja = null;
            Usuario usuario = null;
            foreach (CajaDeAhorro ca in contexto.cajas)
            {
                if (ca._cbu.Equals(cbu))
                {
                    caja= ca;
                }
            }
            foreach (Usuario u in contexto.usuarios)
            {
                if (u._id_usuario==id)
                {
                    usuario = u;
                }

            }

            if (caja!=null && usuario!=null)
            {
                usuario.cajas.Add(caja);
                contexto.usuarios.Update(usuario);
                contexto.SaveChanges();
                return true;
            }
            return false;
        }

        public string obtenerNombre()
        {
            return usuarioLogueado._apellido + ", " + usuarioLogueado._nombre;
        }

        public bool esAdmin()
        {
            return usuarioLogueado._esUsuarioAdmin;
        }


        public void cerrar()
        {

            contexto.Dispose();
        }




    }
}
