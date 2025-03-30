using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

string _bootstrapServers = "kafka:9092";

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


async Task ConsumirAsync(IServiceProvider services)
{
    var config = new ConsumerConfig
    {
        GroupId = "grupo2",
        BootstrapServers = _bootstrapServers,
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    consumer.Subscribe("topic-verificar-nome");

    var producerConfig = new ProducerConfig { BootstrapServers = _bootstrapServers };
    using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

    while (true)
    {
        try
        {
            var cr = consumer.Consume();
            var recebido = JsonSerializer.Deserialize<RecebidoDTO>(cr.Value);
            var nomeRecebido = recebido?.nome?.Trim().ToLower();

            Console.WriteLine($"🟡 Recebido: {nomeRecebido}");

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NomeDbContext>();

            var bonito = await db.Nomes.AnyAsync(n => n.NomeValor.ToLower() == nomeRecebido);

            var jsonObj = new
            {
                nome = nomeRecebido,
                ehBonito = bonito
            };

            string mensagemJson = JsonSerializer.Serialize(jsonObj);

            await producer.ProduceAsync("topic-nomes-bonitos", new Message<Null, string>
            {
                Value = mensagemJson
            });

            Console.WriteLine($"🟢 Resultado enviado: {bonito}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro: {ex.Message}");
        }
    }
}

_ = Task.Run(() => ConsumirAsync(app.Services));

// Inserir ao iniciar a API
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NomeDbContext>();
    db.Nomes.Add(new Nome(null, "Igor"));
    db.Nomes.Add(new Nome(null, "Lucas"));
    db.Nomes.Add(new Nome(null, "Ronnison"));
    db.Nomes.Add(new Nome(null, "Sung Jin Woo"));
    db.Nomes.Add(new Nome(null, "Goku"));
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

class RecebidoDTO
{
    public string nome { get; set; }
}
