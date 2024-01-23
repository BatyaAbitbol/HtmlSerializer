using HtmlSerializer;
using HtmlSerializer.HTML_Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class HtmlAttribute
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
        public HtmlAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
    public class HtmlElement
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public HashSet<HtmlAttribute>? Attributes { get; set; }
        public HashSet<string>? Classes { get; set; }
        public string? InnerHtml { get; set; }

        public HtmlElement? Parent { get; set; }
        public List<HtmlElement>? Children { get; set; }

        public HtmlElement(string name, HashSet<HtmlAttribute> attributes)
        {
            Name = name;
            Attributes = attributes;
            HtmlAttribute? id = Attributes.FirstOrDefault(a => a.Name == "id");
            if (id is not null)
                Id = id.Value;
            
            var classes = Attributes.FirstOrDefault(a => a.Name == "class");
            if (classes is not null)
            {
                var splitedClasses = classes.Value.Split(" ");
                Classes = splitedClasses.ToHashSet();
            }
            Children = new();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            var elements = new Queue<HtmlElement>();
            elements.Enqueue(this);
            
            while(elements.Count > 0)
            {
                var element = elements.Dequeue();
                element.Children?.ForEach(e =>  elements.Enqueue(e));
                if (element == this)
                    continue;
                else yield return element;
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var ancestor = this;
            while(ancestor != null)
            {
                ancestor = ancestor.Parent;
                yield return ancestor;
            }
        }
        public override string ToString()
        {
            var _classes = string.Empty;
            if(Classes != null)
            {
                foreach (var _class in Classes)
                {
                    _classes += $"{_class} ";
                }
            }
            return $"Name:{this.Name}, Id: {this.Id}, Classes: {_classes}";
        }
    }
    public static class HtmlElementExtension {
        public static bool IsMatch(HtmlElement element, Selector selector)
        {
            if ((selector.TagName is null || element.Name == selector.TagName) && (element.Id == selector.Id || selector.Id == null))
            {
                if (selector.Classes.Count > 0)
                {
                    if (element.Classes == null)
                        return false;
                    foreach (var _class in selector.Classes)
                    {
                        if (!element.Classes.Contains(_class))
                            return false;
                    }
                    return true ;
                }
                else return true;
            }
            return false;
        }
        public static HashSet<HtmlElement> FindElementsBySelector(this HtmlElement element, Selector selector)
        {
            var elements = new HashSet<HtmlElement>();
            FindElementsRec(element, selector, ref elements);
            return elements;
        }
        public static void FindElementsRec(HtmlElement element, Selector selector, ref HashSet<HtmlElement> elements)
        {
            if (selector == null)
            {
                elements.Add(element);
                return;
            }
            var decsendants = element.Descendants();
            decsendants = decsendants.Where(d => IsMatch(d, selector)).ToHashSet();
            foreach (var decsendant in decsendants)
                FindElementsRec(decsendant, selector.Child, ref elements);
        }
    }
}