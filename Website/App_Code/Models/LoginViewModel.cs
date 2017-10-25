using System.Web.UI.WebControls;
using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Models
{
    /// <summary>
    /// Summary description for LoginViewModel
    /// </summary>
    [Validator(typeof(Login))]
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Username).NotNull();
        }
    }
}