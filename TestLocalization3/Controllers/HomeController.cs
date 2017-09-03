using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TestLocalization3.Localization;

namespace TestLocalization3.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    //  [Route("{culture}/[controller]")]
    public class HomeController : Controller
    {
        
        [HttpGet]
        public IActionResult Index()
        {

            IRequestCultureFeature feature = HttpContext.Features.Get<IRequestCultureFeature>();
            Console.WriteLine("==================");
            Console.WriteLine($"Culture:{feature.RequestCulture.Culture}");
            Console.WriteLine($"UI Culture:{feature.RequestCulture.UICulture}");
            Console.WriteLine($"Provider:{feature.Provider}");

           // TryChangeLanguage();

            //CultureInfo.CurrentUICulture = new CultureInfo(feature.RequestCulture.Culture.ToString());
            //CultureInfo.CurrentUICulture = new CultureInfo(feature.RequestCulture.Culture.ToString());


            return View();
        }

        //  [Route("ShowMeTheCulture")]
        public string GetCulture()
        {
            return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
           // TryChangeLanguage();
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private void TryChangeLanguage()
        {
            IRequestCultureFeature feature = HttpContext.Features.Get<IRequestCultureFeature>();
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(feature.RequestCulture.UICulture.ToString())),
                new CookieOptions {Expires = DateTimeOffset.UtcNow.AddYears(1)});
        }

    }
}
