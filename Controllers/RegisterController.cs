using ApplicationClientMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Text;
using System.Text.Json;

namespace ApplicationClientMVC.Controllers
{
	public class RegisterController : Controller
	{
		static HttpClient client = new HttpClient();

		//public string ApiUrl = "http://192.168.38.140/Api";
		public string ApiUrl = "http://localhost:441/Api";

		private readonly ILogger<RegisterController> _logger;

		public RegisterController(ILogger<RegisterController> logger)
		{
			_logger = logger;
		}

        [AllowAnonymous]
        public IActionResult Index()
		{
			return View();
		}

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			ApiUrl += $"/Registr";

			using StringContent jsonContent = new(
				JsonSerializer.Serialize(new
				{
					Username = model.user,
					PasswordHash = model.pass
				}), 
				Encoding.UTF8,
				"application/json");

			if (model.pass != model.pass2)
			{
				TempData["DanderMessage"] = "Пароли не совпадают";
				return RedirectToAction("Index", "Register");
			}

			HttpResponseMessage response = await client.PostAsync(ApiUrl, jsonContent);

			_logger.LogInformation("Статус код register: " + response.StatusCode.ToString());

			if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
			{
				string errorMessage = await response.Content.ReadAsStringAsync();
				TempData["DanderMessage"] = errorMessage;
				return RedirectToAction("Index", "Register");
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}


		}
	}
}
