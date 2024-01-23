using HtmlSerializer;
using HtmlSerializer.HTML_Query;

var url = "http://localhost:3000/";
var root = await HtmlSerializer.HtmlSerializer.Serialize(url);

//var query = "div i#logo-id.logo";
var query = "div #my-div-2 p";
//var query = "#root .logo";

var s = Selector.ConvertQueryToSelector(query);
var selected = root.FindElementsBySelector(s);

Console.WriteLine();