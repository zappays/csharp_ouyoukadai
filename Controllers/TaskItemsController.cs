using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OuyouKadai.Data;
using OuyouKadai.Models;
using System.Data;
using X.PagedList;

namespace OuyouKadai.Controllers
{
    // ログインしていないユーザーがアクセスすると強制的にログイン画面へリダイレクトさせるアノテーション
    [Authorize]
    public class TaskItemsController : Controller
    {
        private readonly OuyouKadaiContext _context;

        public TaskItemsController(OuyouKadaiContext context)
        {
            _context = context;
        }

        // タスク一覧画面へのリンクを押下された際に実行されるアクションメソッド
        // GET: TaskItems
        public async Task<IActionResult> Index(int? page)
        {
            // ******************************************************** //
            // ページング（NuGetパッケージ（X.PagedList）を使用）
            // https://github.com/dncuug/X.PagedList/blob/master/examples/X.PagedList.Mvc.Example.Core/Views/Bootstrap41/Index.cshtml
            // ******************************************************** //

            // Null合体演算子（pageを評価してnullでなければpageの値を、nullなら1を代入）
            var pageNumber = page ?? 1;
            // 一覧に表示するために必要な値をjoinしてqueryに格納
            IQueryable<TaskItem> query = _context.TaskItem
                .Include(s => s.Status)
                .Include(p => p.Priority)
                .Include(f => f.Pic)
                .OrderBy(t => t.Id).AsNoTracking();

            // 20件ごとにページングするための情報を渡す
            return View(await query.ToPagedListAsync(pageNumber, 20));
        }

        // タスク登録画面へのリンクを押下された際に実行されるアクションメソッド
        // GET: TaskItems/Create
        public IActionResult Create()
        {
            // ******************************************************************************* //
            // セレクトリスト用　User,Status,Proorityのテーブルから全件取得してViewBagに格納
            // 設定される値はValue = だが見た目にはText =   の値が反映される
            // ******************************************************************************* //
            ViewBag.SelectOptionsUser = _context.User.ToArray()
                .Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

            ViewBag.SelectOptionsStatus = _context.Status.ToArray()
                .Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Status_name});

            ViewBag.SelectOptionsPriority = _context.Priority.ToArray()
                .Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Priority_name });

            ViewBag.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            return View();
        }

        // タスク登録画面で登録ボタンを押下した際に実行されるアクションメソッド
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Body,PicID,StatusID,PriorityID,Deadline")] TaskItem taskItem)
        {

            if (ModelState.IsValid)
            {
                // RegID（登録者）にログインユーザーを代入
                taskItem.RegID = int.Parse(User.Claims.LastOrDefault().Value);
                taskItem.Created_at = DateTime.Now;
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // タスク編集画面へのリンクを押下された際に実行されるアクションメソッド
        // GET: TaskItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TaskItem == null)
            {
                return NotFound();
            }
            // GETで取得したidと同じidのタスクをtaskitemに代入
            var taskItem = await _context.TaskItem.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            // ******************************************************************************* //
            // セレクトリスト用　User,Status,Proorityのテーブルから全件取得してViewBagに格納
            // 設定される値はValue = だが見た目にはText =   の値が反映される
            // ******************************************************************************* //
            ViewBag.SelectOptionsUser = _context.User.ToArray()
                .Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

            ViewBag.SelectOptionsStatus = _context.Status.ToArray()
                .Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Status_name });

            ViewBag.SelectOptionsPriority = _context.Priority.ToArray()
                .Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Priority_name });

            // Updated_byID にnull以外が代入されていた場合、Updated_by_name に名前を代入
            if (taskItem.Updated_byID != null)
            {
                taskItem.Updated_by_name = _context.User.Where(m => m.Id == taskItem.Updated_byID).FirstOrDefault().Name;
            }

            return View(taskItem);
        }

        // タスク編集画面で保存ボタンを押下した際に実行されるアクションメソッド
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,PicID,StatusID,PriorityID,Deadline,RegID,Created_at,Updated_byID")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 更新者をログインユーザに設定
                    taskItem.Updated_byID = int.Parse(User.Claims.LastOrDefault().Value);
                    // 更新時刻を現在に設定
                    taskItem.Updated_at = DateTime.Now;  
                    
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
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
            return View(taskItem);
        }

        // タスク削除画面へのリンクを押下された際に実行されるアクションメソッド
        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TaskItem == null)
            {
                return NotFound();
            }

            // GETで取得したidとタスクの情報を表示するためにjoinしてtaskItemに格納
            var taskItem = await _context.TaskItem
                .Include(s => s.Status)
                .Include(p => p.Priority)
                .Include(r => r.Reg)
                .Include(f => f.Pic)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            var updated_by = _context.User.Where(m => m.Id == taskItem.Updated_byID).FirstOrDefault();
            if (updated_by != null)
            {
                ViewBag.updated_by_name = updated_by.Name;
            }
            
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // タスク削除画面で削除ボタンを押下した際に実行されるアクションメソッド
        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TaskItem == null)
            {
                return Problem("Entity set 'OuyouKadaiContext.TaskItem'  is null.");
            }
            
            var taskItem = await _context.TaskItem.FindAsync(id);

            if (taskItem != null)
            {
                _context.TaskItem.Remove(taskItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // タスクの存在確認用メソッド
        private bool TaskItemExists(int id)
        {
          return (_context.TaskItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
