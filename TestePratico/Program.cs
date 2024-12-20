using Microsoft.EntityFrameworkCore;
using TestePratico;
using TestePratico.Data;

// Cria��o do builder para o aplicativo web
var builder = WebApplication.CreateBuilder(args);

// Adicionando servi�os ao cont�iner
builder.Services.AddControllersWithViews(); // Configura o uso de Controladores com Views para o modelo MVC

// Configura��o da conex�o com o banco de dados SQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Obtendo a string de conex�o do arquivo de configura��o
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // Adiciona o DbContext com a string de conex�o ao servi�o

var app = builder.Build();

// Configurando o pipeline de solicita��o HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Habilita a exibi��o de erros em ambiente de desenvolvimento
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Redireciona para a p�gina de erro personalizada
    app.UseHsts(); // Enforce HTTPS
}

app.UseHttpsRedirection(); // Redireciona todas as solicita��es HTTP para HTTPS
app.UseStaticFiles(); // Serve arquivos est�ticos, como CSS, JS e imagens

app.UseRouting(); // Configura o roteamento das solicita��es

app.UseAuthorization(); // Habilita o middleware de autoriza��o

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Consulta}/{action=Index}/{id?}"); // Define o roteamento padr�o para o aplicativo

// Verificar se h� registros na tabela 'Pessoas' antes de executar o Seed
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Verifica se h� registros na tabela 'Pessoas'
    if (!context.Pessoas.Any())
    {
        PessoaSeed.SeedData(context); // Executa o Seed de dados na tabela 'Pessoas' se n�o houver registros
    }
}

app.Run(); // Inicia o aplicativo