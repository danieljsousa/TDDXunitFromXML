using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FirstTest
{
    public class XmlDataTest
    {
        [Theory]
        [XmlData(@"../../../test.xml","/tests/test1","./value1/text(),./value2/text(),./@value3")]
        public void XMLDATATest1(int x, int y,int z)
        {
            Assert.True((x + y + z) == 80);
        }

		
        [Theory]
        [XmlData(@"../../../test.xml","/tests/test2","./a/@value,./b/text(),./z/c/text()")]
        public void XMLDATATest2(int a, int b,int c)
        {
            Assert.True(a > (b+c) );
        }

		[Theory]
        [XmlData(@"../../../test2.xml","/tests/functionSum/test","./@a,./@b,./@c,./@result")]
        public void XMLDATATestSum(int a, int b,int c, int result)
        {
			
            Assert.True((a+b+c) == result);
        }	

    }		    
}
