using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Xunit.Sdk;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class XmlDataAttribute : DataAttribute
{

    public XmlDataAttribute(string fileName, string queryString, string atributeOrElementList )
    {
        FileName = fileName;
        QueryString = queryString;
        AtributeOrElementList = atributeOrElementList;
    }

    public string FileName { get; private set; }

    public string AtributeOrElementList { get; private set; }

    public string QueryString { get; private set; }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        if (testMethod == null)
            throw new ArgumentNullException("testMethod");

        ParameterInfo[] pars = testMethod.GetParameters();
        return DataSource(FileName, QueryString, pars.Select(par => par.ParameterType).ToArray());
    }

    IEnumerable<object[]> DataSource(string fileName, string xpath, Type[] parameterTypes)
    {
        XmlDocument xdoc = new XmlDocument();
        xdoc.Load(fileName);
        XmlNodeList nodeList = xdoc.SelectNodes(xpath);
        foreach (XmlNode theNode in nodeList)
        {
            //Converting node to array. Depends of AtribuiteOrElementList
            string[] theNodesExpression = AtributeOrElementList.Split(",");            
            List<string> theListAt = new List<string>();
            foreach (string exp in theNodesExpression)
            {
                XmlNode theSingleNode = theNode.SelectSingleNode(exp);                
                theListAt.Add(theSingleNode.Value);
            }
            yield return ConvertParameters(theListAt.ToArray(), parameterTypes);
        }
    }

    static string GetFullFilename(string filename)
    {
        string executable = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
        return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(executable), filename));
    }

    static object[] ConvertParameters(object[] values, Type[] parameterTypes)
    {
        object[] result = new object[values.Length];

        for (int idx = 0; idx < values.Length; idx++)
            result[idx] = ConvertParameter(values[idx], idx >= parameterTypes.Length ? null : parameterTypes[idx]);

        return result;
    }
    
    static object ConvertParameter(object parameter, Type parameterType)
    {
       
        if(parameterType == typeof(int))
        {
            return Convert.ToInt32(parameter);
        }
        else if(parameterType == typeof(decimal))
        {
            return Convert.ToDecimal(parameter);
        }
        else if(parameterType == typeof(double))
        {
            return Convert.ToDouble(parameter);
        }

        return parameter;
    }
}
