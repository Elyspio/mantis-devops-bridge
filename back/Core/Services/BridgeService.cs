﻿using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing.Base;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MantisDevopsBridge.Api.Core.Services;

public class BridgeService(ILogger<BridgeService> logger, IIssueService issueService) : TracingService(logger), IHostedService, IDisposable, IAsyncDisposable
{
	private Timer? _timer;

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_timer = new Timer(Work, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_timer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}


	private void Work(object? state)
	{
		using var _ = LogService();
		issueService.Synchronize().GetAwaiter().GetResult();
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		if (_timer != null) await _timer.DisposeAsync();
	}
}