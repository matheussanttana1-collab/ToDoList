using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data;

public class TarefasContext : IdentityDbContext<Usuario>
{

	public TarefasContext(DbContextOptions<TarefasContext> opts) : base(opts)
	{
		
	}
	public DbSet<Tarefa> Tarefas { get; set; }
}
