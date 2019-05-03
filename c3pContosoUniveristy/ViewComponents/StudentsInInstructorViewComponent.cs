using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c3pContosoUniveristy.ViewComponents
{
    public class StudentsInInstructorViewComponent : ViewComponent
    {

        private readonly DataModels.dbContext _context;

        public StudentsInInstructorViewComponent(DataModels.dbContext context)
        {
            _context = context;
        }



        public IViewComponentResult Invoke(int courseID)
        {
             var Enrollments = _context.Courses
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                 .Where(x => x.CourseID == courseID).Single().Enrollments.ToList();

            return View(Enrollments);

        }
    }
}
