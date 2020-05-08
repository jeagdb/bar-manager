using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace barManagementTests.DataAccess
{
    [CollectionDefinition("testBarDB")]
    public class DBCollection : ICollectionFixture<Fixtures.barFixture>
    {
    }

}
