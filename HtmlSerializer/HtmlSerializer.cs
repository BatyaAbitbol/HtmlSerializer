using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public static class HtmlSerializer
    {
        public static async Task<HtmlElement> Serialize(string url)
        {
            var html = await Load(url);
            // var cleanHtml = new Regex("([^ ]\\s])").Replace(html, "");
            var htmlLines = new Regex("<(.*?)>").Split(html).Where(s => s.Length > 0);
            htmlLines = htmlLines.Where(line => new Regex("(\\s)").Replace(line, "") != string.Empty);

            var root = htmlLines.First(l => l.StartsWith("html"));
            var htmlAttributes = GetAttributes(root);
            var name = GetName(root);

            HtmlElement rootElement = new(name, htmlAttributes);
            rootElement.Parent = null;
            HtmlElement currentElement = rootElement;
            foreach (var line in htmlLines)
            {
                var firstWord = GetName(line);

                if (line.StartsWith("html") || line.StartsWith("!DOCTYPE")) // rootElement
                    continue;
                else if (line.StartsWith("/html")) // end of html file
                    break;
                else if (line.StartsWith("/")) // closed tag
                {
                    currentElement = currentElement.Parent;
                }
                else if (HtmlHelper.Helper.Tags.Contains(firstWord) || HtmlHelper.Helper.VoidTags.Contains(firstWord)) // a tag
                {
                    // create new element
                    htmlAttributes = GetAttributes(line);
                    var newElement = new HtmlElement(firstWord, htmlAttributes);
                    newElement.Parent = currentElement;
                    currentElement.Children.Add(newElement);

                    if (!line.EndsWith("/") && !HtmlHelper.Helper.VoidTags.Contains(line))
                    {
                        currentElement = newElement;
                    }
                }
                else // inner text of the current element
                {
                    var content = new Regex("(\\s)").Replace(line, "");
                    currentElement.InnerHtml += content;
                }
            }
            return rootElement;
        }
        static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
        static HashSet<HtmlAttribute> GetAttributes(string htmlElement)
        {
            var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
            var attributesSet = new HashSet<HtmlAttribute>();
            foreach (var attribute in attributes)
            {
                var splitedAttribute = attribute?.ToString()?.Split("=");
                attributesSet.Add(new HtmlAttribute(splitedAttribute?[0], splitedAttribute[1]));
            }
            return attributesSet;
        }
        static string GetName(string htmlElement)
        {
            var name = htmlElement.Split(" ")[0];
            return name;
        }
    }
}