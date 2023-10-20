using System.ComponentModel.DataAnnotations;

namespace Talabat.AdminDashboard.ViewModels
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage = "Name is required! ")]
        [StringLength(55)]
        public string Name { get; set; }
    }
}
