namespace StudentAdminPortal.API.DomainModels
{
    public class AddStudentRequest
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public long Mobile { get; set; }

        public Guid GenderId { get; set; }

        public string PhysicalAddress { set; get; }

        public string PostalAddress { set; get; }

      //  public string ProfileImageUrl { get; set; }
    }
}
