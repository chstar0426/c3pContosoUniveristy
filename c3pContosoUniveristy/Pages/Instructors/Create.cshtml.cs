using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModels;

namespace c3pContosoUniveristy.Pages.Instructors
{
    public class CreateModel : InstructorCoursesPageModel
    {
        private readonly DataModels.dbContext _context;

        public CreateModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();

            PopulateAssignedCourseData(_context, instructor);
          
            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; }
        public FrmType frmType { get; set; } = FrmType.Write ;


        public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyInstructor = new Instructor();
            if (selectedCourses != null)
            {
                emptyInstructor.CourseAssignments = new List<CourseAssignment>();
                foreach (var item in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment
                    {
                        CourseID = int.Parse(item)
                    };
                emptyInstructor.CourseAssignments.Add(courseToAdd);
                }

            }

            if (await TryUpdateModelAsync<Instructor>(
                emptyInstructor, "Instructor",
                i=>i.FirstMidName, i=>i.LastName, i=>i.OfficeAssignment, i=>i.HireDate))
            {
                _context.Instructors.Add(emptyInstructor);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");

            }

            PopulateAssignedCourseData(_context, emptyInstructor);
            return Page();

        }
    }
}