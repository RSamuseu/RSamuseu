using System.ComponentModel.DataAnnotations;

namespace AdvertisingAgency.ViewModels
{
    public class EditUserViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }
    }
}
