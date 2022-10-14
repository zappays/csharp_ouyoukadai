using Microsoft.EntityFrameworkCore;
using OuyouKadai.Data;

namespace OuyouKadai.Models.SeedData
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new OuyouKadaiContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OuyouKadaiContext>>());

            /* ************************************************************************ */
            /* ユーザ1名、タスク0個 の最低限の SeedData です                            */
            /* 使用する際は Program.cs の 初期データ投入の値を SeedData  に変更する     */
            /* ************************************************************************ */

            // authテーブルの初期データを自動投入（Runしたタイミング）
            if (!context.Auth.Any())
            {
                // 管理者と一般ユーザーの作成
                context.Auth.AddRange(
                    new Auth
                    {
                        Id = 1,
                        Auth_name = "管理者",
                    },

                    new Auth
                    {
                        Id = 2,
                        Auth_name = "一般ユーザー",
                    }
                );
                context.SaveChanges();
            }

            // userテーブルの初期データを自動投入（Runしたタイミング）
            if (!context.User.Any())
            { 
                // 管理者と一般ユーザーの作成
                context.User.AddRange(
                    new User
                    {
                        Name = $"Admin太郎",
                        Pass = "74b87337454200d4d33f80c4663dc5e5" /*"aaaa"*/,
                        RegID = 1,
                        Created_at = DateTime.Now,
                        AuthID = 1,
                    }
                );
                context.SaveChanges();
            }

            // statusテーブルの初期データを自動投入（Runしたタイミング）
            if (!context.Status.Any())
            {
                // ステータスの作成
                context.Status.AddRange(
                    new Status
                    {
                        Id = 1,
                        Status_name = "起票",
                    },

                    new Status
                    {
                        Id = 2,
                        Status_name = "調査中",
                    },

                    new Status
                    {
                        Id = 3,
                        Status_name = "修正中",
                    },

                    new Status
                    {
                        Id = 4,
                        Status_name = "完了",
                    }
                );
                context.SaveChanges();
            }

            // priorityテーブルの初期データを自動投入（Runしたタイミング）
            if (!context.Priority.Any())
            {
                // 優先度　"高"　"中"　"低"　の作成
                context.Priority.AddRange(
                    new Priority
                    {
                        Id = 1,
                        Priority_name = "低",
                    },

                    new Priority
                    {
                        Id = 2,
                        Priority_name = "中",
                    },

                    new Priority
                    {
                        Id = 3,
                        Priority_name = "高",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}