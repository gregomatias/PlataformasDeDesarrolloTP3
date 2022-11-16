using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1
{
    internal class UsuarioCajaDeAhorro
    {
        public int id_caja { get; set; }
        public CajaDeAhorro caja { get; set; }
        public int id_usuario { get; set; }
        public Usuario user { get; set; }
        public UsuarioCajaDeAhorro() { }
    }
}
