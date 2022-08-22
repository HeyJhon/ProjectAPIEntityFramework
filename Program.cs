using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectef;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<TareasContext>(p=>p.UseInMemoryDatabase("TareasDB"));
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbConexion", async([FromServices] TareasContext DbContext) =>
{
    DbContext.Database.EnsureCreated();
    bool flag =  DbContext.Database.IsInMemory();
    return Results.Ok("Base de datos en memoria " + flag);
});

app.MapGet("/api/tareas", async([FromServices] TareasContext dbContext)=>{
    return Results.Ok(dbContext.Tareas.Include(p=>p.Categoria).Where(p=>p.PrioridadTarea == projectef.Models.Prioridad.Baja));
});


app.MapPost("/api/tareas", async([FromServices] TareasContext dbContext, [FromBody] projectef.Models.Tarea tarea)=>{
tarea.TareaId = Guid.NewGuid();
tarea.FechaCreacion = DateTime.Now;

await dbContext.Tareas.AddAsync(tarea);
dbContext.SaveChangesAsync();

return Results.Ok(tarea.Titulo + "Agregada correctamente");

});

app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] projectef.Models.Tarea tarea,[FromRoute] Guid id) => {

    var tareaActual = dbContext.Tareas.Find(id);

    if (tareaActual != null)
    {
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.Descripcion = tarea.Descripcion;
        await dbContext.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound();
});

app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext,[FromRoute] Guid id) => {

    var tareaActual = dbContext.Tareas.Find(id);

    if (tareaActual != null)
    {
        dbContext.Remove(tareaActual);
        await dbContext.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound();
});

app.Run();