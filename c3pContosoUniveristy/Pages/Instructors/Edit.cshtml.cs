using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c3pContosoUniveristy.Pages.Instructors
{
    public class EditModel : InstructorCoursesPageModel
    {
        private readonly DataModels.dbContext _context;

        public EditModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Instructor Instructor { get; set; }
        public FrmType frmType { get; set; } = FrmType.Modify;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await _context.Instructors
                .Include(i=>i.OfficeAssignment)
                .Include(i=>i.CourseAssignments)
                    .ThenInclude(i=>i.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Instructor == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(_context, Instructor); // AssignedCourseDataList
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCourses)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Instructor>(
                instructorToUpdate, "instructor",
                i=>i.FirstMidName, i=>i.LastName, i=>i.OfficeAssignment, i=>i.HireDate))
            {
                if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;

                }
                UpdateInsructorCourse(_context, selectedCourses, instructorToUpdate); 

                await _context.SaveChangesAsync();
                return Redirect("./Index" + Request.Query["Path"].ToString());
            }
            UpdateInsructorCourse(_context, selectedCourses, instructorToUpdate);
            PopulateAssignedCourseData(_context, instructorToUpdate);
            return Page();

        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}
