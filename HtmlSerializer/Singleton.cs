using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class Singleton
    {
        private static Singleton _instance = new Singleton();
        public static Singleton Instance => _instance;
        private Singleton()
        {
            
        }
    }
}

/*

using System.Text.RegularExpressions;

var html = await Load("https://hebrewbooks.org/beis");

var cleanHtml = new Regex("\\s").Replace(html, "");

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);

var htmlElement = "<div id=\"my-div\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";
var attributes = new Regex("([^\\s].*)=\"(.*?)\"").Matches(htmlElement);

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

// read file content
var content = File.ReadAllText("context.txt");
Console.ReadLine();

// yield return
var evenNumbers = GetEvenNumbers(20);

foreach (var number in evenNumbers)
{
    Console.WriteLine(number);
}
IEnumerable<int> GetEvenNumbers(int lastNumber)
{
    for (var i = 0; i < lastNumber; i += 2)
        yield return i;
}
*/