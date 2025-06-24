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
    public class RoomTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoomTypes
        public async Task<IActionResult> Index()
        {
            // SỬA: Đổi từ TypeRooms thành RoomTypes
            return View(await _context.RoomTypes.ToListAsync());
        }

        // GET: RoomTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // SỬA: Đổi từ TypeRooms thành RoomTypes
            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // GET: RoomTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Kind")] RoomType roomType)
        {
            // Debug thông tin
            Console.WriteLine($"Received - Name: '{roomType?.Name}', Kind: '{roomType?.Kind}'");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
/*
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState Errors:");
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Field: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(roomType);
            }*/

            try
            {
                // SỬA: Đổi từ TypeRooms thành RoomTypes
                _context.RoomTypes.Add(roomType);
                await _context.SaveChangesAsync();
                Console.WriteLine("RoomType saved successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving RoomType: {ex.Message}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu.");
                return View(roomType);
            }
        }

        // GET: RoomTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // SỬA: Đổi từ TypeRooms thành RoomTypes
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }
            return View(roomType);
        }

        // POST: RoomTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Kind")] RoomType roomType)
        {
            if (id != roomType.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomTypeExists(roomType.Id))
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
            // Thêm debug để xem lỗi:
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"❌ Field {state.Key} lỗi: {error.ErrorMessage}");
                }
            }
            return View(roomType);
        }

        // GET: RoomTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // SỬA: Đổi từ TypeRooms thành RoomTypes
            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // POST: RoomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // SỬA: Đổi từ TypeRooms thành RoomTypes
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType != null)
            {           
                _context.RoomTypes.Remove(roomType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomTypeExists(int id)
        {
            // SỬA: Đổi từ TypeRooms thành RoomTypes
            return _context.RoomTypes.Any(e => e.Id == id);
        }
    }
}