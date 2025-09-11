
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using CelularesMarket.Data;
	using CelularesMarket.Models;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;



namespace CelularesMarket.Controllers
{
	public class CelularesController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _env;

		public CelularesController(ApplicationDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		// GET: Celulares/Catalogo
		public async Task<IActionResult> Catalogo()
		{
			var celulares = await _context.Celulares.Where(c => c.EnVenta).ToListAsync();
			var modelo = new CatalogoViewModel { Celulares = celulares };
			return View(modelo);
		}

		// GET: Celulares/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();
			var celular = await _context.Celulares.FirstOrDefaultAsync(m => m.Id == id);
			if (celular == null) return NotFound();
			return View(celular);
		}

		// GET: Celulares/Create
		public IActionResult Create()
		{
			return View();
		}

		   // GET: Celulares
		   public async Task<IActionResult> Index()
		   {
			   var celulares = await _context.Celulares.Where(c => c.EnVenta).ToListAsync();
			   return View(celulares);
		   }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Celular celular, IFormFile ImagenFile)
		{
			if (ModelState.IsValid)
			{
				if (ImagenFile != null && ImagenFile.Length > 0)
				{
					var uploads = Path.Combine(_env.WebRootPath, "uploads");
					if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
					var fileName = Path.GetFileName(ImagenFile.FileName);
					var filePath = Path.Combine(uploads, fileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await ImagenFile.CopyToAsync(stream);
					}
					celular.ImagenUrl = "/uploads/" + fileName;
				}
				_context.Add(celular);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Catalogo));
			}
			TempData["Error"] = "No se pudo guardar el celular. Verifica los datos ingresados.";
			return View(celular);
		}

		// GET: Celulares/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();
			var celular = await _context.Celulares.FindAsync(id);
			if (celular == null) return NotFound();
			return View(celular);
		}

		// POST: Celulares/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Celular celular, IFormFile ImagenFile)
		{
			if (id != celular.Id) return NotFound();
			ModelState.Remove("ImagenFile");
			if (ModelState.IsValid)
			{
				var existingCelular = await _context.Celulares.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
				if (existingCelular == null) return NotFound();
				if (ImagenFile != null && ImagenFile.Length > 0)
				{
					var uploads = Path.Combine(_env.WebRootPath, "uploads");
					if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
					var fileName = Path.GetFileName(ImagenFile.FileName);
					var filePath = Path.Combine(uploads, fileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await ImagenFile.CopyToAsync(stream);
					}
					celular.ImagenUrl = "/uploads/" + fileName;
				}
				else
				{
					celular.ImagenUrl = existingCelular.ImagenUrl;
				}
				_context.Update(celular);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Catalogo));
			}
			TempData["Error"] = "No se pudo editar el celular. Verifica los datos ingresados.";
			return View(celular);
		}

		// GET: Celulares/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();
			var celular = await _context.Celulares.FirstOrDefaultAsync(m => m.Id == id);
			if (celular == null) return NotFound();
			return View(celular);
		}

		// POST: Celulares/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var celular = await _context.Celulares.FindAsync(id);
			if (celular != null)
			{
				if (!string.IsNullOrEmpty(celular.ImagenUrl))
				{
					var filePath = Path.Combine(_env.WebRootPath, celular.ImagenUrl.TrimStart('/'));
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
				}
				_context.Celulares.Remove(celular);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Catalogo));
		}

		// GET: Celulares/Venta/{id?}
		[HttpGet]
		public async Task<IActionResult> Venta(int? id)
		{
			var celularesDisponibles = await _context.Celulares
														.Where(c => c.EnVenta)
														.ToListAsync();
			
			var selectList = celularesDisponibles.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {
				Value = c.Id.ToString(),
				Text = $"{c.Marca} {c.Modelo} (IMEI: {c.IMEI})"
			}).ToList();

			ViewBag.CelularesSelectList = selectList;

			var venta = new Venta
			{
				FechaVenta = DateTime.Now
			};

			if (id.HasValue)
			{
				var celularSeleccionado = celularesDisponibles.FirstOrDefault(c => c.Id == id.Value);
				if (celularSeleccionado != null)
				{
					venta.CelularId = celularSeleccionado.Id;
					venta.PrecioVenta = celularSeleccionado.Precio;
				}
				else
				{
					TempData["Error"] = "El celular seleccionado no está disponible para la venta.";
				}
			}

			return View(venta);
		}

		// POST: Celulares/RegistrarVenta
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegistrarVenta(Venta venta)
		{
			ModelState.Remove("Celular"); // El objeto Celular no se envía en el formulario, así que no debe validarse.

			if (ModelState.IsValid)
			{
				var celular = await _context.Celulares.FirstOrDefaultAsync(c => c.Id == venta.CelularId && c.EnVenta);

				if (celular == null)
				{
					TempData["Error"] = "El celular seleccionado no es válido o ya no está disponible para la venta.";
				}
				else
				{
					// Usar el precio de la base de datos para seguridad
					venta.PrecioVenta = celular.Precio;
					venta.FechaVenta = DateTime.Now;

					// Marcar el celular como no disponible
					celular.EnVenta = false;
					_context.Update(celular);

					_context.Ventas.Add(venta);
					await _context.SaveChangesAsync();

					TempData["Success"] = "¡Venta registrada exitosamente!";
					return RedirectToAction(nameof(Catalogo));
				}
			}
			else
			{
				TempData["Error"] = "No se pudo registrar la venta. Verifica los datos ingresados.";
			}

			// Si hay un error o el modelo no es válido, recargar la lista de celulares y mostrar la vista de nuevo
			var celularesDisponibles = await _context.Celulares.ToListAsync(); // PRUEBA: Se quitó el filtro EnVenta para depurar
			
			var selectList = celularesDisponibles.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {
				Value = c.Id.ToString(),
				Text = $"{c.Marca} {c.Modelo} (IMEI: {c.IMEI})"
			}).ToList();

			ViewBag.CelularesSelectList = selectList;
			
			// Es importante devolver la vista "Venta" explícitamente
			return View("Venta", venta);
		}

		[HttpGet]
        public async Task<IActionResult> GetPrecio(int id)
        {
            var celular = await _context.Celulares.FindAsync(id);
            if (celular == null)
            {
                return NotFound();
            }
            return Json(new { precio = celular.Precio });
        }

        // GET: Celulares/Balance
        public async Task<IActionResult> Balance()
        {
            var viewModel = new BalanceViewModel
            {
                Celulares = await _context.Celulares.ToListAsync(),
                Ventas = await _context.Ventas.Include(v => v.Celular).ToListAsync()
            };
            return View(viewModel);
        }
	}
}




