using Microsoft.EntityFrameworkCore;
using QuizApp.Contexts;
using QuizApp.Interfaces;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizApp.Repositories
{
    // Implementation of IRepository for QuizResult entity
    public class QuizResultRepository : IRepository<int, QuizResult>
    {
        private readonly QuizContext _context;

        // Constructor to inject QuizContext
        public QuizResultRepository(QuizContext context)
        {
            _context = context;
        }

        // Add a new QuizResult to the database
        public QuizResult Add(QuizResult entity)
        {
            _context.QuizResults.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        // Delete a QuizResult from the database based on its key
        public QuizResult Delete(int key)
        {
            var quizResult = GetById(key);
            if (quizResult != null)
            {
                _context.QuizResults.Remove(quizResult);
                _context.SaveChanges();
                return quizResult;
            }
            return null;
        }

        // Get all QuizResults from the database
        public IList<QuizResult> GetAll()
        {
            return _context.QuizResults.ToList();
        }

        // Get a QuizResult by its key from the database
        public QuizResult GetById(int key)
        {
            return _context.QuizResults.FirstOrDefault(qr => qr.QuizResultId == key);
        }

        // Update an existing QuizResult in the database
        public QuizResult Update(QuizResult entity)
        {
            var existingQuizResult = GetById(entity.QuizResultId);
            if (existingQuizResult != null)
            {
                _context.Entry(existingQuizResult).CurrentValues.SetValues(entity);
                _context.SaveChanges();
                return existingQuizResult;
            }
            return null;
        }

        // Get all QuizResults for a specific user from the database
        public IList<QuizResult> GetResultsByUser(string username)
        {
            return _context.QuizResults
                .Where(qr => qr.Username.Equals(username))
                .ToList();
        }

        // Get all QuizResults for a specific quiz from the database
        public IList<QuizResult> GetResultsByQuiz(int quizId)
        {
            return _context.QuizResults
                .Where(qr => qr.QuizId == quizId)
                .ToList();
        }
    }
}
