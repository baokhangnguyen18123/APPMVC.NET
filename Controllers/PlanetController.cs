using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers;

[Route("he-mat-troi/[action]")]
public class PlanetController(PlanetService _planetService) : Controller
{
        
    // [Route("/danh-sach-cac-hanh-tinh.html")]
    public IActionResult Index()
    {
        return View();
    }
    //Route: action
    [BindProperty(SupportsGet = true, Name = "action")]
    public string Name {get; set;}
    [HttpGet("/mercury.html")]
    public IActionResult Mercury()
    {
        var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
        return View("Detail", planet);
    }
    public IActionResult Venus()
    {
        var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
        return View("Detail", planet);
    }
    public IActionResult Earth()
    {
        var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
        return View("Detail", planet);
    }
    public IActionResult Mars()
    {
        var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
        return View("Detail", planet);
    }
    public IActionResult Jupiter()
    {
        var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
        return View("Detail", planet);
    }
    public IActionResult Saturn()
    {
        var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
        return View("Detail", planet);
    }
    public IActionResult Uranus()
    {
        var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
        return View("Detail", planet);
    }
    [Route("hanhtinh/[action]",Order = 1,Name ="neptune1")]
    [Route("[controller]-[action].html", Order = 2, Name = "neptune2")]
    public IActionResult Neptune()
    {
        var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
        return View("Detail", planet);
    }
    [Route("hanhtinh/{id:int}")]
    public IActionResult PlanetInfo(int id)
    {
        var planet = _planetService.Where(p => p.Id == id).FirstOrDefault();
        return View("Detail", planet);
    }


}
