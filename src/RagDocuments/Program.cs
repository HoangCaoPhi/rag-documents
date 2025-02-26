using Microsoft.AspNetCore.DataProtection;
using Microsoft.SemanticKernel;
using RagDocuments;
using RagDocuments.Abstractions.VectorStores;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddRazorPages();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "DataProtection-Keys")))
    .SetApplicationName("RagDocuments");

builder.Services.ConfigureSettings();
builder.Services.ConfigureServices();
builder.Services.ConfigureQdrant(builder.Configuration);

#pragma warning disable SKEXP0070
builder.Services.AddOllamaTextEmbeddingGeneration(
    modelId: "mxbai-embed-large",           // E.g. "mxbai-embed-large" if mxbai-embed-large was downloaded as described above.
    endpoint: new Uri("http://ollama:11434")    // Optional; for targeting specific services within Semantic Kernel
);

builder.Services.AddTransient((serviceProvider) => {
    return new Kernel(serviceProvider);
});

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
