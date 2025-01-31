using Hawassa.Data;
using Hawassa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hawassa.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext context;

    public HomeController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index(string searchQuery)
    {
        // Fetch data from Menu table
        var menuItems = context.Menu.AsQueryable();

        // Apply search filter if searchQuery is not null or empty
        if (!string.IsNullOrEmpty(searchQuery))
        {
            menuItems = menuItems.Where(m => EF.Functions.Like(m.Name, $"%{searchQuery}%"));
        }

        // Order the data and convert it to a list
        var result = menuItems.OrderBy(m => m.Id).ToList();

        // Pass the search query to the view for display in the search box
        ViewData["SearchQuery"] = searchQuery;

        // Pass the filtered data to the view
        return View(result);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
