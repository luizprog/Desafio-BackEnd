using System.Reflection;
using LocacaoDesafioBackEnd.Data;
using LocacaoDesafioBackEnd.Extensions;
using LocacaoDesafioBackEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Locacao Desafio API", Version = "v1" });

    c.OperationFilter<SwaggerDescriptionFilter>(); // Use um filtro de operação personalizado para as descrições
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configurando o contexto do banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration["ConnectionStrings__DefaultConnection"]));

// Configurando os serviços
builder.Services.AddSingleton<RabbitMqService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new RabbitMqService(configuration);
});
builder.Services.AddScoped<IMessageBus, RabbitMqService>();
builder.Services.AddScoped<RabbitMqHostedService>(); // Mantenha como scoped
// Registre o serviço que usa IMessageBus
builder.Services.AddTransient<MotoService>();

var app = builder.Build();

// Aplicando migrações
app.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Locacao Desafio API V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();
