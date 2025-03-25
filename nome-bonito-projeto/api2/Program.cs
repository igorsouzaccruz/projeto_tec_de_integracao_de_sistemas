using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NomeDbContext>(opt => opt.UseInMemoryDatabase("NomesDB"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api2/nomes", async (Nome nome, NomeDbContext db) => {
    db.Nomes.Add(nome);
    await db.SaveChangesAsync();
    return Results.Created($"/api2/nomes/{nome.Id}", nome);
});

app.MapGet("/api2/nomes", async (NomeDbContext db) => await db.Nomes.ToListAsync());

app.Run();

record Nome(int Id, string NomeValor);

class NomeDbContext : DbContext {
    public NomeDbContext(DbContextOptions<NomeDbContext> options) : base(options) { }
    public DbSet<Nome> Nomes => Set<Nome>();
}
