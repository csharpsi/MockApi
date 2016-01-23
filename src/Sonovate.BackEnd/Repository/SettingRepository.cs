using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using Humanizer;
using LiteDB;
using Sonovate.BackEnd.Models;

namespace Sonovate.BackEnd.Repository
{
    public class SettingRepository : ISettingRepository
    {
        private static readonly string databaseFile = HostingEnvironment.MapPath("~/App_Data/SonovateBackendStub.db");
        private static readonly string collectionName = nameof(Setting).Pluralize().ToLower();

        public List<Setting> GetAll()
        {
            using (var db = BuildDb())
            {
                var collection = GetCollectionFromDb(db);
                return collection
                    .FindAll()
                    .OrderBy(x => x.Route)
                    .ThenBy(x => x.StatusCode)
                    .ToList();
            }
        }

        public Setting FindById(ObjectId id)
        {
            using (var db = BuildDb())
            {
                var collection = GetCollectionFromDb(db);
                return collection.FindById(id);
            }
        }

        public Setting FindRoute(string route, HttpStatusCode statusCode, HttpMethodType httpMethod)
        {
            using (var db = BuildDb())
            {
                var collection = GetCollectionFromDb(db);

                var routeMatch = Query.EQ(nameof(Setting.Route), route);
                var statusCodeMatch = Query.EQ(nameof(Setting.StatusCode), statusCode.ToString());
                var methodMatch = Query.EQ(nameof(Setting.HttpMethod), httpMethod.ToString());
                var innerAnd = Query.And(routeMatch, statusCodeMatch);
                var outerAnd = Query.And(innerAnd, methodMatch);

                return collection.FindOne(outerAnd);
            }
        }

        public List<Setting> FindAllParameterisedRoutes(HttpStatusCode statusCode, HttpMethodType httpMethod)
        {
            using (var db = BuildDb())
            {
                var collection = GetCollectionFromDb(db);

                var statusCodeMatch = Query.EQ(nameof(Setting.StatusCode), statusCode.ToString());
                var methodMatch = Query.EQ(nameof(Setting.HttpMethod), httpMethod.ToString());
                var routeMatch = Query.And(Query.Contains(nameof(Setting.Route), "{"), Query.Contains(nameof(Setting.Route), "}"));
                var innerAnd = Query.And(statusCodeMatch, methodMatch);

                return collection.Find(Query.And(innerAnd, routeMatch)).ToList();
            }
        }

        public Setting Create(Setting config)
        {
            using (var db = BuildDb())
            {
                var collection = GetCollectionFromDb(db);
                collection.Insert(config);
                collection.EnsureIndex(x => x.Route, new IndexOptions {EmptyStringToNull = true, IgnoreCase = true, TrimWhitespace = true});
                collection.EnsureIndex(x => x.StatusCode);
                collection.EnsureIndex(x => x.HttpMethod, new IndexOptions {IgnoreCase = true, TrimWhitespace = true});

                return config;
            }
        }

        public bool Update(ObjectId id, Setting setting)
        {
            using (var db = BuildDb())
            {
                var collection = GetCollectionFromDb(db);
                return collection.Update(id, setting);
            }
        }

        public bool Delete(ObjectId id)
        {
            using (var db = BuildDb())
            {
                var collection = GetCollectionFromDb(db);
                return collection.Delete(id);
            }
        }

        private static LiteCollection<Setting> GetCollectionFromDb(LiteDatabase db)
        {
            return db.GetCollection<Setting>(collectionName);
        }

        private static LiteDatabase BuildDb()
        {
            return new LiteDatabase(databaseFile);
        }
    }
}