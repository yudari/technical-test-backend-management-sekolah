using System.ComponentModel.DataAnnotations;

public class Student
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string StudentName { get; set; }
    [Required]
    public DateTime DateBirth { get; set; }
}