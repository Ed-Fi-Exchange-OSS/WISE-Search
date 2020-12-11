using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EDFI.Modules.Search.Extensions
{
    public static class XElementExtensions
    {
        public static string AttributeValueOrNull(this XElement element, string attributeName)
        {
            return element.Attributes().Where(x => x.Name == attributeName).Select(x => x.Value).FirstOrDefault();
        }

        public static XElement ReplaceTokens(this XElement element, Dictionary<string, string> replacementTokens)
        {
            var xml = element
                .ToString()
                .ReplaceTokens(replacementTokens);

            return XElement.Parse(xml);
        }

    }
}