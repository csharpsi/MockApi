using System.Collections.Generic;
using LiteDB;
using MockApi.Web.Models;

namespace MockApi.Web.Repository
{
    public interface IMockRepository
    {
        List<Mock> ListAll();
        Mock FindById(string id);
        MockResponse FindActiveResponse(string path, HttpMethodType verb);
        Mock Find(string path, HttpMethodType verb);
        void Create(Mock mock);
        bool Update(string id, Mock mock);
        void Delete(string id);
    }
}