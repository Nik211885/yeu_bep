using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Attributes.Table;
using YeuBep.Const;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Errors;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            
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
