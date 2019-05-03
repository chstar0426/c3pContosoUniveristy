using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels
{
    public class EnrollmentDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime?  EnrollmentDate { get; set; }

        public int StudentCount { get; set; }
    }
}
