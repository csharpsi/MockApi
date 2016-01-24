namespace MockApi.Web.Data
{
    public interface IDataContext
    {
        IDataSession OpenSession();
    }
}