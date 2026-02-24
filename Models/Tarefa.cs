using System.Security.Cryptography.Xml;

namespace ToDoList.Models;

public class Tarefa
{
	public Tarefa()
	{
		
	}
	public Tarefa(string titulo, string descricao, string usuarioId)
	{
		Titulo = titulo;
		Descricao = descricao;
		usuarioID = usuarioId;
	}
	public int Id { get; set; }
	public string Titulo { get; set; }
	public string Descricao { get; set; }
	public StatusTarefa Status {  get; set; } = StatusTarefa.Pendente;
	public DateOnly DataCriacao { get; set; } = DateOnly.FromDateTime(DateTime.Now);
	public DateOnly? DataConclusao { get; set; } 
	public string usuarioID { get; set; }
	public virtual Usuario Usuario { get; set; }

	public void ConcluirTarefa ()
	{
			Status = StatusTarefa.Concluida;
			DataConclusao = DateOnly.FromDateTime(DateTime.Now);
	}

}
