using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OuyouKadai.Data;
using OuyouKadai.Models;
using System.Security.Claims;

namespace OuyouKadai.Controllers
{
    public class AccountController : Controller
    {
        private readonly OuyouKadaiContext _context;

        public AccountController(OuyouKadaiContext context)
        {
            _context = context;
        }

        // ログイン画面へのリンクを押下された際に実行されるアクションメソッド
        public IActionResult Login()
        {
            return View();
        }

        // ログイン画面でログインボタンをを押下された際に実行されるアクションメソッド
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(User user, string? returnUrl = null)
        {

            // "Name"はログイン時に使用しないため、モデルチェックを一時的に無効化
            ModelState.Remove("Name");
            if (ModelState.IsValid)
            {
                // 入力されたIDとパスワードの組み合わせのものが1件もなかった場合のエラーメッセージ
                const string badUserIdOrPasswordMessage = "IDとパスワードの組み合わせが間違っています";

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, badUserIdOrPasswordMessage);
                    return View(user);
                }

                // ユーザー名が一致するユーザーを抽出　データベースのuserテーブルから取得する
                var lookupUser = _context.User.Where(u => u.Id == user.Id).FirstOrDefault();

                // パスワードをハッシュ化するメソッドを呼び出す
                User hash = new();
                string hashpass =  hash.PasswordHash(user);

                if (lookupUser == null)
                {
                    ModelState.AddModelError(string.Empty, badUserIdOrPasswordMessage);
                    return View(user);
                }

                // パスワードの比較
                if (lookupUser.Pass != hashpass/*user.Pass*/)
                {
                    ModelState.AddModelError(string.Empty, badUserIdOrPasswordMessage);
                    return View(user);
                }

                // Cookies 認証スキームで新しい ClaimsIdentity を作成し、ユーザー名を追加する
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                // 名前を代入する
                identity.AddClaim(new Claim(ClaimTypes.Name, lookupUser.Name));
                // IDを代入する（string型）
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, lookupUser.Id.ToString()));

                // クッキー認証スキームと、上の数行で作成されたIDから作成された新しい ClaimsPrincipal を渡す
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "TaskItems");
            }
            return View(user);
        }

        // ログアウトボタンを押下された際に実行されるアクションメソッド
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuthenticationScheme");

            // ログイン画面にリダイレクト
            return RedirectToAction(nameof(Login));
        }
    }
}
