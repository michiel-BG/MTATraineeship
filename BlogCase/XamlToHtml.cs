using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace BlogCase
{
    public class XamlToHtml
    {
        // methods

        // convert XAML to HTML
        public string ConvertXamlToHtml(string xaml)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xaml));
            bool[] li = { false, false };
            string html = FormatHtml(reader, li);
            reader.Close();
            return html;
        }

        // recursive method to parse XAML and substitute XAML-tags with HTML-tags
        private string FormatHtml(XmlReader reader, bool[] li)
        {

            if ((reader.NodeType == XmlNodeType.Text) || ((reader.NodeType == XmlNodeType.Element) && (!reader.IsStartElement())))
            {
                return reader.Value;
            }

            string result = "";
            string style;

            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "List":
                        string marker = reader.GetAttribute("MarkerStyle");
                        if (marker == "Disc" || li[1])
                        {
                            if (reader.IsStartElement())
                            {
                                li[1] = true;
                                result += "<ul>" + FormatHtml(reader, li);
                            }
                            else
                            {
                                result += "</ul>";
                                li[1] = false;
                            }
                        }
                        else
                        {
                            if (reader.IsStartElement())
                            {
                                result += "<ol>" + FormatHtml(reader, li);
                            }
                            else
                            {
                                result += "</ol>";
                            }
                        }
                        break;
                    case "ListItem":
                        if (reader.IsStartElement())
                        {
                            if (reader.Read())
                            {
                                li[0] = true;
                                result += "<li>" + FormatHtml(reader, li);
                            }
                        }
                        else
                        {
                            result += "</li>";
                        }
                        break;
                    case "Paragraph":
                        if (reader.IsStartElement())
                        {
                            if (reader["TextAlignment"] == "Center")
                            {
                                result += "<p style=\"text-align:center\">" + FormatHtml(reader, li);
                            }
                            if (reader["TextAlignment"] == "Right")
                            {
                                result += "<p style=\"text-align:right\">" + FormatHtml(reader, li);
                            }
                            if (reader["TextAlignment"] == "Left")
                            {
                                result += "<p style=\"text-align:left\">" + FormatHtml(reader, li);
                            }
                            if (reader["TextAlignment"] == "Justify")
                            {
                                result += "<p style=\"text-align:justify\">" + FormatHtml(reader, li);
                            }
                            else
                            {
                                result += "<p>" + FormatHtml(reader, li);
                            }
                        }
                        else if (!li[0])
                        {
                            result += "</p>";
                            li[0] = false;
                        }
                        break;
                    case "Run":
                        if (reader.IsStartElement())
                        {
                            style = "";
                            bool _afb = false;
                            bool _yt = false;
                            string ytid = "";
                            if (reader["Foreground"] == "#FFFFFFFF")
                            {
                                _afb = true;
                            }
                            if (reader["Foreground"] == "#FFFF0000")
                            {
                                _yt = true;
                            }
                            if (reader["FontWeight"] == "Bold")
                            {
                                style += "font-weight:bold; ";
                            }
                            if (reader["FontStyle"] == "Italic")
                            {
                                style += "font-style:italic; ";
                            }
                            if (reader["TextDecorations"] == "Underline")
                            {
                                style += "text-decoration:underline; ";
                            }
                            if (reader["FontSize"] != null)
                            {
                                style += "font-size:" + reader["FontSize"] + "px;";
                            }
                            if (reader["FontFamily"] != null)
                            {
                                style += "font-family:" + reader["FontFamily"] + ";";
                            }
                            if (reader.Read())
                            {
                                if (_afb)
                                {
                                    result += "<img src=\"" + FormatHtml(reader, li) + "\"/>";
                                }
                                else if (_yt)
                                {
                                    Regex reg = new Regex(@"\[YOUTUBE\](?<id>.*?)\[/YOUTUBE\]", RegexOptions.IgnoreCase);
                                    MatchCollection matches;


                                    matches = reg.Matches(reader.Value);
                                    foreach (Match match in matches)
                                    {
                                       ytid = match.Groups["id"].Value;
                                    }

                                    result += "<iframe width=\"560\" height=\"315\" src=\"http://www.youtube.com/embed/" + ytid + "\" frameborder=\"0\" allowfullscreen></iframe>";
                                }
                                else
                                {
                                    result += "<span style=\"" + style + "\">" + FormatHtml(reader, li);
                                }
                            }
                        }
                        else
                        {
                            result += "</span>";
                        }
                        break;
                    case "Hyperlink":
                        if (reader.IsStartElement())
                        {
                            string link = "";
                            if (reader["NavigateUri"] != null)
                            {
                                link = reader["NavigateUri"];
                                result += "<a href=\"" + link + "\">" + FormatHtml(reader, li);
                            }

                        }
                        else
                        {
                            result += "</a>";
                        }
                        break;
                }
            }
            return result;
        }
    }
}
