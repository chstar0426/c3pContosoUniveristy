using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments{ get; set; }


    }
}
