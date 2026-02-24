using System.ComponentModel.DataAnnotations;

namespace ToDoList.Requests
{
	public record CreateTarefasRequest ([Required]string titulo, [Required]string Descricao)
	{
	}
}
