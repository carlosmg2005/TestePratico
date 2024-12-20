using Microsoft.EntityFrameworkCore;
using TestePratico;
using TestePratico.Data;

// Criação do builder para o aplicativo web
var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços ao contêiner
builder.Services.AddControllersWithViews(); // Configura o uso de Controladores com Views para o modelo MVC

// Configuração da conexão com o banco de dados SQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Obtendo a string de conexão do arquivo de configuração
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // Adiciona o DbContext com a string de conexão ao serviço

var app = builder.Build();

// Configurando o pipeline de solicitação HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Habilita a exibição de erros em ambiente de desenvolvimento
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Redireciona para a página de erro personalizada
    app.UseHsts(); // Enforce HTTPS
}

app.UseHttpsRedirection(); // Redireciona todas as solicitações HTTP para HTTPS
app.UseStaticFiles(); // Serve arquivos estáticos, como CSS, JS e imagens

app.UseRouting(); // Configura o roteamento das solicitações

app.UseAuthorization(); // Habilita o middleware de autorização

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Consulta}/{action=Index}/{id?}"); // Define o roteamento padrão para o aplicativo

// Verificar se há registros na tabela 'Pessoas' antes de executar o Seed
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Verifica se há registros na tabela 'Pessoas'
    if (!context.Pessoas.Any())
    {
        PessoaSeed.SeedData(context); // Executa o Seed de dados na tabela 'Pessoas' se não houver registros
    }
}

app.Run(); // Inicia o aplicativo