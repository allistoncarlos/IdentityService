using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [EmailAddress]
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Email { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string LastName { get; set; }
    }
}