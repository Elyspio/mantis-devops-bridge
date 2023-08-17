using System.Security.Cryptography;
using System.Text;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using Newtonsoft.Json;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;

public sealed class Ticket : Issue
{
	public required List<IssueMessage> Messages { get; init; }

	public required IssueDates Dates { get; init; }

    /// <summary>
    ///     Used to identify the state of a workItem
    /// </summary>
    public string Hash
	{
		get
		{
			var json = JsonConvert.SerializeObject(new
			{
				Priority,
				Messages,
				App,
				Description,
				Severity,
				Status,
				Summary,
				IdMantis,
				StepsToReproduce,
				Users
			});
			// Compute SHA-256 hash
			var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));

			// Convert byte array to a string
			StringBuilder builder = new();
			foreach (var t in bytes) builder.Append(t.ToString("x2"));

			return builder.ToString();
		}
	}
}