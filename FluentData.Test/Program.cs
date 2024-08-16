using FluentData.Test.UnitTests;
using Newtonsoft.Json;

var dataTable = DbHelper.Context
        .Select<object>("*")
        .From("test")
        .QueryDataTable();
Console.WriteLine(JsonConvert.SerializeObject(dataTable));
Console.WriteLine();