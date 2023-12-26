using BaseDemo.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace BaseDemo.Helpers.Component
{
    public class RichTextBoxHelper : DependencyObject
    {
        private static HashSet<Thread> _recursionProtection = new HashSet<Thread>();

        #region DocumentXaml

        public static string GetDocumentXaml(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentXamlProperty);
        }
        public static void SetDocumentXaml(DependencyObject obj, string value)
        {
            _recursionProtection.Add(Thread.CurrentThread);
            obj.SetValue(DocumentXamlProperty, value);
            _recursionProtection.Remove(Thread.CurrentThread);
        }

        public static readonly DependencyProperty DocumentXamlProperty =
            DependencyProperty.RegisterAttached(
                "DocumentXaml",
                typeof(string),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = XamlWriter.Save(new FlowDocument { LineHeight = 3 }),
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = (obj, e) =>
                    {
                        if (_recursionProtection.Contains(Thread.CurrentThread)) return;

                        var richTextBox = (RichTextBox)obj;

                        try
                        {
                            // Parse the XAML to a document (or use XamlReader.Parse())
                            var xaml = GetDocumentXaml(richTextBox);

                            var newXaml = xaml;
                            if (!xaml.IsNullOrEmpty() && !GetLocalImagePath(richTextBox).IsNullOrEmpty() && xaml.Contains("<Image"))
                            {
                                Regex regex = new Regex(@"<Image Source[\S]*/image/note");
                                newXaml = regex.Replace(xaml, "<Image Source=\"file:///" + GetLocalImagePath(richTextBox));
                            }

                            var doc = (FlowDocument)XamlReader.Parse(newXaml);
                            var range = new TextRange(doc.ContentStart, doc.ContentEnd);
                            // Set the document
                            richTextBox.Document = doc;
                        }
                        catch(Exception ex)
                        {
                            richTextBox.Document = new FlowDocument { LineHeight = 3 };
                        }

                        // When the document changes update the source
                        //richTextBox.TextChanged += (obj2, e2) =>
                        //{
                        //    RichTextBox richTextBox2 = obj2 as RichTextBox;
                        //    if (richTextBox2 != null)
                        //    {
                        //        SetDocumentXaml(richTextBox, XamlWriter.Save(richTextBox.Document));
                        //    }
                        //};

                        richTextBox.LostFocus += (obj2, e2) =>
                        {
                            RichTextBox richTextBox2 = obj2 as RichTextBox;
                            if (richTextBox2 != null)
                            {
                                SetDocumentXaml(richTextBox, XamlWriter.Save(richTextBox.Document));
                            }
                        };
                    }
                }
            );

        #endregion


        public static string GetLocalImagePath(DependencyObject obj)
        {
            return (string)obj.GetValue(LocalImagePathProperty);
        }
        public static void SetLocalImagePath(DependencyObject obj, string value)
        {
            obj.SetValue(LocalImagePathProperty, value);
        }

        public static readonly DependencyProperty LocalImagePathProperty =
            DependencyProperty.Register("LocalImagePath", typeof(string), typeof(RichTextBoxHelper), new PropertyMetadata(string.Empty));


    }
}
