using Microsoft.EntityFrameworkCore;
using projectef.Models;

namespace projectef;
public class TareasContext:DbContext{
    public DbSet<Categoria> Categorias{get;set;}
    public DbSet<Tarea> Tareas{get;set;}
    public TareasContext(DbContextOptions<TareasContext> options):base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder){

        List<Categoria> categoriasInit = new();
        categoriasInit.Add(
            new(){
            CategoriaId = Guid.Parse("09847010-e3e9-4c46-bd73-40cd72c8b1b8"),
            Nombre ="Actividades Pendientes",
            Peso = 20}
        );
         categoriasInit.Add(
            new(){
            CategoriaId = Guid.Parse("09847010-e3e9-4c46-bd73-40cd72c8b1b1"),
            Nombre ="Actividades Personales",
            Peso = 30}
        );

         categoriasInit.Add(
            new(){
            CategoriaId = Guid.Parse("09847010-e3e9-4c46-bd73-40cd72c8b1b2"),
            Nombre ="Actividades Otras",
            Peso = 40}
        );
        modelBuilder.Entity<Categoria>(categoria =>{
            categoria.ToTable("Categoria");
            categoria.HasKey(p=>p.CategoriaId);
            categoria.Property(p=>p.Nombre).IsRequired().HasMaxLength(150);
            categoria.Property(p=>p.Descripcion).IsRequired(false);
            categoria.Property(p=>p.Peso);
            categoria.HasData(categoriasInit);
        });

List<Tarea> tareasInit = new(){
    new(){
        TareaId = Guid.Parse("09847010-e3e9-4c46-bd73-40cd72c8b1ba"),
        CategoriaId = Guid.Parse("09847010-e3e9-4c46-bd73-40cd72c8b1b8"),
        PrioridadTarea = Prioridad.Media,
        Titulo="Pago de servicios",
        FechaCreacion = DateTime.Now    

    },
     new(){
        TareaId = Guid.Parse("09847010-e3e9-4c46-bd73-40cd72c8b1bb"),
        CategoriaId = Guid.Parse("09847010-e3e9-4c46-bd73-40cd72c8b1b1"),
        PrioridadTarea = Prioridad.Baja,
        Titulo="Terminar Pelicula en Netflix",
        FechaCreacion = DateTime.Now    

    }
};

        modelBuilder.Entity<Tarea>(tarea =>{
            tarea.ToTable("Tarea");
            tarea.HasKey(p=>p.TareaId);
            tarea.HasOne(p=>p.Categoria).WithMany(p => p.Tareas).HasForeignKey(p=>p.CategoriaId);
            tarea.Property(p=>p.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(p=>p.Descripcion).IsRequired(false);
            tarea.Property(p=>p.PrioridadTarea);
            tarea.Property(p=>p.FechaCreacion);
            tarea.Ignore(p=>p.Resumen);
            tarea.HasData(tareasInit);
        });
    }
}