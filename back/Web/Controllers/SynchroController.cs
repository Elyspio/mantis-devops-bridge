using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace MantisDevopsBridge.Api.Web.Controllers;

[Authorize]
[RequiredScope("MantisDevopsBridge")]
[Route("api/synchro")]
[ApiController]
public sealed class SynchroController(ISynchroService synchroService, ITicketStateService ticketStateService, IWorkItemService workItemService, ILogger<SynchroController> logger) : TracingController(logger)
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


	[HttpDelete("work-items")]
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> DeleteWorkItems()
	{
		using var _ = LogController();
		await workItemService.DeleteAllWorkItems();
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