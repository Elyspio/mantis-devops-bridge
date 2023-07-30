using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace MantisDevopsBridge.Api.Web.Controllers;

[Route("api/mantis")]
[ApiController]
public class MantisController(IIssueService issueService, ILogger<MantisController> logger) : TracingController(logger)
{
	[HttpGet]
	[ProducesResponseType(typeof(List<Ticket>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll()
	{
		using var _ = LogController();
		return Ok(await issueService.GetAll());
	}
}