using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModels;
using Microsoft.AspNetCore.Http;

namespace c3pContosoUniveristy.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public CreateModel(DataModels.dbContext context)
        {
            _context = context;
        }

        
        [BindProperty]
        public Course Course { get; set; }
        public string ErrorMessage { get; set; }
        public FrmType frmType { get; set; } = FrmType.Write;

        public IActionResult OnGet()
        {
            ViewData["Department"] = new SelectList(_context.Departments.OrderBy(d=>d.Name),
                "DepartmentID", "Name");
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var formSecurity = Request.Form["SecurityText"].ToString();
            var sessionSecurity = HttpContext.Session.GetString("SecurityText");


            if (formSecurity != sessionSecurity)
            {

                ErrorMessage = "(시크릿트 문자를 확인하세요)";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyCourse = new Course();

            if (await TryUpdateModelAsync<Course>(
                emptyCourse, "course",
                c => c.CourseID, c => c.Title, c => c.Credits, c=>c.DepartmentID))
            {
                _context.Courses.Add(emptyCourse);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");

            }

            ViewData["Department"] = new SelectList(_context.Departments.OrderBy(d => d.Name),
                "DepartmentID", "name", emptyCourse.DepartmentID);
            return Page();


        }

    }
}