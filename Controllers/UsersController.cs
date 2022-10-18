using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OuyouKadai.Data;
using OuyouKadai.Models;
using X.PagedList;

namespace OuyouKadai.Controllers
{
    // ログインしていないユーザーがアクセスすると強制的にログイン画面へリダイレクトさせるアノテーション
    [Authorize]
    public class UsersController : Controller
    {
        private readonly OuyouKadaiContext _context;

        public UsersController(OuyouKadaiContext context)
        {
            _context = context;
        }

        // ユーザー一覧画面へのリンクを押下された際に実行されるアクションメソッド
        // GET: Users
        public async Task<IActionResult> Index(int? page)
        {
            // ******************************************************** //
            // ページング（NuGetパッケージ（X.PagedList）を使用）
            // https://github.com/dncuug/X.PagedList/blob/master/examples/X.PagedList.Mvc.Example.Core/Views/Bootstrap41/Index.cshtml
            // ******************************************************** //

            // Null合体演算子（pageを評価してnullでなければpageの値を、nullなら1を代入）
            var pageNumber = page ?? 1;
            IQueryable<User> query = _context.User.Include(a => a.Auth).OrderBy(u => u.Id).AsNoTracking();
            return View(await query.ToPagedListAsync(pageNumber, 20));
        }

        // ユーザー登録画面へのリンクを押下された際に実行されるアクションメソッド
        // GET: Users/Create
        public IActionResult Create()
        {
            // ******************************************************************************* //
            // セレクトリスト用　Authのテーブルから全件取得してViewBagに格納
            // 設定される値はValue = だが見た目にはText =   の値が反映される
            // ******************************************************************************* //
            ViewBag.SelectOptions = _context.Auth.ToArray()
               .Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Auth_name });

            return View();
        }

        // ユーザー登録画面で保存ボタンを押下した際に実行されるアクションメソッド
        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Pass,AuthID")] User user)
        {
            // モデルチェックを無効化　Idはログイン時に入力しないので無視する
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                // RegID（登録者）に現在のログインユーザーのIDを代入
                user.RegID = int.Parse(User.Claims.LastOrDefault().Value);
                user.Created_at = DateTime.Now;

                // パスワードをハッシュ化するメソッドを呼び出す
                User hash = new();
                user.Pass = hash.PasswordHash(user);

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // ユーザー編集画面へのリンクを押下された際に実行されるアクションメソッド
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            // 編集されるユーザーを取得
            var user = await _context.User.FindAsync(id);

            // 編集されるユーザーの登録者を取得
            var reg_user = _context.User.Where(m => m.Id == user.RegID).FirstOrDefault();

            //　編集されるユーザーの登録者が退会済みかどうか
            if (reg_user != null)
            {
                // 登録者のIDからユーザー名を取得して、Reg_nameに格納
                user.Reg_name = reg_user.Name;
            }
            else
            {
                user.Reg_name = "退会済みユーザー";
            }

            // 編集されるユーザーの更新者を取得
            var updated_user = _context.User.Where(m => m.Id == user.Updated_byID).FirstOrDefault();

            //　編集されるユーザーの更新者が退会済みかどうか
            if (updated_user != null)
            {
                // 更新者のIDからユーザー名を取得して、Updated_by_nameに格納
                user.Updated_by_name = updated_user.Name;
            }

            ViewBag.SelectOptions = _context.Auth.ToArray()
                .Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Auth_name });

            return View(user);
        }

        // ユーザー編集画面で保存ボタンを押下した際に実行されるアクションメソッド
        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Pass,AuthID,RegID,Created_at")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Updated_byID（更新者）にログインユーザーのIDを代入
                    user.Updated_byID = int.Parse(User.Claims.LastOrDefault().Value);
                    user.Updated_at = DateTime.Now;

                    // パスワードをハッシュ化するメソッドを呼び出す                   
                    User hash = new();
                    user.Pass = hash.PasswordHash(user);

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists((int)user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // ユーザー削除画面へのリンクを押下された際に実行されるアクションメソッド
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            //　GETで取得したidと同じidを持つユーザを取得し、削除画面で表示する情報を代入
            var user = await _context.User.Include(a => a.Auth)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // ユーザー削除画面で削除ボタンを押下した際に実行されるアクションメソッド
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'OuyouKadaiContext.User'  is null.");
            }

            var user = await _context.User.FindAsync(id);

            // ログイン中のユーザーは自身を削除できないようにする
            int loginUserId = int.Parse(User.Claims.LastOrDefault().Value);
            if (id == loginUserId)
            {
                ViewBag.notDeleteMessage = "ログイン中のユーザーは削除できません";
                return View(user);
            }

            // 削除しようとしているユーザーがタスクの担当者・登録者でないか確認し、そうなら削除できないようにする
            var picUser = _context.TaskItem.Where(m => m.PicID == id).FirstOrDefault();
            var regUser = _context.TaskItem.Where(m => m.RegID == id).FirstOrDefault();

            if (picUser != null)
            {
                ViewBag.notDeleteMessage = "タスクの担当者として登録されているため、削除できません";
                return View(user);
            }

            if (regUser != null)
            {
                ViewBag.notDeleteMessage = "タスクの登録者として登録されているため、削除できません";
                return View(user);
            }

            if (user != null)
            {
                _context.User.Remove(user);
            }

            var query = _context.User.Where(m => m.Updated_byID == id);
            foreach (var item in query)
            {
                item.Updated_byID = null;
            }

            var query1 = _context.TaskItem.Where(m => m.Updated_byID == id);
            foreach (var item in query1)
            {
                item.Updated_byID = null;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ユーザーの存在確認用メソッド
        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
