namespace Domain.Entity
{
    public class UserData
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool? Married { get; set; }
        public string? Phone { get; set; }
        public decimal? Salary { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
