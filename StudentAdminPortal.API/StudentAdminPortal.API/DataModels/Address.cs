namespace StudentAdminPortal.API.DataModels
{
    public class Address
    {
        internal object Location;

        public Guid Id { get; set; }

        public string? PhysicalAddress { set; get; }

        public string ? PostalAddress { set; get; }   

        //Navigation Property

        public Guid StudentId { get; set; }


    }
}
