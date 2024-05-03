using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/authentication")]
public class AuthenticationController(IServiceManager service) : ControllerBase 
{ 
    private readonly IServiceManager _service = service;
}
