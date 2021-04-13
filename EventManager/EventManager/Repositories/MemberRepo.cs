using EventManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Repositories
{
    public class MemberRepo
    {
        ApplicationDbContext db;

        public MemberRepo(ApplicationDbContext context)
        {
            db = context;
        }
        public Member GetOneByEmail(string email)
        {
            var member = db.Members.Where(c => c.email == email).FirstOrDefault();

            return member;
        }
        public bool checkExist(string email)
        {
            var isRegister = GetOneByEmail(email);

            if (isRegister != null)
                return true;
            return false;
        }
        public bool Create(string uName, string lName, string fName, string email)
        {
            Member member = new Member()
            {
                userName = uName,
                lastName = lName,
                firstName = fName,
                email = email
            };

            db.Members.Add(member);
            db.SaveChanges();

            return true;
        }
        public bool Update(int clientId, string lName, string fName)
        {

            Member member = db.Members.Where(m => m.Id == clientId).FirstOrDefault();
            member.lastName = lName;
            member.firstName = fName;

            db.SaveChanges();

            return true;
        }
    }
}
