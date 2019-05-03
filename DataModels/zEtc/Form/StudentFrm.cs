namespace DataModels
{
    public class StudentFrm
    {
        public FrmType FrmType { get; set; }
        public string  ErrorMessage { get; set; }
        public Student Student { get; set; }
        public string ReturnUrl { get; set; }

    }
}
