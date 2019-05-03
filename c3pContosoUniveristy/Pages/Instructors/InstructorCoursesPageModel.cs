using DataModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c3pContosoUniveristy.Pages.Instructors
{
    public class InstructorCoursesPageModel : PageModel
    {

        public List<AssignedCourseData> AssignedCourseDataList { get; set; }
        public void PopulateAssignedCourseData(dbContext _context, Instructor instructor)
        {
            var allCourse = _context.Courses;
            var instructorCourse = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            AssignedCourseDataList = new List<AssignedCourseData>();

            foreach (var item in allCourse)
            {
                AssignedCourseDataList.Add(new AssignedCourseData
                {
                    CourseID = item.CourseID,
                    Title = item.Title,
                    Assigned = instructorCourse.Contains(item.CourseID)

                });

            }

        }

        public void UpdateInsructorCourse(dbContext _context, string[] selectedCourse, Instructor instructorToUpdate)
        {
            if (selectedCourse == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourse);
            var instructorCoursesHS = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));

            foreach (var item in _context.Courses)
            {
                if (selectedCoursesHS.Contains(item.CourseID.ToString()))
                {
                    if (!instructorCoursesHS.Contains(item.CourseID))
                    {
                        instructorToUpdate.CourseAssignments.Add(
                            new CourseAssignment
                            {
                                InstructorID=instructorToUpdate.ID,
                                CourseID=item.CourseID
                            });
                    }

                }
                else
                {
                    if (instructorCoursesHS.Contains(item.CourseID))
                    {
                        var courseToRemove = instructorToUpdate.CourseAssignments.SingleOrDefault(i => i.CourseID == item.CourseID);
                        _context.Remove(courseToRemove);

                    }
                }

            }

        }

    }
}
