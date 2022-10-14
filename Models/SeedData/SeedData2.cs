using Microsoft.EntityFrameworkCore;
using OuyouKadai.Data;

namespace OuyouKadai.Models.SeedData
{
    public static class SeedData2
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new OuyouKadaiContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OuyouKadaiContext>>());

            /* *********************************************************************** */
            /* ユーザ200名、タスク200個 のテストデータが入った SeedData です           */
            /* 使用する際は Program.cs の 初期データ投入の値を SeedData2 に変更する    */
            /* *********************************************************************** */

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
                for (int i = 0; i < 2000; i++)
                {
                    // 管理者と一般ユーザーの作成
                    context.User.AddRange(
                        new User
                        {
                            Name = $"{i + 1}さん",
                            Pass = "74b87337454200d4d33f80c4663dc5e5" /*"aaaa"*/,
                            RegID = i + 50,
                            Created_at = DateTime.Now,
                            AuthID = i % 2 + 1,
                        }
                    );
                }
                context.SaveChanges();
            }

            // userを1名だけ作成する場合はこちら
            /**/
            if (!context.User.Any())
            {
                    // 管理者と一般ユーザーの作成
                    context.User.AddRange(
                        new User
                        {
                            Name = $"管理人X",
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

            // taskitemテーブルの初期データを自動投入（Runしたタイミング）
            if (!context.TaskItem.Any())
            {
                for (int i = 0; i < 2000; i++)
                {
                    // タスクの作成
                    context.TaskItem.AddRange(
                        new TaskItem
                        {
                            Title = $"タイトル（task{i + 1}）",
                            Body = $"内容詳細（task{i + 1}）",
                            PicID = 20,
                            RegID = 30,
                            StatusID = i % 4 + 1,
                            PriorityID = i % 3 + 1,
                            Deadline = DateTime.Now,
                            Created_at = DateTime.Now,
                        }
                    );
                }
                context.SaveChanges();

            }      
        }
    }
}