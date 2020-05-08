using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace barManagementTests.Models
{
    public class Cocktails
    {
        [Fact]
        public void Constructor()
        {
            var cocktailResult = new BarManagement.Models.Cocktails();
            Assert.NotNull(cocktailResult);
        }
    }
}
