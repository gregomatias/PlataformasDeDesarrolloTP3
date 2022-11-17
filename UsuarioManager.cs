using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TP1
{
    internal class UsuarioManager
    {
        private MyContext contexto;
        public UsuarioManager()
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
                if (u._dni== dni)
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
                if (u._dni==dni)
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
                if (u._dni==dni)
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
                if (u._dni==dni)
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
                CajaDeAhorro nuevo = new CajaDeAhorro(cbu,0);
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
                if (u._dni==usuario._dni)
                {
                    lista = u.cajas;
                }
            }

            return lista;

        }

        public bool agregarPlazoFijo(Usuario usuario, PlazoFijo plazoFijo, string cbu)
        {
            if(afectarSaldoCA(usuario, cbu, plazoFijo._monto))
            {
                MessageBox.Show("Llegamos joya");
                try
                {
                    usuario._plazosFijos.Add(plazoFijo);
                    contexto.usuarios.Update(usuario);
                    contexto.SaveChanges();
                    return true;
                }
                catch (Exception ex) { MessageBox.Show("agregarCA: " + ex.Message + ex.InnerException.Message); return false; }
            }

            return false;
        }

        public List<PlazoFijo> buscarListaPF(Usuario usuario)
        {
            foreach (Usuario u in contexto.usuarios)
            {
                if (u._dni==usuario._dni)
                {
                    return u._plazosFijos;
                }
            }

            return null;
        }


        public bool afectarSaldoCA(Usuario usuario, string cbu, double monto)
        {
            foreach(CajaDeAhorro caja in usuario.cajas)
            {
                if (caja._cbu.Equals(cbu))
                {
                    if (caja._saldo >= monto)
                    {
                        caja._saldo = caja._saldo - monto;
                        contexto.usuarios.Update(usuario);
                        contexto.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool eliminarPlazoFijo(Usuario usuario, int id)
        {
            foreach (PlazoFijo p in usuario._plazosFijos)
            {
                if (p._id_plazoFijo==id && p._pagado)
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
                if (u._dni==usuario._dni)
                {
                    return u._tarjetas;
                }
            }

            return null;
        }

        public bool pagarTarjeta(Usuario usuario, string numero, string cbu)
        {
            foreach(CajaDeAhorro caja in usuario.cajas)
            {
                if(caja._cbu.Equals(cbu))
                {
                    foreach(TarjetaDeCredito tc in usuario._tarjetas)
                    {
                        if (tc._numero.Equals(numero))
                        {
                            if(tc._consumos <= caja._saldo)
                            {
                                
                                caja._saldo = caja._saldo - tc._consumos;
                                tc._consumos = 0;
                                contexto.usuarios.Update(usuario);
                                contexto.SaveChanges();
                                return true;

                            }
                        }
                    }

                }
            }

            return false;
        }

        public bool aumentarSaldoCA(Usuario usuario, string cbu, double monto)
        {
            foreach (CajaDeAhorro caja in usuario.cajas)
            {
                if (caja._cbu.Equals(cbu))
                {
                    caja._saldo = caja._saldo + monto;
                    contexto.usuarios.Update(usuario);
                    contexto.SaveChanges();
                    return true;

                }
            }

            return false;
        }

        public bool altaTransferencia(string cbuOrigen, string cbuDestino, double monto)
        {
            CajaDeAhorro cajaOrigen = null;
            foreach(CajaDeAhorro co in contexto.cajas)
            {
                if (co._cbu.Equals(cbuOrigen))
                {
                    cajaOrigen = co;
                }

            }

            try { 
                if (cajaOrigen._saldo >= monto)
                {
                    cajaOrigen._saldo = cajaOrigen._saldo - monto;

                }
            } catch(Exception ex) { return false; }

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
                return true;

            }
            catch (Exception ex) { return false; }

            return false;
        }

        // usuarios.altaPago(monto, metodo, detalle, numero);

        public bool altaPago(Usuario usuario, double monto, string metodo, string detalle, long numero)
        {
            Pago pago = new Pago(monto, false, metodo, detalle, numero);
            usuario._pagos.Add(pago);
            contexto.usuarios.Update(usuario);
            contexto.SaveChanges();
            return true;

        }

        public bool confirmarAltaPago(double monto, string metodo, long numero)
        {
            TarjetaDeCredito tarjeta = null;
            CajaDeAhorro caja = null;
            MessageBox.Show("Metodo: " + metodo);
            if (metodo.Equals("TC"))
            {
                foreach(TarjetaDeCredito tc in contexto.tarjetas)
                {
                    if (long.Parse(tc._numero) == numero)
                    {
                        tarjeta = tc;
                        MessageBox.Show("Encontre la tarjeta");
                    }
                    MessageBox.Show("Las recorri");

                }

                if(tarjeta != null) { 
                    tarjeta._consumos = tarjeta._consumos + monto;
                    contexto.tarjetas.Update(tarjeta);
                    contexto.SaveChanges();
                    return true;
                } else { return false; }


            } else
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
                        return true;
                    } else { return false; }

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
                if (u._dni==usuario._dni)
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
            MessageBox.Show("ID: "+id);
            foreach (Pago p in contexto.pagos)
            {
                if (p._id_pago==id)
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
                if (p._id_pago==id)
                {
                    pago = p;
                }
            }
            
            if(pago!=null)
            {
                pago._pagado = true;
                contexto.pagos.Update(pago);
                contexto.SaveChanges();
                return true;
            } else { return false; }

        }

        public bool eliminarPago(int id)
        {
            Pago pago = null;
            foreach (Pago p in contexto.pagos)
            {
                if (p._id_pago==id)
                {
                    pago = p;
                }
            }

            if (pago!=null)
            {
                contexto.pagos.Remove(pago);
                contexto.SaveChanges();
                return true;
            }
            else { return false; }

        }

    }
}
