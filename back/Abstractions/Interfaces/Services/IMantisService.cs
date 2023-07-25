using MantisDevopsBridge.Api.Abstractions.Models.Transports;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Services;

public interface IMantisService
{

	Task<List<Todo>> GetAll();
	
}