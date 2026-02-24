using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Requests;
using ToDoList.Service;
using ToDoList.Response;
using System.Runtime;
using System.Security.Claims;
namespace ToDoList.EndPoints;

public static class TarefasEndpoints
{
    public static void MapTarefasEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Tarefas").WithTags(nameof(Tarefa)).RequireAuthorization();

        group.MapGet("/", (TarefasContext db, StatusTarefa? status, HttpContext context) =>
        {
			var email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value ;

			var pessoa = db.Users.First(u => u.Email!.Equals(email));
			var tarefas = db.Tarefas.Where(t => t.usuarioID == pessoa.Id).ToList();
       
            if (status is not null) 
            {
                tarefas = tarefas.Where(t => t.Status == status).ToList();
            }
			
			var response = ListaDeEntidadeParaListaDeResponse(tarefas);

            return Results.Ok(response);
            
        })
        .WithName("GetAllTarefas")
        .WithOpenApi();

        group.MapGet("/{id}",(int id, TarefasContext db, HttpContext context) =>
        {
			var email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

			var pessoa = db.Users.First(u => u.Email!.Equals(email));
			var tarefa = db.Tarefas.Where(t => t.usuarioID == pessoa.Id).FirstOrDefault(t => t.Id == id);

            if (tarefa is null)
            {
                return Results.NotFound();
            }
            var response = EntidadeParaResponse(tarefa);
            return Results.Ok(response);
        })
        .WithName("GetTarefasById")
        .WithOpenApi();

        group.MapPut("/{id}",(int id,EditTarefaRequest request 
            , TarefasContext db, HttpContext context) =>
        {
			var email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

			var pessoa = db.Users.First(u => u.Email!.Equals(email));
			var tarefa = db.Tarefas.Where(t => t.usuarioID == pessoa.Id).FirstOrDefault(t => t.Id == id);

            if (tarefa is null)
                return Results.NotFound();

            tarefa.Titulo = request.titulo;
            tarefa.Descricao = request.descricao;
            db.SaveChanges();

            return Results.Ok();
            
        })
        .WithName("UpdateTarefas")
        .WithOpenApi();

        group.MapPut("Concluir/{id}", (int id, TarefasContext db, HttpContext context) =>
        {
			var email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

			var pessoa = db.Users.First(u => u.Email!.Equals(email));
			var tarefa = db.Tarefas.Where(t => t.usuarioID == pessoa.Id).FirstOrDefault(t => t.Id == id);
            if (tarefa is null) 
                return Results.NotFound();

			tarefa.ConcluirTarefa();

            db.SaveChanges();
            return Results.NoContent();
        })
        .WithName("ConcluirTarefa")
        .WithOpenApi();

        group.MapPost("/",(CreateTarefasRequest request, TarefasContext db, HttpContext context) =>
        {
            var email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            

            var pessoa = db.Users.First(u => u.Email!.Equals(email));
            var tarefa = new Tarefa (request.titulo,request.Descricao, pessoa.Id);

            pessoa.NovaTarefa(tarefa);
            db.SaveChanges();

            var response = EntidadeParaResponse(tarefa);
            return Results.Ok(response);
        })
        .WithName("CreateTarefas")
        .WithOpenApi();

        group.MapDelete("/{id}",(int id, TarefasContext db, HttpContext context) =>
        {
            var email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

			var pessoa = db.Users.First(u => u.Email!.Equals(email));
			var tarefa = db.Tarefas.Where(t => t.usuarioID == pessoa.Id).FirstOrDefault(t => t.Id == id);
			if (tarefa is null)
				return Results.NotFound();

            db.Remove(tarefa);
            db.SaveChanges();
            return Results.NoContent();

		})
        .WithName("DeleteTarefas")
        .WithOpenApi();
    }

    private static ICollection<TarefaResponse> ListaDeEntidadeParaListaDeResponse (List<Tarefa> tarefas)
    {
        return tarefas.Select(t =>EntidadeParaResponse(t)).ToList();
    }
    private static TarefaResponse EntidadeParaResponse (Tarefa tarefa)
    {
        return new TarefaResponse(tarefa.Id,tarefa.Titulo, tarefa.Descricao, tarefa.DataCriacao,
            tarefa.Status, tarefa.DataConclusao);
    }
}
