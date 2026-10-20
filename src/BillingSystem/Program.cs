using System.Diagnostics;
using BillingSystem.Data;

const string AppUrl = "http://127.0.0.1:5178";

var builder = WebApplication.CreateBuilder(args);

// Database lives in a "data" folder next to the .exe (portable: copy folder = copy everything)
var dataFolder = Path.Combine(AppContext.BaseDirectory, "data");
var db = new Database(dataFolder);
db.Initialize();
builder.Services.AddSingleton(db);

var app = builder.Build();

app.UseDefaultFiles();   // serves wwwroot/index.html at "/"
app.UseStaticFiles();    // serves wwwroot/*

app.MapGet("/api/health", (Database database) =>
{
    using var conn = database.Open();
    using var cmd = conn.CreateCommand();
    cmd.CommandText = """
        SELECT sqlite_version(),
               (SELECT value FROM app_meta WHERE key = 'schema_version');
        """;
    using var reader = cmd.ExecuteReader();
    reader.Read();

    return Results.Ok(new
    {
        status = "ok",
        sqliteVersion = reader.GetString(0),
        schemaVersion = reader.GetString(1),
        databasePath = database.FilePath
    });
});

// Open the user's default browser once the server is ready
app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        Process.Start(new ProcessStartInfo(AppUrl) { UseShellExecute = true });
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "Could not open browser. Visit {Url} manually.", AppUrl);
    }
});

app.Run(AppUrl);