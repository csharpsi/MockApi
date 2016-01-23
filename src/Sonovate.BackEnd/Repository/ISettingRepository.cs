﻿using System.Collections.Generic;
using System.Net;
using LiteDB;
using Sonovate.BackEnd.Models;

namespace Sonovate.BackEnd.Repository
{
    public interface ISettingRepository
    {
        List<Setting> GetAll();
        Setting FindById(ObjectId id);
        Setting FindRoute(string route, HttpStatusCode statusCode, HttpMethodType httpMethod);
        List<Setting> FindAllParameterisedRoutes(HttpStatusCode statusCode, HttpMethodType httpMethod);
        Setting Create(Setting setting);
        bool Update(ObjectId id, Setting setting);
        bool Delete(ObjectId id);
    }
}