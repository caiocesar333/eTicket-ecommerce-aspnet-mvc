using eTicket.Data;
using eTicket.Data.Static;
using eTicket.Data.ViewModels;
using eTicket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicket.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }
        public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM) 
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password, false, false);
                    if (result.Succeeded) 
                    {
                        return RedirectToAction("Index", "Movies");
                    }
                }
                TempData["Error"] = "Wrong Credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong Credentials. Please, try again!";
            return View(loginVM);
        }

        public async Task <IActionResult> ListUsers()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public IActionResult Register()
        {
            var response = new RegisterVM();
            return View(response);
        }

        [HttpPost]

        public async Task<IActionResult> Register( RegisterVM registerVm)
        {
            if(!ModelState.IsValid) return View(registerVm);

            var user = await _userManager.FindByEmailAsync(registerVm.EmailAddress);
            if (user != null) 
            {
                TempData["Error"] = "This email Addres is already in use";
                return View(registerVm);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVm.FullName,
                Email = registerVm.EmailAddress,
                UserName = registerVm.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVm.Password);
            if (newUserResponse.Succeeded) 
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                
            }

            return View("RegisterCompleted");
        }

        [HttpPost]

        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movies");
        }


    }
}
