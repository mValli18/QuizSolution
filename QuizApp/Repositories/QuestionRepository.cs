using Microsoft.EntityFrameworkCore;
using QuizApp.Interfaces;
using QuizApp.Contexts;
using QuizApp.Models;

namespace QuizApp.Repositories
{
    public class QuestionRepository : IRepository<int, Questions>
    {
        private readonly QuizContext _context;
        public QuestionRepository(QuizContext context)
        {
            _context = context;
        }
        public Questions Add(Questions entity)
        {
            
            _context.Questions.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Questions Delete(int key)
        {
            var question = GetById(key);
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
                return question;
            }
            return null;
        }

        public IList<Questions> GetAll()
        {


            return _context.Questions.ToList();
        }

        public Questions GetById(int key)
        {
            var questions = _context.Questions.SingleOrDefault(u => u.QuestionId == key);
            return questions;
        }

        public Questions Update(Questions entity)
        {
            var question = GetById(entity.QuestionId);
            if (question != null)
            {
                _context.Entry<Questions>(question).State = EntityState.Modified;
                _context.SaveChanges();
                return question;
            }
            return null;
        }
    }
}