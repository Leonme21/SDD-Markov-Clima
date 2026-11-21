using Markov.Web.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpClient<Markov.Web.PredictionClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MarkovApi:BaseUrl"] ?? "http://127.0.0.1:5058/");
    client.Timeout = TimeSpan.FromSeconds(10);
});
var app = builder.Build();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
