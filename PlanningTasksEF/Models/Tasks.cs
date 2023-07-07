namespace PlanningTasksEF.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Company { get; set; }
        public string Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
    }
}
