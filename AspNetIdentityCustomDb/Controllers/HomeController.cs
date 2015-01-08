using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspNetIdentityCustomDb.Controllers
{
    using Owin;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.Identity;

    public class HomeController : Controller
    {
        private Custom.Identity.RoleManager _roleManager;

        public HomeController()
        {
        }

        public HomeController(Custom.Identity.RoleManager roleManager)
        {
            RoleManager = roleManager;
        }

        public async Task<ActionResult> Index()
        {
            var userName = User != null ? User.Identity.GetUserName() : string.Empty;

            var role = await RoleManager.FindByNameAsync("Administrators");
            if (role == null)
            {
                await RoleManager.CreateAsync(new Custom.Identity.Role() { Id = 1, Name = "Administrators" });
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles="Administrators")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public Custom.Identity.RoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<Custom.Identity.RoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
    }
}