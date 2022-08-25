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

        public string GenerateID()
        {
            IQueryable<Major> majors = _context.Major;
            return (majors.Count() + 1).ToString();
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

        public string FindMajor(string major_name)
        {
            IQueryable<Major> majors = _context.Major;
            majors = majors.Where(u => u.MajorName == major_name);
            if(majors.Count() == 1)
            {
                return majors.First().MajorId;
            }
            else if (majors.Count() > 1)
            {
                return "-2"; //出现一个major_Name对应两个major_id的情况，请检查数据库
            }
            else
            {
                return "-1";//说明没有相应的major_id
            }
        }

        public string GetMajorIDFromExpertID(string expert_id)
        {
            IQueryable<HasExpert> hasExperts = _context.HasExpert;
            hasExperts = hasExperts.Where(u => u.ExpertId == expert_id);
            return hasExperts.First().MajorId;
        }

        public List<string> GetMajorID()
        {
            IQueryable<Major> majors = _context.Major;
            List<string> IDList = new List<string> { };
            foreach(var major in majors)
            {
                IDList.Add(major.MajorId);
            }
            return IDList;
        }

        public List<string> GetMajorName()
        {
            IQueryable<Major> majors = _context.Major;
            List<string> NameList = new List<string> { };
            foreach(var major in majors)
            {
                NameList.Add(major.MajorName);
            }
            return NameList;
        }

        public int AddMajor(string major_id,string major_name)
        {
            Major major = new Major
            {
                MajorId = major_id,
                MajorName = major_name
            };
            try
            {
                _context.Add(major);
                _context.SaveChanges();
                return 0;
            }
            catch
            {
                return -2;
            }
        }
    }
}
