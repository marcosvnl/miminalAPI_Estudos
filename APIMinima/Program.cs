using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Clientes"));

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseSwagger();

app.MapGet("/Clientes", async (AppDbContext db) => await db.Clients.ToListAsync());
app.MapGet("/Cliente/{id}", async (int id, AppDbContext db) => await db.Clients.FirstOrDefaultAsync(c => c.Id == id));
app.MapPost("/Clientes", async (Client client, AppDbContext db) =>
{
    db.Clients.Add(client);
    await db.SaveChangesAsync();
    return client;
});

app.MapPut("/Cliente/{id}", async (int id, Client client, AppDbContext db) =>
{
    db.Entry(client).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return client;
});

app.MapDelete("/Cliente/{id}", async (int id, AppDbContext db) =>
{
    Client? client = await db.Clients.FirstOrDefaultAsync(c => c.Id == id);
    if (client != null)
    {
        db.Clients.Remove(client!);
        await db.SaveChangesAsync();
    }
    return;
});

app.UseSwaggerUI();

app.Run();

public class Client
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Client> Clients { get; set; }
}
