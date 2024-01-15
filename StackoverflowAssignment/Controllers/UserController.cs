using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackoverflowAssignment.Models;
using StackoverflowAssignment.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StackoverflowAssignment.Controllers
{
    public class UserController : Controller
    {
        public readonly StackoverflowDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public UserController(StackoverflowDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
    
            dbContext = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(AddUserViewModel userDetails)
        {

            IdentityUser? UserIdentity = await userManager.FindByEmailAsync(userDetails.Email);


            var result = await signInManager.PasswordSignInAsync(UserIdentity.UserName, userDetails.Password, userDetails.RememberMe, false);


            if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            


            return View("Login");
        }
       

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(AddUserViewModel userDetails)
        {
            if (ModelState.IsValid)
            {
                if (await userManager.FindByEmailAsync(userDetails.Email) == null)
                {
                    var userIdentity = new IdentityUser
                    {
                        Email = userDetails.Email,
                        UserName = userDetails.Name,
                        PhoneNumber=userDetails.Contact
                    };

                    IdentityResult userAdded =await userManager.CreateAsync(userIdentity,userDetails.Password);

                    if(userAdded.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userIdentity, "User");
                    }
                    
                }
            

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(userDetails);
            }

        }



        [HttpGet]
        public IActionResult Profile()
        {
           
            var currentUser = this.userManager.GetUserAsync(User).Result;
            if (currentUser != null)
            {
               
               AddUserViewModel temp = new AddUserViewModel()
                {
                    Name = currentUser.UserName,
                    Email = currentUser.Email,
                    Contact = currentUser.PhoneNumber,
                };
               
                return View(temp);
            }
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(AddUserViewModel user)
        {
   
            var existingUser = await this.userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                // Update the properties you want to change
                existingUser.UserName = user.Name;
                existingUser.PhoneNumber = user.Contact;

                // Use UpdateAsync to apply the changes
                var result = await userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", user);
                }
                else
                {
                    // Handle errors in result.Errors
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
            else
            {
                // User not found, handle accordingly
            }
            return RedirectToAction("Profile", user);
        }


    }
}
