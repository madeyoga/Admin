using System.ComponentModel.DataAnnotations;

namespace MyAdmin.Admin.Models;

public class DummyModel
{
    [Key]
    public int Id { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = "test@gmail.com";

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Note { get; set; } = "Test";

    [Required]
    [DataType(DataType.Date)]
    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    [DataType(DataType.DateTime)]
    public DateTime? CreatedDateTime { get; set; }
}
