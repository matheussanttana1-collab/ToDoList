using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data;

public class TarefasContext : DbContext
{

	public TarefasContext(DbContextOptions<TarefasContext> options) : base(options)
	{
		
	}
	public DbSet<Tarefa> Tarefas { get; set; }
}
