using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Transports;
using Microsoft.AspNetCore.Mvc;

namespace MantisDevopsBridge.Api.Web.Controllers;

[Route("api/mantis")]
[ApiController]
public class MantisController(IMantisService mantisService, ILogger<MantisController> logger) : TracingController(logger)
{
	[HttpGet]
	[ProducesResponseType(typeof(List<Todo>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll()
	{
		using var _ = LogController();
		return Ok(await mantisService.GetAll());
	}
}