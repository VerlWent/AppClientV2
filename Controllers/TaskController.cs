using ApplicationClientMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClientMVC.Controllers
{
	public class TaskController : Controller
	{
        static HttpClient client = new HttpClient();

        //public string ApiUrl = "http://192.168.38.140/Api";
        public string ApiUrl = "http://localhost:441/Api";

        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        [Authorize]
		public async Task<IActionResult> Index()
		{
            var user = HttpContext.User;

            var userNameClaim = user.FindFirst(ClaimsIdentity.DefaultNameClaimType);
            var userIdClaim = user.FindFirst("UserId");

            _logger.LogInformation("Cookie:" + userNameClaim.Value);
            _logger.LogInformation("Cookie Id:" + userIdClaim.Value);


            ApiUrl += $"/Task/{userNameClaim.Value}";

            HttpResponseMessage response = await client.GetAsync(ApiUrl);

            _logger.LogInformation("Статус Task Get:" + response.StatusCode);

            string GetContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Содержимое Task Get:" + GetContent);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return View(null);
            }

            //var TaskList = JsonConvert.DeserializeObject<TaskModel>(GetContent);

            //List<TaskModel> taskList = new List<TaskModel> { TaskList };

            var TaskList = JsonConvert.DeserializeObject<List<TaskModel>>(GetContent);

            return View(TaskList);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            ApiUrl += $"/TaskGet/{id}";

            HttpResponseMessage response = await client.GetAsync(ApiUrl);

            string GetContent = await response.Content.ReadAsStringAsync();
            var TaskList = JsonConvert.DeserializeObject<TaskModel>(GetContent);

            return View(TaskList);
        }

        [Authorize]
        public async Task<IActionResult> EditConfurm(TaskModel model)
        {
            var user = HttpContext.User;
            var userIdClaim = user.FindFirst("UserId");

            ApiUrl += $"/UpdateTask"; 

            model.UserId = Convert.ToInt32(userIdClaim.Value);

            model.User = new UserModel
            {
                Id = 0,
                Username = "",
                PasswordHash = ""
            };

            _logger.LogInformation("Содержимое Task Edit:" + model.UserId);

            var jsonTask = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(jsonTask, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PutAsync(ApiUrl, httpContent))
                {
                    _logger.LogInformation("Статус Task update:" + response.StatusCode);
                    _logger.LogInformation("Содержимое Task update:" + response.Content);

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        _logger.LogInformation("Код статуса Task Edit:" + response.StatusCode);

                        TempData["DanderMessage"] = "Заполните все поля";
                        return Redirect($"Edit/{model.Id}");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Create(TaskModel model)
        {
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> CreateConfurm(TaskModel model)
        {
            var user = HttpContext.User;
            var userIdClaim = user.FindFirst("UserId");

			ApiUrl += $"/CreateTask";

			model.UserId = Convert.ToInt32(userIdClaim.Value);

            UserModel UserSet = new UserModel
            {
                Id = 0,
                Username = "",
                PasswordHash = ""
            };

            model.User = UserSet;

            var jsonTask = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(jsonTask, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(ApiUrl, httpContent))
                {
                    _logger.LogInformation("Статус Task Create IdUser:" + model.UserId);
                    _logger.LogInformation("Статус Task Create:" + response.StatusCode);
                    _logger.LogInformation("Содержимое Task Create:" + response.Content);

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        _logger.LogInformation("Код статуса Task Create:" + response.StatusCode);

                        TempData["DanderMessage"] = "Заполните все поля";
                        return Redirect($"Create/{model}");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ApiUrl += $"/Delete/{id}";

            HttpResponseMessage response = await client.DeleteAsync(ApiUrl);

            _logger.LogInformation("Статус Task Delete confurm:" + response.StatusCode);

            return RedirectToAction(nameof(Index));
        }
    }
}
