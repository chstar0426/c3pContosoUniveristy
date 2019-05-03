using DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c3pContosoUniveristy.ViewComponents
{
    public class CourseFormViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(CourseFrm cfrm)
        {
            return View(cfrm);

        }
    }
}
