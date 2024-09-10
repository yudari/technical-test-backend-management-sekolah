using System.ComponentModel.DataAnnotations;

public class Class
{
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string ClassName { get; set; }
}