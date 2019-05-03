using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModels;
using Microsoft.AspNetCore.Http;

namespace c3pContosoUniveristy.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public CreateModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; }
        public FrmType frmType { get; set; } = FrmType.Write;


        public IActionResult OnGet()
        {
            return Page();

        }


        public async Task<IActionResult> OnPostAsync()
        {

            var formSecurity = Request.Form["SecurityText"].ToString();
            var sessionSecurity = HttpContext.Session.GetString("SecurityText");


            if (formSecurity != sessionSecurity)
            {

                ErrorMessage ="(시크릿트 문자를 확인하세요)";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",
                s=>s.FirstMidName, s=>s.LastName, s=>s.EnrollmentDate))
            {
                _context.Students.Add(emptyStudent);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");

            }

            return Page();

        }
    }
}