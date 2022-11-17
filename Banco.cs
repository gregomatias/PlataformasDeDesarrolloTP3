using System;
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
        private UsuarioManager usuarios;
        private CajaDeAhorroManager cajas;
        private Usuario usuarioLogueado;

        public Banco() {
            usuarios = new UsuarioManager();
            cajas = new CajaDeAhorroManager();
        }

        public bool verificarLogueo(int dni, string password)
        {
            if (usuarios.validarUsuario(dni, password)) {
                usuarios.limpiarIntentosFallidos(dni);
                usuarioLogueado = usuarios.buscarUsuario(dni);
                return true;
            } else
            {
                usuarios.agregarIntentoFallido(dni);
                return false;
            }
            
        }

        public bool altaUsuario(int dni, string nombre, string apellido, string mail, string password)
        {
            return usuarios.agregarUsuario(dni, nombre, apellido, mail, password, false, false, 0);
        }

        public bool altaUsuarioAdmin(int dni, string nombre, string apellido, string mail, string password)
        { 
            return usuarios.agregarUsuario(dni, nombre, apellido, mail, password, false, true, 0); ;
        }

        public bool crearCajaAhorro()
        {
            string cbu = obtieneSecuencia(usuarioLogueado);
            
            try
            {
                return usuarios.agregarCA(usuarioLogueado, cbu);

            } catch (Exception ex)
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
            return usuarios.buscarListaCA(usuarioLogueado).ToList();
        }

        public bool crearPlazoFijo(double monto, double tasa, string cbu)
        {
            
            PlazoFijo plazoFijo = new PlazoFijo(monto, DateTime.Now.AddMonths(1), tasa);
            try { 
                return usuarios.agregarPlazoFijo(usuarioLogueado, plazoFijo, cbu);
            } catch (Exception ex) { MessageBox.Show("No se pudo crear el plazo fijo: " + ex.Message); return false; }

        }

        public List<PlazoFijo> buscarPlazosFijosUsuario()
        {
            return usuarios.buscarListaPF(usuarioLogueado);
        }

        public bool bajaPlazoFijo(int id)
        {
            return usuarios.eliminarPlazoFijo(usuarioLogueado, id);
        }

        public bool altaTarjeta()
        {
            string idNuevaTarjeta = this.obtieneSecuencia(usuarioLogueado);
            TarjetaDeCredito tc = new TarjetaDeCredito(idNuevaTarjeta, 1, 500000, 0);
            return usuarios.agregarTarjeta(usuarioLogueado, tc);
        }

        public List<TarjetaDeCredito> buscarTarjetas()
        {
            return usuarios.buscarTarjetasUsuario(usuarioLogueado);
        }

        public bool pagarTarjeta(string numero, string cbu)
        {
            return usuarios.pagarTarjeta(usuarioLogueado, numero, cbu);
        }

        public bool retirarSaldo(string cbu, double monto) 
        {
            try
            {
                return usuarios.afectarSaldoCA(usuarioLogueado, cbu, monto);

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
                return usuarios.aumentarSaldoCA(usuarioLogueado, cbu, monto);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error inesperado: " + ex.Message);
                return false;
            }
        }

        public bool transferir(string cbuOrigen, string cbuDestino, double monto)
        {
            return usuarios.altaTransferencia(cbuOrigen, cbuDestino, monto);       

        }

        public bool pagar(double monto, string metodo, string detalle, long numero)
        {
            return usuarios.altaPago(usuarioLogueado, monto, metodo, detalle, numero);
        }

        public List<Pago> buscarPagosUsuario(bool pagado)
        {
            return usuarios.buscarPagosUsuario(usuarioLogueado, pagado);
        }

        public static bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        public bool modificarPago(int id)
        {
            //(double monto, string metodo, long numero)
            Pago pago = usuarios.buscarPago(id);
            try {
                if (usuarios.confirmarAltaPago(pago._monto, pago._metodo, pago._id_metodo))
                { 
                    return usuarios.confirmarEstadoPago(id);
                } else { return false; }
            } catch { return false; }
        }

        public bool eliminarPago(int id)
        {
            return usuarios.eliminarPago(id);
        }

    }
}
