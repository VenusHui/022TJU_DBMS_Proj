using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;

namespace StudyPlat.Models
{
    public class MMajor : Major
    {
        public readonly ModelContext _context;

        public MMajor(ModelContext context)
        {
            CourseFromMajor = new HashSet<CourseFromMajor>();
            HasExpert = new HashSet<HasExpert>();
            _context = context;
        }

        public Major GetMajor(string major_id)
        {
            IQueryable<Major> majors = _context.Major;
            majors = majors.Where(u => u.MajorId == major_id);
            int num = majors.Count();
            if(num == 1)
            {
                Major major = majors.First();
                return major;
            }
            else
            {
                return new Major
                {
                    MajorId = "-1",
                    MajorName = "-1"
                };
            }
        }
    }
}
