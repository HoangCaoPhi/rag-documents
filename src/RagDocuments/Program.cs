using Microsoft.AspNetCore.DataProtection;
using RagDocuments;
using RagDocuments.Abstractions;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddRazorPages();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "DataProtection-Keys")))
    .SetApplicationName("RagDocuments");

builder.Services.ConfigureSettings();
builder.Services.ConfigureServices();

var app = builder.Build();
 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");   
    app.UseHsts();
}

using var scope = app.Services.CreateScope();
var importer = scope.ServiceProvider.GetService<IVectorStoreImporter>()!;
await importer.ImportFileAndCreateEmbeddings(["SampleFile.pdf"]);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
