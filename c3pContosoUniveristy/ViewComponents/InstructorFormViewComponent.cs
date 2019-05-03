
using c3pContosoUniveristy.Pages.Students;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c3pContosoUniveristy.ViewComponents
{
    public class InstructorFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(InstructorFrm frm)
        {
            return View(frm);
            
        }

    }
}
