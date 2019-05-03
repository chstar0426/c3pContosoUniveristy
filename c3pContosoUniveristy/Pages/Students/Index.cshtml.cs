using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c3pContosoUniveristy.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public IndexModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public IList<Student> Student { get; set; }
        public SearchingVar searchingVar { get; set; }

        #region 페이징관련 변수
        public int TotalCount { get; set; }
            public int PageIndex { get; set; }
        #endregion


        public async Task OnGetAsync()
        {

            int pageIndex = 0;
            int pageSize = 3;


            ////////////////////////////////////////////////////////////////////////////////////

            string searchField = string.Empty;
            string searchQuery = string.Empty;
            bool searchMode = false;

            if (!String.IsNullOrEmpty(Request.Query["SearchField"]) &&
                 !String.IsNullOrEmpty(Request.Query["SearchQuery"]))
            {
                searchMode = true;
                searchField = Request.Query["SearchField"];
                searchQuery = Request.Query["SearchQuery"];

            }

            searchingVar = new SearchingVar();
            searchingVar.SearchMode = searchMode;
            searchingVar.SearchField = searchField;
            searchingVar.SearchQuery = searchQuery;
            searchingVar.DicField = new Dictionary<string, string>()
            {
                { "LastName", "성" },
                { "FirstMidName", "이름" },
                { "Name", "전체" }
            };

            //////////////////////////////////////////////////////////////////////////////

            var iStudent = (IQueryable<Student>)_context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course);

            if (searchMode)
            {
                switch (searchField)
                {
                    case "LastName":
                        iStudent = iStudent.Where(s => s.LastName.Contains(searchQuery));
                        break;

                    case "FirstMidName":
                        iStudent = iStudent.Where(s => s.FirstMidName.Contains(searchQuery));
                        break;

                    default:
                        iStudent = iStudent.Where(s => s.FirstMidName.Contains(searchQuery) || s.LastName.Contains(searchQuery));
                        break;
                }

            }


            iStudent = iStudent.OrderByDescending(s => s.ID);



            //[1] 쿼리스트링에 따른 페이지 보여주기
            if (!string.IsNullOrEmpty(Request.Query["Page"]))
            {
                // Page는 보여지는 쪽은 1, 2, 3, ... 코드단에서는 0, 1, 2, ...
                pageIndex = Convert.ToInt32(Request.Query["Page"]) - 1;
                //Response.Cookies.Append("Page", pageIndex.ToString());
            }

            TotalCount = iStudent.Count();
            PageIndex = pageIndex + 1;

            Student = await iStudent
                .Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();

        }
    }
}
