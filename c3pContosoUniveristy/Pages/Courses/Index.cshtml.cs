﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c3pContosoUniveristy.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public IndexModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get;set; }
        public SearchingVar searchingVar { get; set; }


        #region 페이징관련 변수
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        #endregion

        public async Task OnGetAsync()
        {

            int pageIndex = 0;
            int pageSize = 3;

            string searchField = string.Empty;
            string searchQuery = string.Empty;
            bool searchMode = false;

            ////////////////////////////////////////////////////////////////////////////////////

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
                { "CourseID", "강의번호" },
                { "Title", "강의명" },
                { "Department", "부서" }

            };

            //////////////////////////////////////////////////////////////////////////////


            var iCourse = (IQueryable<Course>)_context.Courses
                .Include(c => c.Department);

            if (searchMode)
            {
                switch (searchField)
                {
                    case "CourseID":
                        iCourse = iCourse.Where(s => s.CourseID == int.Parse(searchQuery));
                        break;

                    case "Title":
                        iCourse = iCourse.Where(s => s.Title.Contains(searchQuery));
                        break;

                    default:
                        iCourse = iCourse.Where(s => s.Department.Name.Contains(searchQuery));
                        break;

                }

            }

            //[1] 쿼리스트링에 따른 페이지 보여주기
            if (!string.IsNullOrEmpty(Request.Query["Page"]))
            {
                // Page는 보여지는 쪽은 1, 2, 3, ... 코드단에서는 0, 1, 2, ...
                pageIndex = Convert.ToInt32(Request.Query["Page"]) - 1;
                //Response.Cookies.Append("Page", pageIndex.ToString());
            }

            TotalCount = iCourse.Count();
            PageIndex = pageIndex + 1;

            Course = await iCourse
                .Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();



        }
    }
}
