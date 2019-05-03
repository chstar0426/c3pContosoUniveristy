using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModels;
using Microsoft.AspNetCore.Http;

namespace c3pContosoUniveristy.Pages.Courses
{
    public class EditModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public EditModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; }
        public string ErrorMessage { get; set; }
        public FrmType frmType { get; set; } = FrmType.Modify;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses
                .Include(c => c.Department).AsNoTracking().FirstOrDefaultAsync(m => m.CourseID == id);

            if (Course == null)
            {
                return NotFound();
            }
            ViewData["Department"] = new SelectList(_context.Departments.OrderBy(d => d.Name),
                "DepartmentID", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            // 보안문자는 글쓰기에만 사용
            //var formSecurity = Request.Form["SecurityText"].ToString();
            //var sessionSecurity = HttpContext.Session.GetString("SecurityText");

            //if (formSecurity != sessionSecurity)
            //{
            //    ViewData["Department"] = new SelectList(_context.Departments.OrderBy(d => d.Name),
            //    "DepartmentID", "Name");

            //    ErrorMessage = "(시크릿트 문자를 확인하세요)";
            //    return Page();

            //}

            if (!ModelState.IsValid)
            {
                return NotFound();

            }

            var courseToUpdate = await _context.Courses.FindAsync(id);


            if (await TryUpdateModelAsync<Course>(
                courseToUpdate, "course",
                c=>c.CourseID, c=>c.Title, c=>c.Credits, c=>c.DepartmentID))
            {
                await _context.SaveChangesAsync();
                //패스를 이용하여 Return 관련 변수 화인
                return Redirect("./Index" + Request.Query["Path"].ToString());


            }

            ViewData["Department"] = new SelectList(_context.Departments.OrderBy(d => d.Name),
                "DepartmentID", "Name");

            return Page();

        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
