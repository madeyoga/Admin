using MyAdmin.Admin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

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

    [TabularInline]
    public List<Product>? Products { get; set; }

    public int ContentTypeId { get; set; }
    [ForeignKey("ContentTypeId")]
    public ContentType? ContentType { get; set; }

    public override string ToString()
    {
        return $"DummyModel object ({Id})";
    }
}
