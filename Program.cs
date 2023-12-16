using ApplicationClientMVC;
using Microsoft.AspNetCore.Authentication.Cookies;

//internal class Program
//{
//	private static void Main(string[] args)
//	{
		
//		var builder = WebApplication.CreateBuilder(args);

//		// Add services to the container.
//		builder.Services.AddControllersWithViews();

//		builder.Services.AddLogging();

//		var app = builder.Build();

//		// Configure the HTTP request pipeline.
//		if (!app.Environment.IsDevelopment())
//		{
//			app.UseExceptionHandler("/Home/Error");
//			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//			app.UseHsts();
//		}

		


//		app.UseHttpsRedirection();
//		app.UseStaticFiles();

//		app.UseRouting();

//		app.UseAuthorization();

//		app.MapControllerRoute(
//			name: "default",
//			pattern: "{controller=Login}/{action=Index}");
		
//		app.Run();
//	}
//}

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		ConfigureServices(builder.Services); // Вызываем метод ConfigureServices и передаем сервисы для конфигурации

		var app = builder.Build();
		Configure(app, app.Environment); // Вызываем метод Configure и передаем app и среду выполнения

		app.Run();
	}

	public static void ConfigureServices(IServiceCollection services)
	{
		// установка конфигурации подключения
		services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		  .AddCookie(options =>
		  {
			  options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Login/Index");
		  });
		services.AddControllersWithViews();
		services.AddLogging();
	}

	public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (!env.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
			  name: "default",
			  pattern: "{controller=Login}/{action=Index}/{id?}");
		});
	}
}