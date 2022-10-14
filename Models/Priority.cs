using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuyouKadai.Models
{
    public class Priority
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(10)")]
        [Display(Name = "優先度")]
        public string? Priority_name { get; set; }

        public ICollection<TaskItem>? TaskItems { get; set; }

    }
}
