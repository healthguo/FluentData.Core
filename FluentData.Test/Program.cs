using FluentData.Test.UnitTests;
using Newtonsoft.Json;

var dataTable = await DbHelper.Context
        .Select<object>("*")
        .From("test")
        .QueryDataTableAsync();

Console.WriteLine(JsonConvert.SerializeObject(dataTable));
Console.WriteLine();