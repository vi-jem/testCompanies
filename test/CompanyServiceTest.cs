using System;
using Xunit;
using myrestful.Models;

namespace myrestful.tests
{
    public class CompanyServiceTest
    {
        [Fact]
        public void PassingTest()
        {
        //Given
        Company a = new Company();
        //When
        
        //Then
        Assert.Equal(4, Add(2,2));
        }

        int Add(int x, int y)
        {
            return x+y;
        }
    }
}
