using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Data_LAyer
{
    public class StudentDTO
    {
            public StudentDTO(int Id, string Name, int Gride)
            {
                this.id = Id;
                this.Name = Name;
                this.gride = Gride;
            }

            public int id { get; set; }
            public string Name { get; set; }
            public int gride { get; set; }


        }

    public class StudentData
    {
        // Configure locally with the STUDENTS_DB_CONNECTION environment variable.
        public static string ConnectionString = Environment.GetEnvironmentVariable("STUDENTS_DB_CONNECTION")
            ?? throw new InvalidOperationException(
                "Set the STUDENTS_DB_CONNECTION environment variable before using the student data layer.");

        static public List<StudentDTO> Get_All_Student()
        {
            var student_List = new List<StudentDTO>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllStudents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            student_List.Add(new StudentDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                        }
                    }
                }
            }
            return student_List;
        }

        static public List<StudentDTO> Get_Passed_Student()
        {
            var passed = new List<StudentDTO>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPassedStudents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            passed.Add(new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                        }
                    }
                }
            }
            return passed;
        }

        static public double Get_Avrage_grade()
        {
            double avrage_grade = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    object reader = cmd.ExecuteScalar();
                    if (reader != DBNull.Value)
                    {
                        avrage_grade = Convert.ToDouble(reader);
                    }
                }
            }
            return avrage_grade;
        }

        static public StudentDTO get_by_ID(int ID)
        {
            StudentDTO student ;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetStudentById", connection))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", ID);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        static public int Add_stuudent(StudentDTO New_Student)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_AddStudent",conn))
                {

                    command.CommandType= CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", New_Student.Name);
                    command.Parameters.AddWithValue("@Grade", New_Student.gride);

                    var ID = new SqlParameter("@NewStudentId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(ID);

                    conn.Open();
                    command.ExecuteNonQuery();

                    return (int)ID.Value; 
                }
            }
        }

        static public bool Uodate_Student(StudentDTO New_Update)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", New_Update.Name);
                    command.Parameters.AddWithValue("@Grade" , New_Update.gride);
                    command.Parameters.AddWithValue("@StudentId", New_Update.id);

                    connection.Open();
                    command.ExecuteNonQuery();

                    return true;
                }
            }
        }

        static public bool Delete_Student(int ID)
        {
            int rows = 0;
            using (SqlConnection connection= new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteStudent", connection))
                {
                    command.CommandType=CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", ID);
                    connection.Open();
                    rows = (int)command.ExecuteScalar();
                    if(rows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
