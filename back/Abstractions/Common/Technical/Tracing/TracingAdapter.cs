using System.Diagnostics;
using System.Runtime.CompilerServices;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing.Base;
using Microsoft.Extensions.Logging;

namespace MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;

/// <summary>
///     Tracing context for Services and Adapters
/// </summary>
public abstract class TracingAdapter : ITracingContext
{
	/// <summary>
	///     Adapter's logger
	/// </summary>
	protected readonly ILogger Logger;

	/// <summary>
	///     Class name of the class inheriting from this class
	/// </summary>
	protected readonly string sourceName;


	/// <summary>
	///     Default constructor
	/// </summary>
	/// <param name="logger"></param>
	protected TracingAdapter(ILogger logger)
	{
		Logger = logger;
		sourceName = GetType().Name;
		TracingContext.AddSource(sourceName);
	}

	private ActivitySource ActivitySource => TracingContext.GetActivitySource(sourceName);


	/// <summary>
	///     Create a logger instance for a specific call
	/// </summary>
	/// <param name="arguments"></param>
	/// <param name="method">name of the method (auto)</param>
	/// <param name="fullFilePath">filename of the method (auto)</param>
	/// <param name="autoExit">Pass false to handle <see cref="Log.LoggerInstance.Exit" /> yourself</param>
	/// <param name="className"></param>
	/// <returns></returns>
	protected Log.LoggerInstance LogAdapter(string arguments = "", [CallerMemberName] string method = "", [CallerFilePath] string fullFilePath = "", bool autoExit = true, string? className = null)
	{
		method = TracingContext.GetMethodName(method);

		className ??= Log.GetClassNameFromFilepath(fullFilePath);

		var activity = ActivitySource.CreateActivity(className, method, arguments);

		return Logger.Enter(arguments, LogLevel.Debug, activity, method, autoExit, className);
	}
}