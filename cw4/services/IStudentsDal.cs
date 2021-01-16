using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.services
{
    public interface IStudentsDal
    {


        public IEnumerable<Student> GetStudents();

    }
}
