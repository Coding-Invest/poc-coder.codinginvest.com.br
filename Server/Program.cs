using Server.Interfaces;
using Server.Services;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(o =>
    {
        o.ServerCertificate = new X509Certificate2("./server.pfx", "#Abc1234");
        o.AllowAnyClientCertificate();
    });
});
#endif

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFileListService, FileListService>();
builder.Services.AddScoped<IFileReadService, FileReadService>();
builder.Services.AddScoped<IFileWriteService, FileWriteService>();
builder.Services.AddScoped<IFileDeleteService, FileDeleteService>();
builder.Services.AddScoped<IFileDynamicReadService, FileDynamicReadService>();
builder.Services.AddScoped<IGitCloneService, GitCloneService>();
builder.Services.AddScoped<IGitBranchCreateService, GitBranchCreateService>();
builder.Services.AddScoped<IGitCheckoutService, GitCheckoutService>();
builder.Services.AddScoped<IGitCommitService, GitCommitService>();
builder.Services.AddScoped<IGithubAddService, GitAddService>();
builder.Services.AddScoped<IGitPullRequestService, GitPullRequestService>();
builder.Services.AddScoped<IGitPullService, GitPullService>();
builder.Services.AddScoped<IGitPushService, GitPushService>();
builder.Services.AddScoped<IMemorySaveService, MemorySaveService>();
builder.Services.AddScoped<IMemoryLoadService, MemoryLoadService>();
builder.Services.AddScoped<IMemorySummarizeService, MemorySummarizeService>();
builder.Services.AddScoped<IDotnetBuildService, DotnetBuildService>();
builder.Services.AddScoped<IEmailSendService, EmailSendService>();
builder.Services.AddScoped<IGoogleSearchService, GoogleSearchService>();
builder.Services.AddScoped<IFetchUrlService, FetchUrlService>();
builder.Services.AddScoped<INgBuildService, NgBuildService>();
builder.Services.AddScoped<INpmInstallService, NpmInstallService>();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); //TODO: remove after development

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // Must map controllers
});

app.Run();