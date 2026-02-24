using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Service;

public class TarefasService
{
	private TarefasContext _context;

	public TarefasService(TarefasContext context)
	{
		_context = context;
	}

	public void CriarTarefa(Tarefa tarefa)
	{
		_context.Add(tarefa);
		_context.SaveChanges();
	}

}
