namespace StudentAdminPortal.API.Controllers
{
    internal interface IMapper
    {
        object? Map<T>(T students);
    }
}