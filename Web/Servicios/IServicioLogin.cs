namespace Web.Servicios
{
    public interface IServicioLogin
    {
        Task Login(string token);

        Task Logout();
    }
}
