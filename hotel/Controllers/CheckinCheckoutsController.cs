using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hotel.Data;
using hotel.Models;

namespace hotel.Controllers
{
    public class CheckinCheckoutsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckinCheckoutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CheckinCheckouts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CheckinCheckouts.Include(c => c.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CheckinCheckouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkinCheckout = await _context.CheckinCheckouts
                .Include(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkinCheckout == null)
            {
                return NotFound();
            }

            return View(checkinCheckout);
        }

        // GET: CheckinCheckouts/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "IdRoom", "NameRoom");
            ViewBag.KhuyenMais = _context.KhuyenMais.ToList();
            ViewBag.Rooms = _context.Rooms.Include(r => r.TypeRoom).ToList();
            return View();
        }

        // POST: CheckinCheckouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CheckinCheckout checkinCheckout, int? khuyenMaiId)
        {
            if (ModelState.IsValid)
            {
                // 🔍 1. Tìm phòng
                var room = await _context.Rooms.FindAsync(checkinCheckout.RoomId);

                if (room == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy phòng.");
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "IdRoom", "NameRoom", checkinCheckout.RoomId);
                    return View(checkinCheckout);
                }

                // ❌ 2. Nếu đang sử dụng → từ chối
                if (room.Status == "Đang sử dụng")
                {
                    ModelState.AddModelError("", "Phòng này đang được sử dụng. Vui lòng chọn phòng khác.");
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "IdRoom", "NameRoom", checkinCheckout.RoomId);
                    return View(checkinCheckout);
                }

                // ✅ 3. Tính giá: mặc định lấy Price1
                decimal giaCuoi = room.Price1;

                // 🎁 4. Nếu có mã khuyến mãi → áp dụng
                if (khuyenMaiId.HasValue)
                {
                    var promo = await _context.KhuyenMais.FindAsync(khuyenMaiId.Value);
                    if (promo != null)
                    {
                        decimal giam = room.Price1 * (decimal)promo.PhanTramGiam / 100;
                        giaCuoi = room.Price1 - giam;
                    }
                }


                // 📝 5. Gán giá và lưu phiếu
                checkinCheckout.Gia = giaCuoi;
                _context.Add(checkinCheckout);

                // 🔄 6. Cập nhật trạng thái phòng
                room.Status = "Đang sử dụng";
                _context.Update(room);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ
            ViewData["RoomId"] = new SelectList(_context.Rooms, "IdRoom", "NameRoom", checkinCheckout.RoomId);
            return View(checkinCheckout);
        }



        // GET: CheckinCheckouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkinCheckout = await _context.CheckinCheckouts.FindAsync(id);
            if (checkinCheckout == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "IdRoom", "IdRoom", checkinCheckout.RoomId);
            return View(checkinCheckout);
        }

        // POST: CheckinCheckouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NgayBatDau,NgayKetThuc,RoomId,Gia,Status")] CheckinCheckout checkinCheckout)
        {
            if (id != checkinCheckout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật phiếu checkin
                    _context.Update(checkinCheckout);

                    // 👇 Thêm đoạn này: nếu đã checkout thì cập nhật trạng thái phòng về "Trống"
                    if (checkinCheckout.Status == "Đã checkout")
                    {
                        var room = await _context.Rooms.FindAsync(checkinCheckout.RoomId);
                        if (room != null)
                        {
                            room.Status = "Trống";
                            _context.Update(room);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckinCheckoutExists(checkinCheckout.Id))
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

            ViewData["RoomId"] = new SelectList(_context.Rooms, "IdRoom", "IdRoom", checkinCheckout.RoomId);
            return View(checkinCheckout);
        }


        // GET: CheckinCheckouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkinCheckout = await _context.CheckinCheckouts
                .Include(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkinCheckout == null)
            {
                return NotFound();
            }

            return View(checkinCheckout);
        }

        // POST: CheckinCheckouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkinCheckout = await _context.CheckinCheckouts.FindAsync(id);
            if (checkinCheckout != null)
            {
                _context.CheckinCheckouts.Remove(checkinCheckout);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckinCheckoutExists(int id)
        {
            return _context.CheckinCheckouts.Any(e => e.Id == id);
        }
    }
}
