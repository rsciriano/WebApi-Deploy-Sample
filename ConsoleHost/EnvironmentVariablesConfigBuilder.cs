using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleHost
{
    public class EnvironmentVariablesConfigBuilder: System.Configuration.ConfigurationBuilder
    {
        public override XmlNode ProcessRawXml(XmlNode rawXml)
        {

            rawXml.InnerXml = Environment.ExpandEnvironmentVariables(rawXml.InnerXml);

            return rawXml;
        }

    }
}
