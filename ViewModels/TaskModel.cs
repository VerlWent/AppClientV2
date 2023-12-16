using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationClientMVC.ViewModels
{
    public class TaskModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? Done { get; set; }

        public string? Priority { get; set; }
		//public string? Deadline { get; set; }
		public DateOnly? Deadline { get; set; }
        public virtual UserModel User { get; set; } = null!;
    }
}
