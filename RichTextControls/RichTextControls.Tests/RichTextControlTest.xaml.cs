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
        public static string[] FontNames = {
    "Arial", "Calibri", "Cambria", "Cambria Math", "Comic Sans MS", "Courier New",
    "Ebrima", "Gadugi", "Georgia",
    "Javanese Text Regular Fallback font for Javanese script", "Leelawadee UI",
    "Lucida Console", "Malgun Gothic", "Microsoft Himalaya", "Microsoft JhengHei",
    "Microsoft JhengHei UI", "Microsoft New Tai Lue", "Microsoft PhagsPa",
    "Microsoft Tai Le", "Microsoft YaHei", "Microsoft YaHei UI",
    "Microsoft Yi Baiti", "Mongolian Baiti", "MV Boli", "Myanmar Text",
    "Nirmala UI", "Segoe MDL2 Assets", "Segoe Print", "Segoe UI", "Segoe UI Emoji",
    "Segoe UI Historic", "Segoe UI Symbol", "SimSun", "Times New Roman",
    "Trebuchet MS", "Verdana", "Webdings", "Wingdings", "Yu Gothic",
    "Yu Gothic UI"
};
        public RichTextControlTest()
        {
            this.InitializeComponent();
            foreach(var font in FontNames)
            {
                var menuFlyoutItem = new MenuFlyoutItem() { Text = font };
                menuFlyoutItem.Click += MenuFlyoutItem_Click;
                FontFlyout.Items.Add(menuFlyoutItem);
            }

            LoadRichTextControlintoRichTextBlock();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            htmlTextBlock.FontFamily = new FontFamily(((MenuFlyoutItem)sender).Text);
        }

        string Html = "";

        string ReaderHtml { get { return Html; } }
        public void LoadRichTextControlintoRichTextBlock()
        {
            HtmlTextBlock htmlTextBlock = new HtmlTextBlock
            {
                Html = Html
            };
            htmlTest = htmlTextBlock;
        }

        HtmlTextBlock htmlTextBlock;
        private void ShowHtml()
        {
            //TestGrid.Children.Remove(htmlTest);
            TestGrid.Children.Clear();
            //Html2RichTextBlock html2RichTextBlock = new Html2RichTextBlock(Html);
            //RichTextBlock richTextBlock = html2RichTextBlock.ConvertToRichTextBlock();
            htmlTextBlock = new HtmlTextBlock { Html = Html };
            TestGrid.Children.Add(htmlTextBlock);
        }

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
            Invoke(() => { ShowHtml(); });
        }

        private async void SmartReaderButton_Click(object sender, RoutedEventArgs e)
        {
            var url = UrlBox.Text;
            var article = await SmartReader.Reader.ParseArticleAsync(url);
            Html = article.Content;
            //Convert(Html);
            Invoke(() => { ShowHtml(); });
        }

        public async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });
        }

        private void ConverHTML_Click(object sender, RoutedEventArgs e)
        {
            Html = UrlBox.Text;
            Invoke(() => { ShowHtml(); });
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            TestGrid.Width = ReaderWidthSlider.Value * 8 + 500;
        }

        private void FontSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            htmlTextBlock.FontSize = (FontSizeSlider.Value + 10) / 2;
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
