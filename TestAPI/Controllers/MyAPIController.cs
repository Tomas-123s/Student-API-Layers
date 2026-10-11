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
            API_Busness_Layer.CLS_Student Add_student = new API_Busness_Layer.CLS_Student(NewStudent, CLS_Student.ENmode.ADD_New);
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
            if (API_Busness_Layer.CLS_Student.DeLete_Student(ID))
            {
                return Ok($"the Student with ID {ID} deleted succefully");
            }
            else
            {
                return NotFound("The student is not found");
            }
        }
        [HttpPut("{StudentID}",Name = "Update_Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO>Update_Student(int StudentID ,StudentDTO student)
        {
            if(StudentID < 1 || student.Name == ""|| student == null)
            {
                return BadRequest("invalid data");
            }

            API_Busness_Layer.CLS_Student New_Update = API_Busness_Layer.CLS_Student.Student_By_ID(StudentID);
            if(New_Update == null)
            {
                return NotFound("the student not found");
            }
            New_Update.name = student.Name;
            New_Update.grade = student.gride;
            New_Update.save();
            return Ok("the student updated succefully");
        }

        [HttpPost("Upload_Image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult>Upload_Image(IFormFile Image_File)
        {
            if(Image_File == null || Image_File.Length == 0)
            {
                return BadRequest("Pleas upload The Image");
            }
            var Upload_diriction = "D:\\Image_Test";
            var file_Name = Guid.NewGuid().ToString() + Path.GetExtension(Image_File.FileName);
            var File_Path = Path.Combine(Upload_diriction, file_Name);

            if (!Directory.Exists(Upload_diriction))
            {
                Directory.CreateDirectory(File_Path);
            }

            using (var Final = new FileStream(File_Path, FileMode.Create))
            {
                await Image_File.CopyToAsync(Final);
            }
            return Ok(new { File_Path });
        }
        [HttpGet("Get_Image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Get_Image(string File_Name)
        {
            if(File_Name == null || File_Name.Length == 0)
            {
                return BadRequest("PLease upload the Image Name");
            }
            var Image_Diricyion = "D:\\Image_Test";
            var Image_Path = Path.Combine(Image_Diricyion, File_Name);

            if (!System.IO.File.Exists(Image_Path))
            {
                return NotFound("There isn't any Image Whith that Name");
            }

            var Image = System.IO.File.OpenRead(Image_Path);
            var Mime_Type = mimeType(Image_Path);

            return File(Image, Mime_Type);
        }

        private string mimeType(string Image_Path)
        {
            var END = Path.GetExtension(Image_Path).ToLowerInvariant();
            return END switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }
    }
}