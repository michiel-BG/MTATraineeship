using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Documents;
using System.Web.UI;

namespace BlogCase
{
    public class Publish
    {

        // method
        public string Convert(string title, string img, string text, bool preview, int template)
        {
            // Initialize StringWriter instance.
            StringWriter stringWriter = new StringWriter();

            // Put HtmlTextWriter in using block because it needs to call Dispose.
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                // write HTML with post-variables
                writer.Write("<!doctype html>");
                writer.RenderBeginTag(HtmlTextWriterTag.Html); // <html>
                writer.RenderBeginTag(HtmlTextWriterTag.Head); // <head>
                writer.RenderBeginTag(HtmlTextWriterTag.Title); // <title>

                writer.Write(title); / /Post Title

                writer.RenderEndTag(); // </title>

                writer.AddAttribute("http-equiv", "X-UA-Compatible");
                writer.AddAttribute(HtmlTextWriterAttribute.Content, "IE=Edge");
                writer.RenderBeginTag(HtmlTextWriterTag.Meta); // <meta>
                writer.RenderEndTag(); // </meta>

                // choose between templates (css)
                if (template == 1)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "master.css");
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                    writer.RenderBeginTag(HtmlTextWriterTag.Link); // <link>
                    writer.RenderEndTag(); // </link>
                }
                else if (template == 2)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "master2.css");
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                    writer.RenderBeginTag(HtmlTextWriterTag.Link); // <link>
                    writer.RenderEndTag(); // </link>
                }

                if (template == 1 && preview)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "scale.css");
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                    writer.RenderBeginTag(HtmlTextWriterTag.Link); // <link>
                    writer.RenderEndTag(); // </link>
                }

                writer.RenderEndTag(); // </head>

                writer.RenderBeginTag(HtmlTextWriterTag.Body); // <body>

                if (template == 1)
                {
                    writer.RenderBeginTag("nav"); // <nav>
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul); // <ul>
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "selected");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                    writer.Write("Blog");
                    writer.RenderEndTag(); // </a>
                    writer.RenderEndTag(); // </li>
                    writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                    writer.Write("About");
                    writer.RenderEndTag(); // </a>
                    writer.RenderEndTag(); // </li>
                    writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                    writer.Write("Contact");
                    writer.RenderEndTag(); // </a>
                    writer.RenderEndTag(); // </li>
                    writer.RenderEndTag(); // </ul>
                    writer.RenderEndTag(); // </nav>

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "logo");
                    writer.RenderBeginTag("header"); // <header>
                    writer.RenderBeginTag(HtmlTextWriterTag.H1); // <h1>
                    writer.Write("The New York Rangers Blog");
                    writer.RenderEndTag(); // </h1>
                    writer.RenderBeginTag(HtmlTextWriterTag.H2); // <h2>
                    writer.Write("Doing the dirty work in front of the net");
                    writer.RenderEndTag(); // </h2>
                    writer.RenderEndTag(); // </header>
                }
                else if (template == 2)
                {
                    writer.RenderBeginTag("header"); // <header>
                    writer.RenderBeginTag(HtmlTextWriterTag.H1); // <h1>
                    writer.Write("The New York Rangers Blog");
                    writer.RenderEndTag(); // </h1>
                    writer.RenderBeginTag(HtmlTextWriterTag.H2); // <h2>
                    writer.Write("Doing the dirty work in front of the net");
                    writer.RenderEndTag(); // </h2>
                    writer.RenderEndTag(); // </header>

                    writer.RenderBeginTag("nav"); // <nav>
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul); // <ul>
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "selected");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                    writer.Write("Blog");
                    writer.RenderEndTag(); // </a>
                    writer.RenderEndTag(); // </li>
                    writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                    writer.Write("About");
                    writer.RenderEndTag(); // </a>
                    writer.RenderEndTag(); // </li>
                    writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                    writer.Write("Contact");
                    writer.RenderEndTag(); // </a>
                    writer.RenderEndTag(); // </li>
                    writer.RenderEndTag(); // </ul>
                    writer.RenderEndTag(); // </nav>
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Id, "content");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "mainContent");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>

                writer.RenderBeginTag("section"); // <section>
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "blogPost");
                writer.RenderBeginTag("article"); // <article>
                writer.RenderBeginTag("header"); // <header>
                writer.RenderBeginTag(HtmlTextWriterTag.H4); // <h4>
                writer.Write(title);
                writer.RenderEndTag(); // </h4>
                writer.WriteBreak();
                writer.RenderEndTag(); // </header>
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "data");
                writer.RenderBeginTag(HtmlTextWriterTag.P); // <p>
                writer.Write("Gepost door Michiel");
                writer.RenderEndTag(); // </p>
                if (img != null)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, img);
                    writer.RenderBeginTag(HtmlTextWriterTag.Img); // <img>
                    writer.RenderEndTag(); // </img>
                }
                writer.RenderBeginTag(HtmlTextWriterTag.P); // <p>

                writer.Write(text); // Post Content

                
                writer.RenderEndTag(); // </p>
                writer.RenderEndTag(); // </article>
                writer.RenderEndTag(); // </section>
                writer.RenderEndTag(); // </div>
                
                writer.RenderBeginTag("aside"); // <aside>

                writer.RenderBeginTag("section"); // <section>
                writer.RenderBeginTag("header"); // <header>
                writer.RenderBeginTag(HtmlTextWriterTag.H3); // <h3>
                writer.Write("Archives");
                writer.RenderEndTag(); // </h3>
                writer.RenderEndTag(); // </header>
                writer.RenderBeginTag(HtmlTextWriterTag.Ul); // <ul>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("December 2013");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("January 2014");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("February 2014");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("March 2014");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("April 2014");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("May 2014");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderEndTag(); // </ul>
                writer.RenderEndTag(); // </section>

                writer.RenderEndTag(); // </aside>

                writer.RenderEndTag(); // </div>

                writer.RenderBeginTag("footer"); // <footer>

                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "about");
                writer.RenderBeginTag("section"); // <section>
                writer.RenderBeginTag(HtmlTextWriterTag.H4); // <h4>
                writer.Write("About");
                writer.RenderEndTag(); // </h4>
                writer.RenderBeginTag(HtmlTextWriterTag.P); // <p>
                writer.Write("The New York Rangers are a professional ice hockey team based in the borough of Manhattan in New York City. They are members of the Metropolitan Division of the Eastern Conference of the National Hockey League (NHL). Playing their home games at Madison Square Garden, the Rangers are one of the oldest teams in the NHL, having joined in 1926 as an expansion franchise. They are part of the group of teams referred to as the Original Six, along with the Boston Bruins, Chicago Blackhawks, Detroit Red Wings, Montreal Canadiens, and Toronto Maple Leafs. The Rangers were the first NHL franchise in the United States to win the Stanley Cup,[1] which they have done four times, most recently in 1993–94.");
                writer.RenderEndTag(); // </p>
                writer.RenderEndTag(); // </section>

                writer.AddAttribute(HtmlTextWriterAttribute.Id, "popular");
                writer.RenderBeginTag("section"); // <section>
                writer.RenderBeginTag(HtmlTextWriterTag.H4); // <h4>
                writer.Write("Popular");
                writer.RenderEndTag(); // </h4>
                writer.RenderBeginTag(HtmlTextWriterTag.Ul); // <ul>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "Rangers1.html");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("Rangers vs. Canadiens Game 5 recap: Lundqvist chased as Habs force Game 6");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "Rangers2.html");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("New York Rangers News: Derek Stepan Is On The Ice This Morning");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "Rangers3.html");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("Rangers Vs. Canadiens: One Step Away");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "Rangers4.html");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("Rangers focused on closing; Stepan still a question mark; Trades paying off");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "Rangers5.html");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("Rangers vs. Canadiens Game 4: On playing with leads, and closing out");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "Rangers6.html");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write("Rangers take Game 4; Hagelin's speed; Suspensions and injuries");
                writer.RenderEndTag(); // </a>
                writer.RenderEndTag(); // </li>
                writer.RenderEndTag(); // </ul>
               
                writer.RenderEndTag(); // </section>

                writer.RenderEndTag(); // </div>
                writer.RenderEndTag(); // </footer>

                if (template == 1)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, "js/jquery.js");
                    writer.RenderBeginTag(HtmlTextWriterTag.Script); // <script>
                    writer.RenderEndTag(); // </script>
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, "js/jquery.backstretch.js");
                    writer.RenderBeginTag(HtmlTextWriterTag.Script); // <script>
                    writer.RenderEndTag(); // </script>
                    writer.RenderBeginTag(HtmlTextWriterTag.Script); // <script>
                    writer.Write("$.backstretch([\"images/NY.jpg\"]);");
                    writer.RenderEndTag(); // </script>
                }

                writer.RenderEndTag(); // </body>
                writer.RenderEndTag(); // </html>

            }
            // Return the result.
            return stringWriter.ToString();

        }


        // save HTML post
        public void SavePost(string t)
        {
            using (StreamWriter file = new StreamWriter(@"C:\Users\Student\Documents\Visual Studio 2012\Projects\BlogCase\BlogCase\HTML\BlogPost.html"))
            {
                file.WriteLine(t);
            }
        }
    }
}