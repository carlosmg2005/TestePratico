using Microsoft.EntityFrameworkCore;
using TestePratico;
using TestePratico.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicione servi�os ao cont�iner.
builder.Services.AddControllersWithViews(); // Use Controllers with Views para MVC

// Configura��o da conex�o com o banco de dados SQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configurar o pipeline de solicita��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Consulta}/{action=Index}/{id?}");

// Verificar se h� registros na tabela 'Pessoas' antes de executar o Seed
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Verifica se h� registros na tabela 'Pessoas'
    if (!context.Pessoas.Any())
    {
        PessoaSeed.SeedData(context);
    }
}

app.Run();