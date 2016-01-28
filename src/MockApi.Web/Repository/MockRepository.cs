using System.Collections.Generic;
using System.Linq;
using LiteDB;
using MockApi.Web.Data;
using MockApi.Web.Models;

namespace MockApi.Web.Repository
{
    public class MockRepository : IMockRepository
    {
        private readonly IDataContext dataContext;

        private static string Path => nameof(Mock.Path);
        private static string Verb => nameof(Mock.Verb);

        public MockRepository(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public List<Mock> ListAll()
        {
            using (var session = dataContext.OpenSession())
            {
                return session.Mocks.FindAll().ToList();
            }
        }

        public Mock FindById(string id)
        {
            using (var session = dataContext.OpenSession())
            {
                return session.Mocks.FindById(new ObjectId(id));
            }
        }

        public MockResponse FindActiveResponse(string path, HttpMethodType verb)
        {
            var mock = Find(path, verb);
            
            var activeStatus = mock?.ActiveStatusCode;

            return mock?.Responses.FirstOrDefault(x => x.StatusCode == activeStatus);
        }

        public Mock Find(string path, HttpMethodType verb)
        {
            using (var session = dataContext.OpenSession())
            {
                var pathEq = Query.EQ(Path, path);
                var verbEq = Query.EQ(Verb, verb.ToString());
                var query = Query.And(pathEq, verbEq);

                var mock = session.Mocks.FindOne(query);

                if (mock == null)
                {
                    var routeMatch = Query.And(Query.Contains(Path, "{"), Query.Contains(Path, "}"));
                    var routes = session.Mocks.Find(Query.And(routeMatch, Query.EQ(Verb, verb.ToString()))).ToList();

                    var fuzzyMock = (from route in routes
                                     let co = route.Path.DiceCoefficient(path)
                                     orderby co descending 
                                     select route).FirstOrDefault();

                    if (fuzzyMock == null)
                    {
                        return null;
                    }

                    mock = fuzzyMock;
                }

                return mock;
            }
        }

        public void Create(Mock mock)
        {
            using (var session = dataContext.OpenSession())
            {
                session.Mocks.Insert(mock);
                session.Mocks.EnsureIndex(x => x.Path, new IndexOptions { EmptyStringToNull = true, IgnoreCase = true, TrimWhitespace = true });
                session.Mocks.EnsureIndex(x => x.Verb, new IndexOptions { EmptyStringToNull = true, IgnoreCase = true, TrimWhitespace = true });
            }
        }

        public bool Update(string id, Mock mock)
        {
            using (var session = dataContext.OpenSession())
            {
                return session.Mocks.Update(new ObjectId(id), mock);
            }
        }

        public void Delete(string id)
        {
            using (var session = dataContext.OpenSession())
            {
                session.Mocks.Delete(new ObjectId(id));
            }
        }
    }
}