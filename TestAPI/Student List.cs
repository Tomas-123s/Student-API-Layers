using System;
using System.Collections.Generic;
using System.Text;

namespace STudent
{
    public class Student_List
    {
        public static readonly List<Student> student_list = new List<Student>
        {
            new Student {id = 1 , name = "Tomas" , gride = 50},
            new Student {id = 2 , name = "Mostafa" , gride = 60},
            new Student {id = 3 , name = "Beshoy" , gride = 40}
        };

    }
}
