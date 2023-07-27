﻿namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets;

public class TicketMessage
{
	public required int IdMessage { get; set; }
	public required string Text { get; set; }
	public required string Reporter { get; set; }
	public required DateTime CreatedAt { get; set; }
	public required bool Private { get; set; }
}