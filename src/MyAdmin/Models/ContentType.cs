using System.ComponentModel.DataAnnotations;

namespace MyAdmin.Admin;

public class ContentType
{
    [Key]
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Model { get; set; }
}
