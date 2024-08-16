using FluentData.Core;
using System.Data;

namespace FluentData.Test.UnitTests
{
    public interface INamedDbContext : IDbContext
    {
        string Name { get; }
    }

    public class NamedDbContext : DbContext, INamedDbContext
    {
        public NamedDbContext(string name, string connectionString, IDbProvider fluentDataProvider, string? providerName = null)
        {
            this.Name = name;
            this.ConnectionString(connectionString, fluentDataProvider, providerName);
        }

        public NamedDbContext(string connectionName, IDbProvider fluentDataProvider)
        {
            this.Name = connectionName;
            this.ConnectionStringName(connectionName, fluentDataProvider);
        }

        public string Name { get; }

        private bool isDebugMode = false;
        public bool IsDebugMode
        {
            get
            {
                return isDebugMode;
            }
            set
            {
                isDebugMode = value;
                if (isDebugMode)
                {
                    this.OnExecuted(ExecutedAction);
                }
            }
        }

        private readonly Action<CommandEventArgs> ExecutedAction = e =>
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(e.Command.CommandText);
            foreach (IDataParameter item in e.Command.Parameters)
            {
                Console.WriteLine("{0}:{1}", item.ParameterName, item.Value);
            }
            Console.ResetColor();
            Console.WriteLine();
        };
    }
}
