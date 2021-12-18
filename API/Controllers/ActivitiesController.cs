using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers

  // * Want API Controller to be thin and dumb

{
  public class ActivitiesController : BaseApiController
  {
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities()
    {
      return await Mediator.Send(new List.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivity(Guid id)
    {
      var res =  await Mediator.Send(new Details.Query {Id = id});
      if (res is null)
      {
        throw new SqlNullValueException();
      }

      return res;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAcitivity(Activity activity)
    {
      return Ok(await Mediator.Send(new Create.Command{Activity = activity}));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAcitivity(Guid id, Activity activity)
    {
      activity.Id = id;
      return Ok(await Mediator.Send(new Edit.Command {Activity = activity}));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
      return Ok(await Mediator.Send(new Delete.Command {Id = id}));
    }
  }
}