using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c3pContosoUniveristy.ViewComponents
{
    public class SecurityTextsViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke()
        {
            
            string imsiValue = string.Empty;

            Random random = new Random();

            char c1 = (char)random.Next(65, 90);
            char c2 = (char)random.Next(48, 57);
            char c3 = (char)random.Next(97, 122);
            char c4 = (char)random.Next(48, 57);

            imsiValue = $"{c1} {c2} {c3} {c4}";

            HttpContext.Session.SetString("SecurityText",$"{c1}{c2}{c3}{c4}");

                    
            return View("Default", imsiValue);

        }

    }
}
