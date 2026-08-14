using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using water.Data;
using water.Models;

namespace water.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 取得目前登入的使用者
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(); // 如果使用者未登入，導向登入頁面
            }

            // 取得該使用者的水費繳費紀錄
            var waterDeliveryRecords = await _dbContext.WaterDeliveryInfos
                .Where(w => w.UserId == user.Id)
                .OrderByDescending(w => w.DeliveryDate)
                .ToListAsync();

            var viewModels = new List<WaterDeliveryViewModel>(); // 建立一個 List 來放 WaterDeliveryViewModel

            for (int i = 0; i < waterDeliveryRecords.Count; i++)
            {
                int? daysSincePrevious = null;
                if (i < waterDeliveryRecords.Count - 1) 
                {
                    daysSincePrevious = waterDeliveryRecords[i].DeliveryDate.DayNumber - waterDeliveryRecords[i + 1].DeliveryDate.DayNumber;
                }

                viewModels.Add(new WaterDeliveryViewModel
                {
                    Record = waterDeliveryRecords[i],
                    DaysSincePrevious = daysSincePrevious
                });
            }

            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var record = await _dbContext.WaterDeliveryInfos
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == user.Id);

            if (record == null)
            {
                return NotFound();
            }

            _dbContext.WaterDeliveryInfos.Remove(record);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var record = await _dbContext.WaterDeliveryInfos
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == user.Id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WaterDeliveryInfo waterDeliveryInfo)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }
            
            if (!ModelState.IsValid)
            {
                return View(waterDeliveryInfo);
            }

            var record = await _dbContext.WaterDeliveryInfos
                .FirstOrDefaultAsync(w => w.Id == waterDeliveryInfo.Id && w.UserId == user.Id);

            if (record == null)
            {
                return NotFound();
            }

            record.Product_Id = waterDeliveryInfo.Product_Id;
            record.Product_Name = waterDeliveryInfo.Product_Name;
            record.DeliveryDate = waterDeliveryInfo.DeliveryDate;
            record.Quantity = waterDeliveryInfo.Quantity;
            record.RemainingQuantity = waterDeliveryInfo.RemainingQuantity;
            record.Sheet_Id = waterDeliveryInfo.Sheet_Id;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
