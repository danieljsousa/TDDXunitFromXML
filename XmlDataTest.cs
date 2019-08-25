using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FirstTest
{
    public class XmlDataTest
    {
        [Theory]
        [XmlData(@"../../../teste.xml","/teste","./valor1/text(),./valor2/text(),./@valor3")]
        public void XMLDATAFuncionaTest(int x, int y,int z)
        {
            Assert.True((x + y + z) == 80);
        }
    }
}
