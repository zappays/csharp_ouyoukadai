using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Resources;
using System.Text;

namespace OuyouKadai.Models
{
    public class User
    {
        [Range(1, int.MaxValue, ErrorMessage = "値は214748367以下にする必要があります")]
        [Required(ErrorMessage = "IDを入力してください")]
        [Display(Name = "ID")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "1～20文字で入力してください")]
        [Column(TypeName = "varchar(20)")]
        [RegularExpression(@".{1,20}", ErrorMessage = "1～20文字で入力してください")]
        [Display(Name = "ユーザ名")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "1～20文字で入力してください")]
        [DataType(DataType.Password)]
        [RegularExpression(@".{1,20}", ErrorMessage = "1～20文字で入力してください")]
        [Column(TypeName = "varchar(32)")]
        [Display(Name = "パスワード")]
        public string? Pass { get; set; }

        [Display(Name = "登録者")]
        public int RegID { get; set; }

        [NotMapped]
        public string? Reg_name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "登録日時")]
        public DateTime Created_at { get; set; }

        [Display(Name = "更新者")]
        public int? Updated_byID { get; set; }

        [NotMapped]
        public string? Updated_by_name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "更新日時")]
        public DateTime? Updated_at { get; set; }

        [Required(ErrorMessage = "権限を選択してください")]
        [Display(Name = "権限")]
        public int AuthID { get; set; }

        public Auth? Auth { get; set; }

        // パスワードをハッシュ化するメソッド
        public string PasswordHash(User user)
        {
            // MD5ハッシュ値を計算する文字列
            string s = user.Pass;
            // 文字列をbyte型配列に変換する
            byte[] data = Encoding.UTF8.GetBytes(s);

            // MD5CryptoServiceProviderオブジェクトを作成
            var md5 = System.Security.Cryptography.MD5.Create();

            // ハッシュ値を計算する
            byte[] bs = md5.ComputeHash(data);

            // リソースを解放する
            md5.Clear();

            // byte型配列を16進数の文字列に変換
            StringBuilder result = new();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }
    }
}
