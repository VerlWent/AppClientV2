using System.ComponentModel.DataAnnotations;

namespace ApplicationClientMVC.ViewModels
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Не указан Email")]
		public string user { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string pass { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string pass2 { get; set; }
	}
}
