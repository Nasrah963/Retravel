namespace DB_Project.Models
{
    public class Employee
    {
        public int Employee_ID { get; set; }
        public string Role { get; set; }
        public string Trip { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public int Num_of_children { get; set; }
        public int Num_of_Adults { get; set; }

    }
}
