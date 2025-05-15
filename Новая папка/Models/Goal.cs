namespace ProgressTreeBot.Models
{
    public class Goal
    {
        public string Description { get; set; } = "";
        public bool IsCompletedToday { get; set; } = false;
        public DateTime LastCompletedDate { get; set; } = DateTime.MinValue;
    }
}