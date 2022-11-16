using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1
{
    internal class CajaDeAhorroManager
    {
        private MyContext contexto;
        public CajaDeAhorroManager()
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
                contexto.cajas.Load();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar tabla de usuarios");
            }
        }

    }
}
