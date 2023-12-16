using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationClientMVC.ViewModels
{
	public class LoginModel
	{
		public int IdUser { get; set; }
        [Required(ErrorMessage = "Не указан Email")]
		public string user { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string pass { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<TaskModel> TaskTs { get; } = new List<TaskModel>();
    }
}

