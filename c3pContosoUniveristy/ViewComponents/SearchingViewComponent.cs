using DataModels;
using Microsoft.AspNetCore.Mvc;

namespace RazzorPagesExample.ViewComponents
{
    public class SearchingViewComponent : ViewComponent
    {
       
        public  IViewComponentResult Invoke(SearchingVar searchingVar)
        {
            return View(searchingVar);
        }

    }
}
