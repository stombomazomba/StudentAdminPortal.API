using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Repositories;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using Student = StudentAdminPortal.API.DataModels.Student;
using StudentAdminPortal.API.DomainModels;

namespace StudentAdminPortal.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly AutoMapper.IMapper mapper;

        public StudentsController(IStudentRepository studentRepos, AutoMapper.IMapper mapper)
        {

            this.studentRepository = studentRepos;
            this.mapper = mapper;
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
        [Route("[controller]/{studentId:guid}")]
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
    }
}
