using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using water.Models;
using water.Services;
using water.Data;

namespace water.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class WaterDeliveryController : Controller
    {
        private readonly WaterDeliveryService _waterDeliveryService;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public WaterDeliveryController(WaterDeliveryService waterDeliveryService, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this._waterDeliveryService = waterDeliveryService;
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Analyze(IFormFile receiptImage)
        {
            if (receiptImage == null || receiptImage.Length == 0)
            {
                ModelState.AddModelError("", "請選擇一張圖片");
                return View("Upload");
            }

            // 把上傳的圖片轉成 byte 陣列，準備交給 Service 判讀
            using var memoryStream = new MemoryStream();
            await receiptImage.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            try
            {
                WaterDeliveryInfo info = await _waterDeliveryService.AnalyzeImageAsync(imageBytes);

                // 判讀完的結果，先不存資料庫，傳到「確認頁面」給使用者看
                return View("Upload", info);
            }
            catch (Exception ex)
            {
                // 如果判讀失敗，顯示錯誤訊息
                ModelState.AddModelError("", "圖片判讀失敗，請稍後再試。");
                return View("Upload");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(WaterDeliveryInfo info)
        {
            if (!ModelState.IsValid)
            {
                return View("Upload", info);
            }
            // 取得目前登入的使用者
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            // 設定使用者資訊
            info.UserId = user.Id;
            info.User = user;

            // 存到資料庫
            _dbContext.WaterDeliveryInfos.Add(info);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
