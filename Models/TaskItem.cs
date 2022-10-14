using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuyouKadai.Models
{
    public class TaskItem
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "1～20文字で入力してください")]
        [Column(TypeName = "varchar(20)")]
        [RegularExpression(@".{1,20}", ErrorMessage = "1～20文字で入力してください")]
        [Display(Name = "タイトル")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "1～255文字で入力してください")]
        [Column(TypeName = "varchar(255)")]
        [RegularExpression(@".{1,255}", ErrorMessage = "1～255文字で入力してください")]
        [Display(Name = "内容詳細")]
        public string? Body { get; set; }

        public User? Pic { get; set; }

        [Required(ErrorMessage = "担当者を選択してください")]
        [Display(Name = "担当者")]
        public int PicID { get; set; }

        [Required(ErrorMessage = "締め切りを入力してください")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "締め切り")]
        public DateTime Deadline { get; set; }

        [Display(Name = "登録者")]
        public User? Reg { get; set; }
        public int RegID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "登録日時")]
        public DateTime Created_at { get; set; }

        [Display(Name = "更新者")]
        public int? Updated_byID { get; set; }

        [NotMapped]
        public string? Updated_by_name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "更新日時")]
        public DateTime? Updated_at { get; set; }

        [Required(ErrorMessage = "ステータスを選択してください")]
        [Display(Name = "ステータス")]
        public int StatusID { get; set; }

        public Status? Status { get; set; }

        [Required(ErrorMessage = "優先度を選択してください")]
        [Display(Name = "優先度")]
        public int PriorityID { get; set; }

        public Priority? Priority { get; set; }

    }
}
