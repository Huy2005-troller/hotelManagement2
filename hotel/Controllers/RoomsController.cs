using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hotel.Data;
using hotel.Models;
using Microsoft.Extensions.Hosting;

namespace hotel.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RoomsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        // GET: Rooms
        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Include(r => r.TypeRoom)    // Include TypeRoom để có thể hiển thị tên loại phòng
                .Include(r => r.Images)      // Include Images để có thể hiển thị ảnh
                .ToListAsync();

            return View(rooms); // ✅ Return đúng data đã include
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.TypeRoom)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(m => m.IdRoom == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewBag.TypeRoomId = new SelectList(_context.RoomTypes, "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room, List<IFormFile> ImageFiles)
        {
           
                room.Images = new List<RoomImage>();

                string wwwRootPath = _hostEnvironment.WebRootPath;

                foreach (var file in ImageFiles)
                {
                    if (file != null && file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(wwwRootPath, "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        room.Images.Add(new RoomImage
                        {
                            Path = "/images/" + fileName
                        });
                    }
                }

                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            

            return View(room);
        }

     


        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewBag.TypeRoomId = new SelectList(_context.RoomTypes, "Id", "Name", room.TypeRoomId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room room, List<IFormFile> ImageFiles)
        {
            // DEBUG: In ra tất cả thông tin nhận được
            Console.WriteLine($"=== DEBUG EDIT ROOM ===");
            Console.WriteLine($"Route ID: {id}");
            Console.WriteLine($"Room.IdRoom: {room?.IdRoom}");
            Console.WriteLine($"Room.NameRoom: '{room?.NameRoom}'");
            Console.WriteLine($"Room.TypeRoomId: {room?.TypeRoomId}");
            Console.WriteLine($"Room.Status: '{room?.Status}'");
            Console.WriteLine($"Room.Price1: {room?.Price1}");
            Console.WriteLine($"Room.Price2: {room?.Price2}");
            Console.WriteLine($"Room.MoTa: '{room?.MoTa}'");
            Console.WriteLine($"ImageFiles count: {ImageFiles?.Count ?? 0}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (id != room.IdRoom)
            {
                Console.WriteLine($"ERROR: ID mismatch - Route: {id}, Model: {room.IdRoom}");
                return NotFound();
            }

            // DEBUG: Kiểm tra ModelState errors
            if (ModelState.IsValid)// bỏ "!" ?????
            {
                Console.WriteLine("=== ModelState Errors ===");
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Field: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }

                // Tái tạo ViewBag khi có lỗi
                ViewBag.TypeRoomId = new SelectList(_context.RoomTypes, "Id", "Name", room.TypeRoomId);
                return View(room);
            }

            try
            {
                // Lấy dữ liệu gốc từ DB
                var existingRoom = await _context.Rooms
                                        .Include(r => r.Images)
                                        .FirstOrDefaultAsync(r => r.IdRoom == id);

                if (existingRoom == null)
                {
                    Console.WriteLine($"ERROR: Room with ID {id} not found in database");
                    return NotFound();
                }

                Console.WriteLine($"Found existing room: {existingRoom.NameRoom}");

                // Cập nhật các trường
                existingRoom.NameRoom = room.NameRoom;
                existingRoom.TypeRoomId = room.TypeRoomId;
                existingRoom.Status = room.Status;
                existingRoom.Price1 = room.Price1;
                existingRoom.Price2 = room.Price2;
                existingRoom.MoTa = room.MoTa;

                // Xử lý ảnh mới
                if (ImageFiles != null && ImageFiles.Count > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    Console.WriteLine($"Processing {ImageFiles.Count} image files");

                    foreach (var file in ImageFiles)
                    {
                        if (file != null && file.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string filePath = Path.Combine(wwwRootPath, "images", fileName);

                            // Tạo thư mục nếu chưa có
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            existingRoom.Images.Add(new RoomImage
                            {
                                Path = "/images/" + fileName
                            });

                            Console.WriteLine($"Added image: {fileName}");
                        }
                    }
                }

                // Lưu vào DB
                _context.Update(existingRoom);
                await _context.SaveChangesAsync();

                Console.WriteLine("Room updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật dữ liệu.");
                ViewBag.TypeRoomId = new SelectList(_context.RoomTypes, "Id", "Name", room.TypeRoomId);
                return View(room);
            }
        }


        // GET: Rooms/Delete/5
        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.TypeRoom)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(m => m.IdRoom == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.IdRoom == id);
        }
    }
}
