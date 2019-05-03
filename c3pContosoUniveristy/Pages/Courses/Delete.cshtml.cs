using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c3pContosoUniveristy.Pages.Courses
{
    public class DeleteModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public DeleteModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? errorMessage = false)
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

            if (errorMessage.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed, Try againt";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses.FindAsync(id);

            if (Course == null)
            {
                return NotFound();
            }

            try
            {

                _context.Courses.Remove(Course);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");

            }
            catch (DbUpdateException)
            {

                return RedirectToPage("./Delete", new { id, errorMessage = true });
            }

            
        }
    }
}
