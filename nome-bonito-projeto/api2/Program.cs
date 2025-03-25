using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Definir porta como 8083
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8083);
});

// Configuração do banco em memória
builder.Services.AddDbContext<NomeDbContext>(opt => opt.UseInMemoryDatabase("NomesDB"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Inserir "IGOR" ao iniciar a API
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NomeDbContext>();
    db.Nomes.Add(new Nome(null, "Igor"));
    db.SaveChanges();
}

// Endpoints
app.MapPost("/api2/nomes", async (Nome nome, NomeDbContext db) =>
{
    db.Nomes.Add(nome);
    await db.SaveChangesAsync();
    return Results.Created($"/api2/nomes/{nome.Id}", nome);
});

app.MapGet("/api2/nomes", async (NomeDbContext db) => await db.Nomes.ToListAsync());

app.Run();

record Nome(int? Id, string NomeValor);

class NomeDbContext : DbContext
{
    public NomeDbContext(DbContextOptions<NomeDbContext> options) : base(options) { }

    public DbSet<Nome> Nomes => Set<Nome>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Nome>()
            .HasKey(n => n.Id);

        modelBuilder.Entity<Nome>()
            .Property(n => n.Id)
            .ValueGeneratedOnAdd();
    }
}
