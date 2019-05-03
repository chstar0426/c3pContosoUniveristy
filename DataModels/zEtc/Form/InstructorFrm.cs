namespace DataModels
{
    public class InstructorFrm
    {
        public FrmType FrmType { get; set; }
        //public string  ErrorMessage { get; set; }
        public Instructor Instructor { get; set; }
        public System.Collections.Generic.List<AssignedCourseData> AssignedCourseDataList { get; set; }
        public string ReturnUrl { get; set; }

    }
}
