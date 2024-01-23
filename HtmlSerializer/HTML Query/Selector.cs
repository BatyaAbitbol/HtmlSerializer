using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer.HTML_Query
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public HashSet<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            Classes = new HashSet<string>();
        }
        public static Selector ConvertQueryToSelector(string query)
        {
            var queryLevels = query.Split(' ');
            var root = new Selector();
            root.Parent = null;
            var current = root;
            foreach (var queryLevel in queryLevels)
            {
                var idIndex = queryLevel.IndexOf('#');
                var classIndex = queryLevel.IndexOf('.');

                var delimiters = new char[] { '.', '#' };
                var splitted = queryLevel.Split(delimiters);
                var tag = splitted[0];
                if (HtmlHelper.IsValidTag(tag))
                {
                    current.TagName = tag;
                }
                if (idIndex != -1)
                {
                    var _id = classIndex != -1 && idIndex > classIndex ? splitted[2] : splitted[1];
                    current.Id = "\"" + _id + "\"";
                }
                if (classIndex != -1)
                {
                    var _class = idIndex != -1 && idIndex < classIndex ? splitted[2] : splitted[1];
                    current.Classes.Add("\"" + _class + "\"");
                }
                var child = new Selector();
                child.Parent = current;
                current.Child = child;
                current = current.Child;
            }
            current.Parent.Child = null;
            return root;
        }
    }
}
