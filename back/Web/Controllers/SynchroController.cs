using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MantisDevopsBridge.Api.Web.Controllers;

[Route("api/synchro")]
[ApiController]
public sealed class SynchroController(ISynchroService synchroService, ITicketStateService ticketStateService, ILogger<SynchroController> logger) : TracingController(logger)
{
	/// <inheritdoc cref="ISynchroService.Synchronize" />
	[HttpPost("tickets")]
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Synchronize()
	{
		using var _ = LogController();
		await synchroService.Synchronize();
		return NoContent();
	}


	/// <inheritdoc cref="ITicketStateService.Regenerate" />
	[HttpPatch("states")]
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> RegenerateStates()
	{
		using var _ = LogController();
		await ticketStateService.Regenerate();
		return NoContent();
	}
}