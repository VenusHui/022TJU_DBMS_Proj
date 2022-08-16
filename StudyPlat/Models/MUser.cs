using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;

namespace StudyPlat.Models
{
    public class MUser : User
    {
        public ModelContext _context;
        public MUser(ModelContext context)
        {
            _context = context;
            CollectionBook = new HashSet<CollectionBook>();
            CollectionCourse = new HashSet<CollectionCourse>();
            CollectionQuestion = new HashSet<CollectionQuestion>();
            FeedbackPosting = new HashSet<FeedbackPosting>();
        }

        public string GenerateId()
        {
            IQueryable<User> users = _context.User;
            int num = 0;
            num = users.Count() + 1;
            return num.ToString();
        }

        public User findUser(string id)
        {
            IQueryable<User> users = _context.User;
            users = users.Where(u => u.UserId == id);
            User user = users.First();
            return user;
        }
    }
}
