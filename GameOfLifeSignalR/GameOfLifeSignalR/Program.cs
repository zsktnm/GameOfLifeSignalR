using GameOfLifeSignalR.Components;
using GameOfLifeSignalR.Hubs;
using GameOfLifeSignalR.Services;
using GameOfLifeSignalR.Shared;

var builder = WebApplication.CreateBuilder(args);

/* ===SERVICES=== */
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddSignalR();
builder.Services.AddSingleton(
    new List<GameRoom>([
        new GameRoom(35, 35, 10),
        new GameRoom(40, 30, 5),
        new GameRoom(40, 40, 2)
    ]));
builder.Services.AddSingleton<GameRoomsService>();

var app = builder.Build();

/* ===PIPELINE=== */
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapHub<GameHub>("/gamehub");

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(GameOfLifeSignalR.Client._Imports).Assembly);

app.Run();