using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// In-Memory 캐싱
builder.Services.AddMemoryCache();

// 세션추가
builder.Services.AddSession(options =>
{
    // 세션 유지 시간
    //options.IdleTimeout = TimeSpan.FromSeconds(5);
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

// 전역으로 최대업로드 용량 설정
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
// 세션 사용
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
