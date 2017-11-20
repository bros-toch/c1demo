using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using FluentValidation;
using FluentValidation.Attributes;

namespace Models
{
    /// <summary>
    /// Summary description for LoginViewModel
    /// </summary>
    [Validator(typeof(Login))]
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}