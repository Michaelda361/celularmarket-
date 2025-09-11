using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CelularesMarket.Models;

namespace CelularesMarket.Controllers;

public class CompraController : Controller
{
	private readonly ILogger<CompraController> _logger;

	public CompraController(ILogger<CompraController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
