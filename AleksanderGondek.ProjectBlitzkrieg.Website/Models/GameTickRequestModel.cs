namespace AleksanderGondek.ProjectBlitzkrieg.Website.Models
{
    public class GameTickRequestModel
    {
        public string ExectutionType { get; set; }
        public int Workers { get; set; }
        public int MaximumIterations { get; set; }
        public int MaxiumumSimulations { get; set; }
    }
}