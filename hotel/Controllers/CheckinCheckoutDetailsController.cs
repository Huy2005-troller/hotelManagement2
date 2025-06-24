using hotel.Data;
using hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hotel.Controllers
{
   
        public class CheckinCheckoutDetailsController : Controller
        {
            private readonly ApplicationDbContext _context;

            public CheckinCheckoutDetailsController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: Nhập số lượng khách + dịch vụ
            public IActionResult Create(int checkinId)
            {
                ViewBag.CheckinId = checkinId;
                return View();
            }

            // POST: Nhận số lượng khách + dịch vụ
            [HttpPost]
            public IActionResult CreateMultiple(int CheckinId, int SoLuongKhach, int SoLuongDichVu)
            {
                ViewBag.CheckinId = CheckinId;
                ViewBag.SoLuongKhach = SoLuongKhach;
                ViewBag.SoLuongDichVu = SoLuongDichVu;

                ViewBag.DichVus = new SelectList(_context.DichVus.ToList(), "Id", "Ten");

                return View("FillDetails");
            }

            // POST: Lưu danh sách chi tiết vào DB
            [HttpPost]
            public async Task<IActionResult> SaveDetails(
                int CheckinId,
                List<string> TenKhachList,
                List<string> CCCDList,
                List<int> DichVuIdList)
            {
                // Lưu thông tin khách
                for (int i = 0; i < TenKhachList.Count; i++)
                {
                    var ct = new CheckinCheckoutDetail
                    {
                        CheckinCheckoutId = CheckinId,
                        TenKhach = TenKhachList[i],
                        CCCD = CCCDList[i]
                    };
                    _context.CheckinCheckoutDetails.Add(ct);
                }

                // Lưu thông tin dịch vụ
                for (int i = 0; i < DichVuIdList.Count; i++)
                {
                    var dv = await _context.DichVus.FindAsync(DichVuIdList[i]);
                    if (dv != null)
                    {
                        var ct = new CheckinCheckoutDetail
                        {
                            CheckinCheckoutId = CheckinId,
                            DichVuId = dv.Id,
                            DonGiaDichVu = dv.DonGia
                        };
                        _context.CheckinCheckoutDetails.Add(ct);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "CheckinCheckouts", new { id = CheckinId });
            }
        }
    }



