using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c3pContosoUniveristy.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public IndexModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public InstructorIndexData Instructor { get;set; }
        public SearchingVar searchingVar { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        #region 페이징관련 변수
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        #endregion

        public async Task OnGetAsync(int? id, int? courseID)
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
                { "LastName", "성" },
                { "FirstMidName", "이름" },
                { "Name", "전체" }
            };

            //////////////////////////////////////////////////////////////////////////////


            Instructor = new InstructorIndexData();
            var iqInstructor = (IQueryable<Instructor>)_context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                        .ThenInclude(c => c.Department);
                //.Include(i => i.CourseAssignments)
                //    .ThenInclude(c => c.Course)
                //        .ThenInclude(c => c.Enrollments)
                //            .ThenInclude(e => e.Student);

            if (searchMode)
            {
                switch (searchField)
                {
                    case "LastName":
                        iqInstructor = iqInstructor.Where(s => s.LastName.Contains(searchQuery));
                        break;

                    case "FirstMidName":
                        iqInstructor = iqInstructor.Where(s => s.FirstMidName.Contains(searchQuery));
                        break;

                    default:
                        iqInstructor = iqInstructor.Where(s => s.FirstMidName.Contains(searchQuery) || s.LastName.Contains(searchQuery));
                        break;
                }

            }

            iqInstructor = iqInstructor.OrderByDescending(i => i.ID);

            

            //[1] 쿼리스트링에 따른 페이지 보여주기
            if (!string.IsNullOrEmpty(Request.Query["Page"]))
            {
                // Page는 보여지는 쪽은 1, 2, 3, ... 코드단에서는 0, 1, 2, ...
                pageIndex = Convert.ToInt32(Request.Query["Page"]) - 1;
                //Response.Cookies.Append("Page", pageIndex.ToString());
            }

            TotalCount = iqInstructor.Count();
            PageIndex = pageIndex + 1;

            Instructor.Instructors = await iqInstructor
                .Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();


            if (id != null)
            {
                InstructorID = id.Value;
                var inss = Instructor.Instructors.Where(i => i.ID == id.Value).Single();
                Instructor.Courses = inss.CourseAssignments.Select(c => c.Course);

            }

            if (courseID != null)
            {
                CourseID = courseID.Value;

                //Instructor.Enrollments =_context.Courses
                //    .Include(c=>c.Enrollments)
                //        .ThenInclude(e=>e.Student)
                //     .Where(x => x.CourseID == courseID).Single().Enrollments;

                var selectedCourse = Instructor.Courses.SingleOrDefault(c => c.CourseID == courseID);
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach (var item in selectedCourse.Enrollments)
                {
                    await _context.Entry(item).Reference(x => x.Student).LoadAsync();
                }
                Instructor.Enrollments = selectedCourse.Enrollments;
            }

        }
    }
}
