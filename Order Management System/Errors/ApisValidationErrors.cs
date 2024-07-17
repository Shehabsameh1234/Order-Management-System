namespace Order_Management_System.Errors
{
    public class ApisValidationErrors:ApisResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApisValidationErrors()
            : base(400)
        {
            Errors = new List<string>();
        }
    }
}
