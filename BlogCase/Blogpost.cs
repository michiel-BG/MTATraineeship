using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Text;
using System.Threading.Tasks;

namespace BlogCase
{
    class Blogpost
    {
        // fields
        private string _title;
        private string _mainimg;
        private string _text;
        private int _template;

        // constructor
        public Blogpost(string title, string mainimg, string text, int template)
        {
            _title = title;
            _text = text;
            _mainimg = mainimg;
            _template = template;
        }
        

        // methods
        public void Publish(bool preview)
        {
            // convert XAML to HTML
            var xhc = new XamlToHtml();
            string html = xhc.ConvertXamlToHtml(_text);

            // parse HTML
            Publish p = new Publish();
            string converted = p.Convert(_title, _mainimg, html, preview, _template);

            // save the post as an HTML file
            p.SavePost(converted);
        }
    }
}
