using System.Collections;
using System.Collections.ObjectModel;

namespace FluentData.Test.UnitTests
{
    public interface IDbContextCollection : IList<INamedDbContext>, IList, IReadOnlyList<INamedDbContext>
    {

    }

    public class DbContextCollection : KeyedCollection<string, INamedDbContext>, IDbContextCollection
    {
        protected override string GetKeyForItem(INamedDbContext item)
        {
            return item.Name;
        }
    }
}
