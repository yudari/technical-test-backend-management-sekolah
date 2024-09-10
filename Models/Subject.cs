using System.ComponentModel.DataAnnotations;

public class Subject
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string SubjectName { get; set; }
}
