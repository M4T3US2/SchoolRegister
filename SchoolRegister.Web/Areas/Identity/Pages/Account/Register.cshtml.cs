using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.Model.DataModels;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Web.Areas.Identity.Pages.Account;


public class RegisterModel : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;

    public RegisterModel(UserManager<User> userManager,
                         SignInManager<User> signInManager,
                         RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = default!;

        [Required]
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = default!;

        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = default!;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            
            var user = new User
            {
                UserName = Input.Email,
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                RegistrationDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
             
                const string defaultRole = "Student";
                if (await _roleManager.RoleExistsAsync(defaultRole))
                    await _userManager.AddToRoleAsync(user, defaultRole);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}