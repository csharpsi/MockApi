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

        public LiteCollection<Setting> Settings => database?.GetCollection<Setting>(nameof(Setting).Pluralize().ToLower());
    }
}