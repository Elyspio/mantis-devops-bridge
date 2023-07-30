using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace MantisDevopsBridge.Api.Web.Controllers;

[Route("api/synchro")]
[ApiController]
public class SynchroController(IIssueService issueService, ILogger<SynchroController> logger) : TracingController(logger)
{
	[HttpPost]
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Synchronize()
	{
		using var _ = LogController();
		await issueService.Synchronize();
		return NoContent();
	}
}