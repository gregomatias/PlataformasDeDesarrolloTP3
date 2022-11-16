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


    }
}
