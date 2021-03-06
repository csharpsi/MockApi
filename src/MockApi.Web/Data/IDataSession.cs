﻿using System;
using LiteDB;
using MockApi.Web.Models;

namespace MockApi.Web.Data
{
    public interface IDataSession : IDisposable
    {
        LiteCollection<Mock> Mocks { get; } 
    }
}