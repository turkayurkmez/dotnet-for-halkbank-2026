using DependencyInjectionLifeCyycle.Models;
using DependencyInjectionLifeCyycle.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DependencyInjectionLifeCyycle.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISingleton _singleton;
        private readonly IScoped _scoped;
        private readonly ITransient _transient;
        private readonly GuidService _service;

        public HomeController(IScoped scoped, ITransient transient, ISingleton singleton, GuidService service)
        {
            _scoped = scoped;
            _transient = transient;
            _singleton = singleton;
            _service = service;
        }
        public IActionResult Index()
        {
            ViewBag.Singleton = _singleton.Guid.ToString();
            ViewBag.Transient = _transient.Guid.ToString();
            ViewBag.Scoped = _scoped.Guid.ToString();


            ViewBag.ServiceSingleton = _service.Singleton.Guid.ToString();
            ViewBag.ServiceTransient = _service.Transient.Guid.ToString();
            ViewBag.ServiceScoped = _service.Scoped.Guid.ToString();
            return View();
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
