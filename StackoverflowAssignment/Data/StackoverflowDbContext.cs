
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackoverflowAssignment.Models;

namespace StackoverflowAssignment.Data
{
    public class StackoverflowDbContext : IdentityDbContext
    {

        public StackoverflowDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AnswerModel> Answers { get; set; }

        public DbSet<CategoryModel> Categories { get; set; }

        public DbSet<QuestionModel> Questions { get; set; }

        public DbSet<QuestionVoteModel> QuestionVotes { get; set; }


    }
}
