﻿using Commands.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class RegistrationController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateUseCasesCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
