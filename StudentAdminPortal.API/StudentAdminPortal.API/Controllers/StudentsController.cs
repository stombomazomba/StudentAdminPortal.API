using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Repositories;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using Student = StudentAdminPortal.API.DataModels.Student;
using StudentAdminPortal.API.DomainModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace StudentAdminPortal.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly AutoMapper.IMapper mapper;
        private readonly object student;
        private readonly IImageRepository imageRepository;

      
        public StudentsController(IStudentRepository studentRepos, AutoMapper.IMapper mapper, IImageRepository imageRepository )
        {

            this.studentRepository = studentRepos;
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }   

        //Get All Students
        [HttpGet]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await studentRepository.GetStudentsAsync();
            var studentsDto = mapper.Map<List<Student>>(students);
            return Ok(studentsDto);

        }

        //Get Single student details
        [HttpGet]
        [Route("[controller]/{studentId:guid}"),ActionName("GetstudentAsync")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {

            //Fetchhing Student details from student repository
            var student = await studentRepository.GetStudentAsync(studentId);

            //Return Student
            if (student == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Student>(student));
        }


       //Update student details
        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] DomainModels.UpdateStudentRequest request)
        {
            if (await studentRepository.Exists(studentId))
            {
                //Update Details
                var updateStudent = await studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));

                if (updateStudent != null)
                {
                    return Ok(mapper.Map<DomainModels.Student>(updateStudent));
                }

            }
            return NotFound();
        }
       
        
        //Delete the student
        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]

        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await studentRepository.Exists(studentId))
            {
              var student = await studentRepository.DeleteStudent(studentId);
                return Ok(mapper.Map<Student>(student));

            }
            return NotFound();
        }


        //Adding a new student 
        [HttpPost]
        [Route ("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] [Required] AddStudentRequest request)
        {

            if (!ModelState.IsValid) // Check if the model state is valid
            {
                return BadRequest(ModelState); // Return bad request with validation errors
            }

            var student = await studentRepository.AddStudent(mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync),new { studentId = student.Id },
                mapper.Map<DomainModels.Student>(student));

        }


        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {

            var validExtensions = new List<string>
            {
                ".jpeg", ".png",".gif",".jpg",
            };

            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    //Check if student exists
                    if (await studentRepository.Exists(studentId))

                    {
                        

                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);


                        //Upload the image to local storage
                       var fileImagePath = await imageRepository.Upload(profileImage, fileName);

                        //update the profile image path in the database
                        if (await studentRepository.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");

                    }

                }
                return BadRequest("Image not supported");
            }

            return NotFound();
        }



        //public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        //{
        //    // Check if student exists
        //    if (await studentRepository.Exists(studentId))
        //    {
        //        // Check if profileImage is null or empty
        //        if (profileImage != null && profileImage.Length > 0)
        //        {
        //            return BadRequest("Profile image is required.");
        //        }

        //        // Upload the image to local storage
        //        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
        //       var fileImagePath = await imageRepository.Upload(profileImage, fileName);

        //        // Update the profile image path in the database
        //        if (await studentRepository.UpdateProfileImage(studentId, fileImagePath))
        //        {
        //            return Ok(fileImagePath);
        //        }

        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error on uploading Image");
        //    }

        //    return NotFound();
        //}




    }
}
