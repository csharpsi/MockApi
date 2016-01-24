using System.Collections.Generic;
using System.Linq;
using System.Net;
using LiteDB;
using MockApi.Web.Data;
using MockApi.Web.Models;

namespace MockApi.Web.Repository
{
    public class SettingRepository : ISettingRepository
    {
        private readonly IDataContext dataContext;

        public SettingRepository(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public List<Setting> GetAll()
        {
            using (var db = dataContext.OpenSession())
            {
                return db.Settings
                    .FindAll()
                    .OrderBy(x => x.Route)
                    .ThenBy(x => x.StatusCode)
                    .ToList();
            }
        }

        public Setting FindById(ObjectId id)
        {
            using (var db = dataContext.OpenSession())
            {
                return db.Settings.FindById(id);
            }
        }

        public Setting FindRoute(string route, HttpStatusCode statusCode, HttpMethodType httpMethod)
        {
            using (var db = dataContext.OpenSession())
            {
                var routeMatch = Query.EQ(nameof(Setting.Route), route);
                var statusCodeMatch = Query.EQ(nameof(Setting.StatusCode), statusCode.ToString());
                var methodMatch = Query.EQ(nameof(Setting.HttpMethod), httpMethod.ToString());
                var innerAnd = Query.And(routeMatch, statusCodeMatch);
                var outerAnd = Query.And(innerAnd, methodMatch);

                return db.Settings.FindOne(outerAnd);
            }
        }

        public List<Setting> FindAllParameterisedRoutes(HttpStatusCode statusCode, HttpMethodType httpMethod)
        {
            using (var db = dataContext.OpenSession())
            {
                var statusCodeMatch = Query.EQ(nameof(Setting.StatusCode), statusCode.ToString());
                var methodMatch = Query.EQ(nameof(Setting.HttpMethod), httpMethod.ToString());
                var routeMatch = Query.And(Query.Contains(nameof(Setting.Route), "{"), Query.Contains(nameof(Setting.Route), "}"));
                var innerAnd = Query.And(statusCodeMatch, methodMatch);

                return db.Settings.Find(Query.And(innerAnd, routeMatch)).ToList();
            }
        }

        public Setting Create(Setting setting)
        {
            using (var db = dataContext.OpenSession())
            {
                db.Settings.Insert(setting);
                db.Settings.EnsureIndex(x => x.Route, new IndexOptions {EmptyStringToNull = true, IgnoreCase = true, TrimWhitespace = true});
                db.Settings.EnsureIndex(x => x.StatusCode);
                db.Settings.EnsureIndex(x => x.HttpMethod, new IndexOptions {IgnoreCase = true, TrimWhitespace = true});

                return setting;
            }
        }

        public bool Update(ObjectId id, Setting setting)
        {
            using (var db = dataContext.OpenSession())
            {
                return db.Settings.Update(id, setting);
            }
        }

        public bool Delete(ObjectId id)
        {
            using (var db = dataContext.OpenSession())
            {
                return db.Settings.Delete(id);
            }
        }
    }
}