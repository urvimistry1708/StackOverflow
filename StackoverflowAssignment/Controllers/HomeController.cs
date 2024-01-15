using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackoverflowAssignment.Data;
using StackoverflowAssignment.Models;
using System.Diagnostics;

namespace StackoverflowAssignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly StackoverflowDbContext dbContext;
        public readonly UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger, StackoverflowDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = this.userManager.GetUserAsync(User).Result;

            List<QuestionModel> allQuestions = dbContext.Questions
                .OrderByDescending(q => q.QuestionId)      
                .Take(10).ToList();

            List<AskQuestionViewModel> questionList = new List<AskQuestionViewModel>();

            //Add category object to the list
            foreach (var question in allQuestions)
            {
                //get answer Count
                var ansCount = dbContext.Answers.Where(a => a.QuestionId == question.QuestionId).Count();
                var category = dbContext.Categories.FirstOrDefault(c => c.CategoryId == question.CategoryId);

                var questionUser = await userManager.FindByIdAsync(question.Name);

                //Total Vote
                var voteCount = dbContext.QuestionVotes.Where(a => a.QuestionId == question.QuestionId).Count();
                
                if (category != null)
                {
                    

                    questionList.Add(new AskQuestionViewModel
                    {
                        QuestionId = question.QuestionId,
                        QuestionName = question.QuestionName,
                        QuestionDateAndTime = question.QuestionDateAndTime,
                        Name = questionUser.UserName,
                        Category = category,
                        ViewCount = question.ViewCount,
                        AnswerCount = ansCount,
                        VoteCount = voteCount,
                       
                    }); 
                }

            }

            // Pass the list of categories to the view
            return View(questionList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ViewCategory()
        {
            List<CategoryModel> allCategories = dbContext.Categories
                .OrderByDescending(q => q.CategoryId)
                .Take(10).ToList();


            // Pass the list of categories to the view
            return View(allCategories);
        }

        [HttpGet]
        public async Task<IActionResult> ViewQuestion()
        {
            var currentUser = this.userManager.GetUserAsync(User).Result;

            List<QuestionModel> allQuestions = dbContext.Questions
                .OrderByDescending(q => q.QuestionId).ToList();

            List<AskQuestionViewModel> questionList = new List<AskQuestionViewModel>();

            //Add category object to the list
            foreach (var question in allQuestions)
            {
                //get answer Count
                var ansCount = dbContext.Answers.Where(a => a.QuestionId == question.QuestionId).Count();
                var category = dbContext.Categories.FirstOrDefault(c => c.CategoryId == question.CategoryId);

                var questionUser = await userManager.FindByIdAsync(question.Name);


                //Total Vote
                var voteCount = dbContext.QuestionVotes.Where(a => a.QuestionId == question.QuestionId).Count();

                if (currentUser != null)
                {
                    //User vote
                    QuestionVoteModel userVote = dbContext.QuestionVotes
                        .SingleOrDefault(a => a.QuestionId == question.QuestionId && a.UserId == currentUser.Id);


                    if (category != null)
                    {
                        /* question.Category = category;
                         question.AnsCount= ansCount;*/

                        questionList.Add(new AskQuestionViewModel
                        {
                            QuestionId = question.QuestionId,
                            QuestionName = question.QuestionName,
                            QuestionDateAndTime = question.QuestionDateAndTime,
                            Category = category,
                            Name = questionUser.UserName,
                            ViewCount = question.ViewCount,
                            AnswerCount = ansCount,
                            VoteCount = voteCount,

                            UserVote = userVote != null ? true : false
                        });
                    }

                }
                else
                {

                    if (category != null)
                    {
                        

                        questionList.Add(new AskQuestionViewModel
                        {
                            QuestionId = question.QuestionId,
                            QuestionName = question.QuestionName,
                            QuestionDateAndTime = question.QuestionDateAndTime,
                            Category = category,
                            Name = questionUser.UserName,
                            ViewCount = question.ViewCount,
                            AnswerCount = ansCount,
                            VoteCount = voteCount,

                            UserVote =  false
                        });
                    }

                }
            }

            // Pass the list of categories to the view
            return View(questionList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}