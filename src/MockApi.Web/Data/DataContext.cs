using LiteDB;

namespace MockApi.Web.Data
{
    public class DataContext : IDataContext
    {
        private readonly string databaseFileName;
        
        public DataContext(string databaseFileName)
        {
            this.databaseFileName = databaseFileName;
        }
        
        public IDataSession OpenSession()
        {
            return new DataSession(new LiteDatabase(databaseFileName));
        }
    }
}