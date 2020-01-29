﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using EmailGroupsAppv2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmailGroupsAppv2.Services;

namespace EmailGroupsAppv2.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly IUserAccessor userAccessor;

    public WeatherForecastController(IUserAccessor userAccessor)
    {
      this.userAccessor = userAccessor;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
      var rng = new Random();
      var test = userAccessor.UserId;
      //var test2 = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      })
      .ToArray();
    }
  }
}
