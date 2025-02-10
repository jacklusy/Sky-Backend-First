using System.ComponentModel.DataAnnotations;

public class Position
{
    public int PositionId { get; set; }

    [Required]
    [MaxLength(30)]
    public string PositionName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }
}