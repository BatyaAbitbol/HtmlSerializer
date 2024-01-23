using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace HtmlSerializer
{
    public class HtmlHelper
    {
        private static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Helper => _instance;
        public List<string> Tags { get; set; }
        public List<string> VoidTags { get; set; }
        private HtmlHelper()
        {
            Tags = new List<string>();
            VoidTags = new List<string>();

            // load data from Json files
            var pathTags = ".\\Json\\HtmlTags.json";
            var pathVoidTags = ".\\Json\\HtmlVoidTags.json";
            var content = File.ReadAllText(pathTags);
            Tags = (List<string>)JsonSerializer.Deserialize(content, Tags.GetType());
            content = File.ReadAllText(pathVoidTags);
            VoidTags = (List<string>)JsonSerializer.Deserialize(content, VoidTags.GetType());
        }
        public static bool IsValidTag(string tag)
        {
            return _instance.Tags.Contains(tag) || _instance.VoidTags.Contains(tag);
                
        }
    }
}
