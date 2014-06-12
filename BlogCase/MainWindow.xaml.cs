using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using Microsoft.Windows.Controls.Ribbon;
using System.Xml;
using System.Windows.Markup;
using SHDocVw;
using System.Runtime.InteropServices;


namespace BlogCase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private bool _isChanged = false;

        public MainWindow()
        {
            InitializeComponent();

            // intialize fonts and fontsizes in Ribbon
            fonts.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontsComboBox.SelectedItem = "Segoe UI";
            fontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            fontSizeComboBox.SelectedItem = "12";
        }

        // methods

        // get XAML string from RichTextBox
        private string getXaml()
        {
            TextRange text = new TextRange(tekst.Document.ContentStart, tekst.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                text.Save(ms, DataFormats.Xaml);
                return ASCIIEncoding.Default.GetString(ms.ToArray());

            }
        }

        // method to save file
        private void SaveChanges()
        {
            string zaml = getXaml();
            string svtitel = (titel.Text == "" ? " " : titel.Text);
            string afbeelding;
            if (imgPhoto.Source != null)
            {
                afbeelding = (imgPhoto.Source as BitmapImage).UriSource.ToString();
            }
            else
            {
                afbeelding = " ";
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Untitled";
            sfd.DefaultExt = ".xml";
            sfd.Filter = "XML documents (.xml)|*.xml";
            sfd.Title = "Save as";
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                using (XmlWriter writer = XmlWriter.Create(@sfd.FileName))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("Bestand");

                    writer.WriteElementString("titel", svtitel);
                    writer.WriteElementString("afbeelding", afbeelding);
                    writer.WriteElementString("RTB", zaml);

                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }
            }
        }

        // method to check if file is changed, if so, ask user if file needs to be saved
        private bool CheckSave()
        {
            if (_isChanged)
            {
                var result
                    = (System.Windows.Forms.MessageBox.Show("You have made changes, would you like to save them?"
                    , "Save Changes"
                    , System.Windows.Forms.MessageBoxButtons.YesNoCancel
                    , System.Windows.Forms.MessageBoxIcon.Warning));

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        SaveChanges();
                        _isChanged = false;
                        break;
                    case System.Windows.Forms.DialogResult.No:
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        return true;
                }
            }
            return false;
        }

        // method to publish post
        private void Publish(bool preview, int template)
        {
            string zaml = getXaml();

            string mainimg = null;
            if (imgPhoto.Source != null)
            {
                mainimg = (imgPhoto.Source as BitmapImage).UriSource.ToString();
            }
            // create a new blogpost
            Blogpost blog = new Blogpost(titel.Text, mainimg, zaml, template);

            blog.Publish(preview);
        }

        // events

        // Event Handler to apply font
        private void RibbonFont_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            tekst.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontsComboBox.SelectedItem);
        }

        // Event Handler to apply font-size
        private void RibbonFontSize_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            tekst.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.SelectedItem);
        }

        // Event Handler to open new file
        private void RibbonApplicationMenuItem_Open(object sender, RoutedEventArgs e)
        {
            string xaml = null;
            string optitel = null;
            string afbeelding = null;

            bool cancel = CheckSave(); // check if file is changed
            if (!cancel)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = ".xml";
                ofd.Filter = "XML documents (.xml)|*.xml";
                FlowDocument fd = null;

                if (ofd.ShowDialog() == true)
                {
                    // save post as XML-file with tags around different fields
                    using (XmlReader reader = XmlReader.Create(ofd.OpenFile()))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "RTB")
                                {
                                    reader.Read();
                                    xaml = reader.Value;
                                }
                                else if (reader.Name == "titel")
                                {
                                    reader.Read();
                                    optitel = reader.Value;
                                }
                                else if (reader.Name == "afbeelding")
                                {
                                    reader.Read();
                                    afbeelding = reader.Value;
                                }
                            }
                        }
                    }
                }

                try
                {
                    StringReader stringReader = new StringReader(xaml);
                    using (XmlReader reader = XmlReader.Create(stringReader))
                    {
                        Section sec = XamlReader.Load(reader) as Section;
                        fd = new FlowDocument();
                        while (sec.Blocks.Count > 0)
                        {
                            var block = sec.Blocks.FirstBlock;
                            sec.Blocks.Remove(block);
                            fd.Blocks.Add(block);
                        }
                    }
                    tekst.Document = fd;
                    titel.Text = optitel;
                    if (afbeelding != null)
                    {
                        imgPhoto.Source = new BitmapImage(new Uri(afbeelding));
                    }
                    else
                    {
                        imgPhoto.Source = null;
                    }
                }
                catch
                {
                    // Do nothing
                }
            }
        }

        // Event Handler to save file
        private void RibbonApplicationMenuItem_Save(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }

        // Event Handler to create a new file
        private void RibbonApplicationMenuItem_New(object sender, RoutedEventArgs e)
        {
            bool cancel = CheckSave(); // check if file is changed
            if (!cancel)
            {
                titel.Text = null;
                imgPhoto.Source = null;
                tekst.Document = new FlowDocument();
            }
        }

        // Event Handler to note that a change in the file is made
        private void fieldChanged(object sender, TextChangedEventArgs e)
        {
            _isChanged = true;
        }

        // Eventhandler to handle save if form is closed
        private void RibbonWindow_Closing(object sender, EventArgs e)
        {
            CheckSave();
        }

        // Eventhandler to select featured image
        private void _btnHeadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecteer een afbeelding";
            op.Filter = "Toegestane formaten|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
                imgPhotoTT.Source = imgPhoto.Source;
            }
            _isChanged = true;
        }

        // Eventhandler to publish post if button is clicked
        private void _btnPublish_Click(object sender, RoutedEventArgs e)
        {
            int template;
            if (Template1.IsChecked == true)
            {
                template = 1;
            }
            else
            {
                template = 2;
            }
            Publish(false, template);

            // Open IE and preview post
            Process.Start("IExplore.exe", @"C:\Users\Student\Documents\Visual Studio 2012\Projects\BlogCase\BlogCase\HTML\BlogPost.html");
        }

        // Eventhandler to add an image inside the post
        private void _btnImage_Click(object sender, RoutedEventArgs e)
        {
            FileDialog imgOfd = new OpenFileDialog();
            imgOfd.Title = "Insert Image";
            imgOfd.Filter = "Toegestane formaten|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (imgOfd.ShowDialog() == true)
            {
                var clipBoardData = Clipboard.GetDataObject();
                BitmapImage bitmapImage = new BitmapImage(new Uri(imgOfd.FileName, UriKind.Absolute));
                Paragraph insert = new Paragraph();
                Run run = new Run();
                run.Foreground = Brushes.White;
                run.Text = imgOfd.FileName;
                insert.Inlines.Add(run);
                tekst.Document.Blocks.Add(insert);
                Clipboard.SetImage(bitmapImage);
                tekst.Paste();
                Clipboard.SetDataObject(clipBoardData);
            }
        }

        // Eventhandler to add a hyperlink inside the post
        private void _btnLink_Click(object sender, RoutedEventArgs e)
        {
            TextSelection ts = tekst.Selection;
            TextPointer start = ts.Start;
            TextPointer end = ts.End;

            TextRange before = new TextRange(tekst.Document.ContentStart, start);
            TextRange after = new TextRange(end, tekst.Document.ContentEnd);
            TextRange linker = new TextRange(start, end);

            Paragraph myParagraph = new Paragraph();
            myParagraph.Inlines.Clear();
            myParagraph.Inlines.Add(before.Text);

            Hyperlink hyperLink = new Hyperlink();
            hyperLink.IsEnabled = true;

            // ask the user for an URL
            var dialog = new MyDialog();
            dialog.SomeData = "Enter a valid URL (like ww.example.com)";
            // If MyDialog has properties that affect its behavior, set them here
            var result = dialog.ShowDialog();
            string link = "";

            if (result == false)
            {
                // cancelled
            }
            else if (result == true)
            {
                
                link = dialog.SomeData;
                link = "http://" + link;

                hyperLink.NavigateUri = new Uri(link);
                hyperLink.Inlines.Add(ts.Text);

                myParagraph.Inlines.Add(hyperLink);
                myParagraph.Inlines.Add(after.Text);

                tekst.Document.Blocks.Clear();

                tekst.Document.Blocks.Add(myParagraph);
            }

            
        }

        // Eventhandler to add a YouTube-video inside the post
        private void _btnYT_Click(object sender, RoutedEventArgs e)
        {
            TextSelection ts = tekst.Selection;
            TextPointer start = ts.Start;
            TextPointer end = ts.End;

            TextRange before = new TextRange(tekst.Document.ContentStart, start);
            TextRange after = new TextRange(end, tekst.Document.ContentEnd);
            TextRange linker = new TextRange(start, end);

            Paragraph myParagraph = new Paragraph();
            myParagraph.Inlines.Clear();
            myParagraph.Inlines.Add(before.Text);


            var dialog = new MyDialog();
            dialog.SomeData = "Enter a YouTube ID";
            // If MyDialog has properties that affect its behavior, set them here
            var result = dialog.ShowDialog();
            string link = "";

            if (result == false)
            {
                // cancelled
            }
            else if (result == true)
            {
                
                link = dialog.SomeData;

                Run run = new Run();
                run.Foreground = Brushes.Red;
                run.Text = "[YOUTUBE]" + link + "[/YOUTUBE]";

                myParagraph.Inlines.Add(run);
                myParagraph.Inlines.Add(after.Text);

                tekst.Document.Blocks.Clear();

                tekst.Document.Blocks.Add(myParagraph);
            }
        }

        // Eventhandler to preview post as template 1
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Publish(true, 1);
            PreviewWebBrowser.Navigate(@"C:\Users\Student\Documents\Visual Studio 2012\Projects\BlogCase\BlogCase\HTML\BlogPost.html");
        }

        // Eventhandler to preview post as template 2
        private void Image_MouseEnter_2(object sender, MouseEventArgs e)
        {
            Publish(true, 2);
            PreviewWebBrowser2.Navigate(@"C:\Users\Student\Documents\Visual Studio 2012\Projects\BlogCase\BlogCase\HTML\BlogPost.html");
        }

        // Eventhandler to close the file
        private void RibbonApplicationMenuItem_Close(object sender, RoutedEventArgs e)
        {
            CheckSave(); // check if file is changed
            Close();
        }

    }
}
