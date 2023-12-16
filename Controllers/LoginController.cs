using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using static System.Net.WebRequestMethods;
//using ApplicationClientMVC.Models;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using ApplicationClientMVC.ViewModels;
using System.Reflection;
using Newtonsoft.Json;

namespace ApplicationClientMVC.Controllers
{
	public class LoginController : Controller
	{

		static HttpClient client = new HttpClient();

		private readonly ILogger<LoginController> _logger;

		//public string ApiUrl = "http://192.168.38.140/Api";
		public string ApiUrl = "http://localhost:441/Api";

		public LoginController(ILogger<LoginController> logger)
		{
			_logger = logger;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			_logger.LogInformation("Это запуск");

            return View();
		}

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
		public async Task<IActionResult> Login(UserModel model)
		{
			_logger.LogInformation("Логин:" + model.Username + "Пароль:" + model.PasswordHash);

			ApiUrl += $"/Auth/{model.Username}/{model.PasswordHash}";

			HttpResponseMessage response = await client.GetAsync(ApiUrl);


			if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				TempData["DanderMessage"] = "Неверные данные";
				return RedirectToAction("Index", "Login");
			}

            string GetContent = await response.Content.ReadAsStringAsync();
            var TaskList = JsonConvert.DeserializeObject<UserModel>(GetContent);

			

            _logger.LogInformation("Логин:" + TaskList.Username + "Пароль:" + TaskList.PasswordHash + "Id: " + TaskList.Id);


            await Authenticate(TaskList);

			_logger.LogInformation(response.Content.ToString());

            //return RedirectPermanent("/Task");
            return RedirectToAction("Index", "Task");

			
		}

        [AllowAnonymous]
        private async Task Authenticate(UserModel model)
		{

            // создаем один claim
            var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, model.Username),
                new Claim("UserId", model.Id.ToString()),
                new Claim("UserPassword", model.PasswordHash.ToString())
            };
			// создаем объект ClaimsIdentity
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			// установка аутентификационных куки
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Login");
		}
    }
}
