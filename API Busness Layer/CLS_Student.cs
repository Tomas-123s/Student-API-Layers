using API_Data_LAyer;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace API_Busness_Layer
{
    public class CLS_Student
    {
        public enum ENmode { ADD_New = 0, Update = 1 }
        static public ENmode mode = ENmode.ADD_New;

        public StudentDTO DTOstudent
        {
            get { return new StudentDTO(this.id, this.name, this.grade); }
        }
        public int id {  get; set; }
        public string name { get; set; }
        public int grade {  get; set; }

        public CLS_Student(StudentDTO sDTO , ENmode eNmode)
        {
            this.id = sDTO.id;
            this.name = sDTO.Name;
            this.grade = sDTO.gride;

            mode = eNmode;
        }

        private bool _ADD_Student()
        {
            this.id = StudentData.Add_stuudent(DTOstudent);

            return (this.id != -1);
        }
        static public List<StudentDTO> Get_all_Students()
        {
            return StudentData.Get_All_Student();
        }
        static public List<StudentDTO> Get_Passed()
        {
            return StudentData.Get_Passed_Student();
        }
        static public double Avrage_Grade()
        {
            return StudentData.Get_Avrage_grade();
        }
        static public CLS_Student Student_By_ID(int id)
        {
            StudentDTO student = StudentData.get_by_ID(id);
            if(student != null)
            {
                return new CLS_Student(student, ENmode.Update);
            }
            else
            {
                return null;
            }
        }

        private bool Update_Student()
        {
            return StudentData.Uodate_Student(DTOstudent);
        }
        static public bool DeLete_Student(int ID)
        {
            return StudentData.Delete_Student(ID);
        }

         public bool save()
        {
            switch (mode)
            {
                case ENmode.ADD_New:
                    if (_ADD_Student())
                    {
                        mode = ENmode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                 case ENmode.Update:
                     return Update_Student();
                
                
                default: return false;
            }
        }

    }
}
