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
        private string _currentLanguage;

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

            if (string.IsNullOrEmpty(_currentLanguage))
            {
                // SetDefaulLanguage();
            }

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
            //TryChangeLanguage();
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

        private void TryChangeLanguage(string culture)
        {

            IRequestCultureFeature feature = HttpContext.Features.Get<IRequestCultureFeature>();
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                //  CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(feature.RequestCulture.UICulture.ToString())),
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
        }

        private void SetDefaulLanguage()
        {
            var culture = CurrentLanguage;
            //if (lang == "et")
            //{
            //    lang = "ee";
            //}
            var feature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var lang = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();

            TryChangeLanguage(culture);


        }

        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                {
                    return _currentLanguage;
                }



                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                }

                return _currentLanguage;
            }
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {

            returnUrl = SetNewCultureURL(culture, returnUrl);



            //Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        private string SetNewCultureURL(string culture, string returnUrl)
        {
            var urlItems = returnUrl.Split('/').ToList();
            if (urlItems.Count > 3)
            {
                urlItems.RemoveRange(0, 2);
                returnUrl = "/" + culture;
                foreach (var item in urlItems)
                {
                    returnUrl += "/" + item;
                }
                return returnUrl;
            }
            
            return /*"/" + culture +*/ returnUrl;
        }

    }
}
