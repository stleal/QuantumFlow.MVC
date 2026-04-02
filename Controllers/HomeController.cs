using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuantumFlowMVC.Models;

namespace QuantumFlowMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public async Task<IActionResult> Login(string username, string password)
    {
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            var client = new HttpClient();
            var apiUrl = _configuration["QuantumFlow.API:BaseUrl"] + _configuration["QuantumFlow.API:LoginEndpoint"];
            var resp = await client.PostAsJsonAsync(apiUrl, new { Username = username, Password = password });
            return resp.IsSuccessStatusCode ? RedirectToAction("Index", "Dashboard") : RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
