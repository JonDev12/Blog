using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Rotativa.AspNetCore;

using BlogCore.Models.ViewModels;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //Opción 1: Obtener todos los usuario
            //return View(_contenedorTrabajo.Usuario.GetAll());

            //Opción 2: Obtener todos los usuarios menos el que esté logueado, para no bloquearse el mismo
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));
        }

        //Agregar opcion de generar PDF de los usuarios
        //Agregar opcion de generar Excel de los usuarios

        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GenerarReporteUsuarios() 
        {
            var totalUsuarios = _contenedorTrabajo.Usuario.ContarUsuarios();
            var totalArticulos = _contenedorTrabajo.Usuario.ContarArticulos();
            var articulos = _contenedorTrabajo.Usuario.MostrarArticulos();
            var listarUsuarios = _contenedorTrabajo.Usuario.GetAll();

            var vm = new UsuarioVM
            {
                TotalUsuarios = totalUsuarios,
                TotalArticulos = totalArticulos,
                Articulos = articulos,
                Usuarios = listarUsuarios
            };

            // Generar el PDF
            return new ViewAsPdf("Reporte", vm){
                FileName = "Reporte-General.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(20, 15, 20, 15)
            };
        }
    }
}
