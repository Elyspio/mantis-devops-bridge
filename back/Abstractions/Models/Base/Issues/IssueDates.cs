﻿namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

public sealed class IssueDates
{
	public DateTime CreatedAt { get; init; }
	public DateTime? UpdatedAt { get; set; }
	public DateTime UpdatedAtOrCreatedAt => UpdatedAt ?? CreatedAt;
}