using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class UsuarioVM
    {
        public int TotalUsuarios { get; set; }
        public int TotalArticulos { get; set; }
        public IEnumerable<Articulo> Articulos { get; set; }
        public IEnumerable<ApplicationUser> Usuarios { get; set; }
    }
}
