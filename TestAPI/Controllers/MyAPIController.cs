using API_Busness_Layer;
using API_Data_LAyer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using STudent;

namespace TestAPI.Controllers
{
    [Route("api/MyAPI")]
    [ApiController]
    public class MyAPIController : ControllerBase
    {
        [HttpGet("ALL", Name = "GET_All_Studetn")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> Get_all_student()
        {
            List<StudentDTO> student_List = API_Busness_Layer.CLS_Student.Get_all_Students();
            if(student_List.Count == 0)
            {
                return NotFound("There are'nt any student");
            }
            return Ok(student_List);
        }
        [HttpGet("Passed", Name = "GET_Pssed_Student")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<StudentDTO>> Get_Passed_Student()
        {
            var Passed_Studint = API_Busness_Layer.CLS_Student.Get_Passed();
            if(Passed_Studint.Count == 0)
            {
                return NotFound("there isn't any passed student");
            }
            return Ok(Passed_Studint);
        }
        [HttpGet("AvregGrade", Name = "GET_Avreg_Grade")]

        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<double> GET_Avreg_Student()
        {
            double AvregGrade = API_Busness_Layer.CLS_Student.Avrage_Grade();
            return Ok(AvregGrade);
        }


        [HttpGet("{id}", Name = "Get_By_ID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GET_By_ID(int id)
        {
            if (id < 1)
            {
                return BadRequest("the id is not valid");
            }
            API_Busness_Layer.CLS_Student student = API_Busness_Layer.CLS_Student.Student_By_ID(id);
            if (student == null)
            {
                return NotFound("the id is wrong");
            }
            StudentDTO dTO = student.DTOstudent;
            return Ok(dTO);
        }
        [HttpPost("ADD_Student", Name = "ADD_New_Student")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> ADD_NEW_Student(StudentDTO NewStudent)
        {
            if (NewStudent == null || string.IsNullOrEmpty(NewStudent.Name))
            {
                return BadRequest("The Data IS Wrong");
            }
            API_Busness_Layer.CLS_Student Add_student = new API_Busness_Layer.CLS_Student(new StudentDTO(NewStudent.id,NewStudent.Name,NewStudent.gride), CLS_Student.ENmode.ADD_New);
            Add_student.save();
            NewStudent.id = Add_student.id;

            return CreatedAtRoute("Get_By_ID", new { id = NewStudent.id }, NewStudent);

        }
        [HttpDelete("{ID}", Name = "Delet Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> Delet_Student(int ID)
        {
            if (ID < 1)
            {
                return BadRequest("the id is wrong");
            }
            var delet = Student_List.student_list.FirstOrDefault(s => s.id == ID);
            if (delet == null)
            {
                return NotFound("The Student not found");
            }
            Student_List.student_list.Remove(delet);
            return Ok("the student deleted succsefully");
        }
        [HttpPut("{StudentID}",Name = "Update_Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student>Update_Student(int StudentID ,Student student)
        {
            if(StudentID < 1 || student.name == ""|| student == null)
            {
                return BadRequest("invalid data");
            }
            var update = Student_List.student_list.FirstOrDefault(student => student.id == StudentID);
            if(update == null)
            {
                return NotFound("the student not found");
            }
            update.name = student.name;
            update.gride = student.gride;
            return Ok("the student updated succefully");
        }

    }
}