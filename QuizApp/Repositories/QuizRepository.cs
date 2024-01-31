using Microsoft.EntityFrameworkCore;
using QuizApp.Contexts;
using QuizApp.Interfaces;
using QuizApp.Models;

namespace QuizApp.Repositories
{
    public class QuizRepository : IRepository<int, Quiz>
    {
        private readonly QuizContext _context;

        public QuizRepository(QuizContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Add the quiz to table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Quiz Add(Quiz entity)
        {
            _context.Quizs.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        /// <summary>
        /// Delete quiz record from the table using the quiz Id.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Quiz Delete(int key)
        {
            var quiz = GetById(key);
            if (quiz != null)
            {
                _context.Quizs.Remove(quiz);
                _context.SaveChanges();
                return quiz;
            }
            return null;
        }
        /// <summary>
        /// Get all the quizzes present in the quiz table.
        /// </summary>
        /// <returns></returns>
        public IList<Quiz> GetAll()
        {
            if (_context.Quizs.Count() == 0)
                return null;
            return _context.Quizs.ToList();
        }
        /// <summary>
        /// Get the respective quiz of given quiz id.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Quiz GetById(int key)
        {
            return _context.Quizs
        .Include(q => q.Questions) // Eager load questions
        .FirstOrDefault(q => q.QuizId == key);
        }
        /// <summary>
        /// Update the existing quiz.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Quiz Update(Quiz entity)
        {
            var quiz = GetById(entity.QuizId);
            if (quiz != null)
            {
                _context.Entry<Quiz>(quiz).State = EntityState.Modified;
                _context.SaveChanges();
                return quiz;
            }
            return null;
        }

    }
}
