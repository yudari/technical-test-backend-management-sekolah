using System.ComponentModel.DataAnnotations;

public class Teacher
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string TeacherName { get; set; }
}