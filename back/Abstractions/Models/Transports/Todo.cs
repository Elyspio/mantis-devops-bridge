using System.ComponentModel.DataAnnotations;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Business;
using MantisDevopsBridge.Api.Abstractions.Models.Base;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports;

public class Todo : TodoBase, ITransport
{
	[Required] public required Guid Id { get; init; }
}