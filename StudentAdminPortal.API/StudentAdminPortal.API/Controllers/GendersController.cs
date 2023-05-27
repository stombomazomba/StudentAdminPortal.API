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

    [ApiController]
    public class GendersController : Controller
    {

        private readonly IStudentRepository studentRepository;
        private readonly AutoMapper.IMapper mapper;

        public GendersController( IStudentRepository studentRepository, AutoMapper.IMapper mapper) 
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetallGenders()
        {
            var genderList = await studentRepository.GetGendersAsync();

            if(genderList == null || !genderList.Any())
            {
                return NotFound();
            }

            return Ok(mapper.Map<List<Gender>>(genderList));
        }
    }
}
