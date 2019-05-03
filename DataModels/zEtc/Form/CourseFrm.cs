namespace DataModels
{
    public class CourseFrm
    {
        public FrmType FrmType { get; set; }
        public string  ErrorMessage { get; set; }
        public Course Course { get; set; }
        public string ReturnUrl { get; set; }

    }
}
