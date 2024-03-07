using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// In-Memory ĳ��
builder.Services.AddMemoryCache();

// �����߰�
builder.Services.AddSession(options =>
{
    // ���� ���� �ð�
    //options.IdleTimeout = TimeSpan.FromSeconds(5);
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

// �������� �ִ���ε� �뷮 ����
builder.WebHost.ConfigureKestrel(options => {
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024; // 1GB
});

builder.Services.Configure<FormOptions>(options => {
    options.MultipartBodyLengthLimit = 1024 * 1024 * 1024; // 1GB
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
// ���� ���
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
