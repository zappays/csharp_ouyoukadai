using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuyouKadai.Models
{
    public class Auth
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(10)")]
        public string? Auth_name { get; set; }

        public ICollection<User>? Users { get; set; }

    }
}
