using ToDoList.Models;

namespace ToDoList.Response
{
	public record TarefaResponse (int id, string Titulo, string Descricao, 
		DateOnly DataCriacao, StatusTarefa Status, DateOnly? DataConclusao)
	{

	}
}
