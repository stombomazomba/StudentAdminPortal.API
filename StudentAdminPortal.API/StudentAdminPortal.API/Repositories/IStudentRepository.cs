using StudentAdminPortal.API.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepository
    {
        //Returning a list of Students
        Task<List<Student>> GetStudentsAsync();


        //Return a single student's detail
        Task <Student> GetStudentAsync(Guid studentId);


        Task <List<Gender>> GetGendersAsync();

        Task<bool> Exists(Guid studentId);

        Task<Student> UpdateStudent(Guid studendId, Student request);
     
    }
}
