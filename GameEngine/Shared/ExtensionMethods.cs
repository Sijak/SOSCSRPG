using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameEngine.Shared
{
    static class ExtensionMethods
    {
        public static int AttributeAsInt (this XmlNode node, string attributeName)
        {
            return Convert.ToInt32(node.AttributeAsString(attributeName));
        }

        public static string AttributeAsString (this XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes?[attributeName];
            if (attribute==null)
            {
                throw new ArgumentException($"The Attribute {attributeName} does not exist.");
            }
            //return Convert.ToString(attribute.Value); This works too. This is what i did.
            return attribute.Value;
        }
    }
}
