using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Repositories;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Transports;
using MantisDevopsBridge.Api.Core.Assemblers;
using Microsoft.Extensions.Logging;

namespace MantisDevopsBridge.Api.Core.Services;

public class MantisService(ITodoRepository todoRepository, ILogger<MantisService> logger) : TracingService(logger), IMantisService
{
	

	public async Task<List<Todo>> GetAll()
	{
		using var _ = LogService();

		return new List<Todo>();
	}

}