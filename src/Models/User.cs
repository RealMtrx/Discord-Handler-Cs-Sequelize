using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordHandler.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = string.Empty;

    [Column("created_at")]
    public long CreatedAt { get; set; }
}
