using Microsoft.AspNetCore.Identity;

namespace ToDoList.Models;

public class Usuario : IdentityUser
{
	public virtual ICollection<Tarefa> Tarefas {  get; set; } = new List<Tarefa>();

	public void NovaTarefa(Tarefa tarefa)
	{
		Tarefas.Add(tarefa);
	}

}
