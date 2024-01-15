using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackoverflowAssignment.Data;
using StackoverflowAssignment.Models;

namespace StackoverflowAssignment.Controllers
{
    public class QuestionController : Controller
    {
        public readonly StackoverflowDbContext dbContext;
        public readonly UserManager<IdentityUser> userManager;


        public QuestionController(StackoverflowDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult AskQuestion()
        {
            var currentUser = this.userManager.GetUserAsync(User).Result;

            if (currentUser != null)
            {
                List<CategoryModel> allCategories = dbContext.Categories.ToList();

                ViewBag.AllCategories = allCategories;


                // Pass the list of categories to the view
                return View();
            }
            return RedirectToAction("Login", "User");
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult AddVotes(int ID)
        {
            var currentUser = this.userManager.GetUserAsync(User).Result;

            if (currentUser != null)
            {

                QuestionVoteModel addUserVote = dbContext.QuestionVotes
                    .FirstOrDefault(x => x.QuestionId == ID && x.UserId == currentUser.Id);

                if (addUserVote == null)
                {
                    var userVote = new QuestionVoteModel()
                    {
                        QuestionId = ID,
                        UserId = currentUser.Id,
                    };


                    dbContext.QuestionVotes.Add(userVote);
                    dbContext.SaveChanges();
                }

                return RedirectToAction("ViewQuestion", "Home");
            }

            return RedirectToAction("Login", "User");
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AskQuestion(AskQuestionViewModel question)
        {
            var currentUser = this.userManager.GetUserAsync(User).Result;

            if (currentUser != null)
            {
                if (question.CategoryId == 0 || string.IsNullOrWhiteSpace(question.QuestionName))
                {
                    // Add a model error for each validation issue
                    if (question.CategoryId == 0)
                    {
                        ModelState.AddModelError("CategoryId", "Please select a category");
                    }

                    if (string.IsNullOrWhiteSpace(question.QuestionName))
                    {
                        ModelState.AddModelError("QuestionName", "Please enter a question");
                    }

                    // Return to the view with validation errors
                    return View(question);
                }

                else
                {
                    var newQuestion = new QuestionModel()
                    {
                        QuestionName = question.QuestionName,
                        QuestionDateAndTime = DateTime.Now,
                        CategoryId = question.CategoryId,
                        Name = currentUser.Id
                    };
                    var questionAdded = dbContext.Questions.Add(newQuestion);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("ViewQuestion", "Home");
            }
            return RedirectToAction("Login", "User");
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> EditAnswer(AskQuestionViewModel answer)
        {

            var existingAnswer = dbContext.Answers.FirstOrDefault(a => a.AnswerId == answer.AnswerId && a.QuestionId == answer.QuestionId);

            {
                existingAnswer.AnswerText = answer.AnswerText;
            }

            dbContext.SaveChanges();



            if (existingAnswer != null)
            {
                return RedirectToAction("ViewQuestion", "Home");
            }



            return RedirectToAction("Login", "User");
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddAnswer(AskQuestionViewModel answer)
        {
            var currentUser = this.userManager.GetUserAsync(User).Result;

            if (currentUser != null)
            {
                if (answer != null)
                {
                    var newAnswer = new AnswerModel()
                    {
                        QuestionId = answer.QuestionId,
                        AnswerText = answer.AnswerText,
                        AnswerDateAndTime = DateTime.Now,
                        UserId = currentUser.Id,
                        Name = currentUser.UserName
                    };
                    var answerAdded = dbContext.Answers.Add(newAnswer);
                    dbContext.SaveChanges();

                    if (answerAdded != null)
                    {
                        return RedirectToAction("ViewQuestion", "Home");
                    }

                }
            }

            return RedirectToAction("Login", "User");

        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> AddAnswer(int ID)
        {
            var currentUser = this.userManager.GetUserAsync(User).Result;

            if (currentUser != null)
            {
                QuestionModel? question = dbContext.Questions.FirstOrDefault(x => x.QuestionId == ID);

                {
                    question.ViewCount = question.ViewCount + 1;
                    dbContext.SaveChanges();
                }

                //get category name
                var category = dbContext.Categories.FirstOrDefault(x => x.CategoryId == question.CategoryId);

                //get answer Count
                var ansCount = dbContext.Answers.Where(id => id.QuestionId == ID).Count();

                //get vote Count
                var voteCount = dbContext.QuestionVotes.Where(id => id.QuestionId == ID).Count();

                //get answer
                var answer = dbContext.Answers.FirstOrDefault(x => x.QuestionId == ID && x.UserId == currentUser.Id);

                //get question user
                var questionUser = await userManager.FindByIdAsync(question.Name);


                if (answer != null)
                {
                    AskQuestionViewModel temp = new AskQuestionViewModel()
                    {
                        QuestionId = ID,
                        QuestionName = question.QuestionName,
                        CategoryName = category.CategoryName,
                        Name = questionUser.UserName,
                        QuestionDateAndTime = question.QuestionDateAndTime,

                        AnswerId = answer.AnswerId,
                        AnswerText = answer.AnswerText,
                        AnswerDateAndTime = answer.AnswerDateAndTime,

                        ViewCount = question.ViewCount,
                        AnswerCount = ansCount,
                        VoteCount = voteCount,
                    };
                    return View("AddAnswer", temp);
                }
                else
                {
                    AskQuestionViewModel temp = new AskQuestionViewModel()
                    {
                        QuestionId = ID,
                        QuestionName = question.QuestionName,
                        CategoryName = category.CategoryName,
                        Name = questionUser.UserName,
                        QuestionDateAndTime = question.QuestionDateAndTime,


                        AnswerText = null,

                        ViewCount = question.ViewCount,
                        AnswerCount = ansCount,
                        VoteCount = voteCount,
                    };
                    return View("AddAnswer", temp);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }


        }
    }

}
