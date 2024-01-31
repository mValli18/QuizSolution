using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Contexts
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizs { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>(quiz =>
            {
                quiz.HasKey(q => q.QuizId);
            });

            modelBuilder.Entity<Questions>(question =>
            {
                question.HasKey(q => q.QuestionId);
            });

            modelBuilder.Entity<QuizResult>(quizResult =>
            {
                quizResult.HasKey(q => q.QuizResultId);
            });
            
        }
    }

}