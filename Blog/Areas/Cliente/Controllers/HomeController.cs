using Blog.Models;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Printing;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        //Primera versi�n p�gina de inicio sin paginaci�n
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    HomeVM homeVM = new HomeVM()
        //    {
        //        Sliders = _contenedorTrabajo.Slider.GetAll(),
        //        ListArticulos = _contenedorTrabajo.Articulo.GetAll()
        //    };

        //    //Esta l�nea es para poder saber si estamos en el home o no
        //    ViewBag.IsHome = true;

        //    return View(homeVM);
        //}

        //Segunda versi�n p�gina de inicio con paginaci�n
        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 6)
        {

            var articulos = _contenedorTrabajo.Articulo.AsQueryable();

            // Paginar los resultados
            var paginatedEntries = articulos.Skip((page - 1) * pageSize).Take(pageSize);

            HomeVM homeVM = new HomeVM()
            {
                Sliders = _contenedorTrabajo.Slider.GetAll(),
                ListArticulos = paginatedEntries.ToList(),
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(articulos.Count() / (double)pageSize)
        };

            //Esta l�nea es para poder saber si estamos en el home o no
            ViewBag.IsHome = true;

            return View(homeVM);
        }


        //Para buscador
        [HttpGet]
        public IActionResult ResultadoBusqueda(string searchString, int page = 1, int pageSize = 3)
        {
            var articulos = _contenedorTrabajo.Articulo.AsQueryable();

            // Filtrar por t�tulo si hay un t�rmino de b�squeda
            if (!string.IsNullOrEmpty(searchString))
            {
                articulos = articulos.Where(e => e.Nombre.Contains(searchString));
            }

            // Paginar los resultados
            var paginatedEntries = articulos.Skip((page - 1) * pageSize).Take(pageSize);

            // Crear el modelo para la vista
            var model = new ListaPaginada<Articulo>(paginatedEntries.ToList(), articulos.Count(), page, pageSize, searchString);
            return View(model);
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(id);
            return View(articuloDesdeBd);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
