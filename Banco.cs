using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
