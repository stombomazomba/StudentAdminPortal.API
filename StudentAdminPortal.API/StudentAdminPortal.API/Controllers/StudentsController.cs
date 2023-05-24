using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Repositories;
using StudentAdminPortal.API.DataModels;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly AutoMapper.IMapper mapper;

        public StudentsController(IStudentRepository studentRepos , AutoMapper.IMapper mapper) 
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
    }
}
