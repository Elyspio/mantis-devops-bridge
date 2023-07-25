using MantisDevopsBridge.Api.Abstractions.Models.Base;
using MantisDevopsBridge.Api.Abstractions.Models.Entities;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Repositories;

public interface ITodoRepository : ICrudRepository<TodoEntity, TodoBase>
{
	Task<TodoEntity> Add(string label, string user);
	Task<List<TodoEntity>> GetAll(string user);
	Task<TodoEntity> Check(Guid id, string user);
	Task Delete(Guid id, string user);
}