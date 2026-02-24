using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Requests;
using ToDoList.Service;
using ToDoList.Response;
using System.Runtime;
namespace ToDoList.EndPoints;

public static class TarefasEndpoints
{
    public static void MapTarefasEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Tarefas").WithTags(nameof(Tarefa));

        group.MapGet("/", (TarefasContext context, StatusTarefa? status) =>
        {
            var tarefas = context.Tarefas.ToList();
       
            if (status is not null) 
            {
                tarefas = tarefas.Where(t => t.Status == status).ToList();
            }

            var response = ListaDeEntidadeParaListaDeResponse(tarefas);

            return Results.Ok(response);
            
        })
        .WithName("GetAllTarefas")
        .WithOpenApi();

        group.MapGet("/{id}",(int id, TarefasContext context) =>
        {
            var tarefa = context.Tarefas.FirstOrDefault(t => t.Id == id);

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
            , TarefasContext context) =>
        {
            var tarefa = context.Tarefas.FirstOrDefault(t => t.Id == id);

            if (tarefa is null)
                return Results.NotFound();

            tarefa.Titulo = request.titulo;
            tarefa.Descricao = request.descricao;
            context.SaveChanges();

            return Results.Ok();
            
        })
        .WithName("UpdateTarefas")
        .WithOpenApi();

        group.MapPut("Concluir/{id}", (int id, TarefasContext context) =>
        {
            var tarefa = context.Tarefas.FirstOrDefault(t => t.Id == id);

            tarefa.ConcluirTarefa();

            context.SaveChanges();
            return Results.NoContent();
        })
        .WithName("ConcluirTarefa")
        .WithOpenApi();

        group.MapPost("/",(CreateTarefasRequest request, TarefasContext context) =>
        {
            var tarefa = new Tarefa (request.titulo,request.Descricao);

            context.Add(tarefa);
            context.SaveChanges();

            return Results.Ok(tarefa);
        })
        .WithName("CreateTarefas")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, TarefasContext db) =>
        {
            var affected = await db.Tarefas
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
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
