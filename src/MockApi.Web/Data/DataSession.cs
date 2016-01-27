using Humanizer;
using LiteDB;
using MockApi.Web.Models;

namespace MockApi.Web.Data
{
    public class DataSession : IDataSession
    {
        private readonly LiteDatabase database;

        public DataSession(LiteDatabase database)
        {
            this.database = database;
        }

        public void Dispose() => database?.Dispose();

        public LiteCollection<Mock> Mocks => database?.GetCollection<Mock>(nameof(Mock).Pluralize().ToLower());
    }
}