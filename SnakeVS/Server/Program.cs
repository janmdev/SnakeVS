using MessagePack;
using Microsoft.AspNetCore.ResponseCompression;
using SnakeVS.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR()
    .AddMessagePackProtocol(options =>
    {
        options.SerializerOptions =
            MessagePackSerializerOptions.Standard;//.WithCompression(MessagePackCompression.Lz4BlockArray);
    });
builder.Services.AddResponseCompression(options => 
    options.MimeTypes = ResponseCompressionDefaults
    .MimeTypes
    .Concat(new[] { "application/octet-stream" })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseResponseCompression();

app.MapRazorPages();
app.MapControllers();
app.MapHub<GameHub>("/gamehub");
app.MapHub<RoomsHub>("/roomshub");
app.MapFallbackToFile("index.html");

app.Run();
