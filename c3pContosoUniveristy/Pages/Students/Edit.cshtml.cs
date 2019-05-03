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

namespace c3pContosoUniveristy.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public EditModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public FrmType frmType { get; set; } = FrmType.Modify;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();

        }

        public async Task<IActionResult> OnPostAsync(int? id)
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

            var studentToUpdate = await _context.Students.FindAsync(id);

            if (await TryUpdateModelAsync<Student>(
                studentToUpdate, "student",
                s=>s.FirstMidName, s=>s.LastName, s=>s.EnrollmentDate))
            {
                await _context.SaveChangesAsync();
                //패스를 이용하여 Return 관련 변수 화인
                return Redirect("./Index" + Request.Query["Path"].ToString());


            }

            return Page();
            
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
