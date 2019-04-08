using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RichTextControls;
using RichTextControls.RichTextBlockConverter;
using System.Text;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace RichTextControls.Tests
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RichTextControlTest : Page
    {
        string[] FontNames = Microsoft.Graphics.Canvas.Text.CanvasTextFormat.GetSystemFontFamilies();
        public RichTextControlTest()
        {
            this.InitializeComponent();
            foreach (var font in FontNames)
            {
                var menuFlyoutItem = new MenuFlyoutItem() { Text = font };
                menuFlyoutItem.Click += MenuFlyoutItem_Click;
                FontFlyout.Items.Add(menuFlyoutItem);
            }

            //LoadRichTextControlintoRichTextBlock();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            htmlTest.FontFamily = new FontFamily(((MenuFlyoutItem)sender).Text);
        }

        string Html = "";

        string ReaderHtml { get { return Html; } }

        private async void ReaderSharpButton_Click(object sender, RoutedEventArgs e)
        {
            var url = UrlBox.Text;
            ReadSharp.Reader reader = new ReadSharp.Reader();
            try
            {
                var readerSharpArticle = await reader.Read(new Uri(url));
                Html = readerSharpArticle.Content;
            }
            catch (UriFormatException exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
            CodeBlock.Code = Html;
            //Convert(Html);
            //Invoke(() => { ShowHtml(); });
            Bindings.Update();
        }

        private async void SmartReaderButton_Click(object sender, RoutedEventArgs e)
        {
            var url = UrlBox.Text;
            var article = await SmartReader.Reader.ParseArticleAsync(url);
            Html = article.Content;
            //Convert(Html);
            //Invoke(() => { ShowHtml(); });
        }

        public async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });
        }

        private void ConverHTML_Click(object sender, RoutedEventArgs e)
        {
            Html = UrlBox.Text;
            //Invoke(() => { ShowHtml(); });
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            TestGrid.Width = ReaderWidthSlider.Value * 8 + 500;
        }

        private void FontSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            htmlTest.FontSize = (FontSizeSlider.Value + 10) / 2;
        }

        //public string Convert(string Html)
        //{
        //    var sb = new StringBuilder();
        //    var stringWriter = new StringWriter(sb);
        //    var doc = new HtmlDocument
        //    {
        //        OptionOutputAsXml = true,
        //        OptionCheckSyntax = true,
        //        OptionFixNestedTags = true,
        //        OptionAutoCloseOnEnd = true,
        //        OptionDefaultStreamEncoding = Encoding.UTF8
        //    };
        //    doc.LoadHtml(Html);
        //    MemoryStream stream = new MemoryStream();
        //    doc.Save(stringWriter);
        //    return sb.ToString();
        //}
    }
}
