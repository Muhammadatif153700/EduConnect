using EduConnect.Components;
using EduConnect.Data;
using EduConnect.Models;
using EduConnect.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Repositories (Singleton to share data across all users/sessions)
builder.Services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));

// Register Core Services (Scoped - one instance per user connection/session)
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGradeService, GradeService>();

// Register State Services
builder.Services.AddScoped<AuthStateService>(); // Scoped per user session
builder.Services.AddSingleton<NotificationService>(); // Singleton to handle global broadcasts

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
