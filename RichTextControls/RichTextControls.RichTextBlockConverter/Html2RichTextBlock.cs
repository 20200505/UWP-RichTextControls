using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RichTextControls.Generators;
using RichTextControls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI;

namespace RichTextControls.RichTextBlockConverter
{
    public class Html2RichTextBlock
    {
        private StackPanel uiElement;
        public Html2RichTextBlock(string Html)
        {
            HtmlXamlGenerator htmlXamlGenerator = new HtmlXamlGenerator(Html);
            uiElement = htmlXamlGenerator.Generate() as StackPanel;
        }

        public RichTextBlock ConvertToRichTextBlock()
        {
            RecursiveConvert(uiElement);
            return richTextBlock;
        }

        private RichTextBlock richTextBlock = new RichTextBlock();

        private void RecursiveConvert(StackPanel stackElement)
        {
            foreach(var element in stackElement.Children)
            {
                if(element is RichTextBlock)
                {
                    foreach(var block in (element as RichTextBlock).Blocks)
                    {
                        (element as RichTextBlock).Blocks.Remove(block);
                        richTextBlock.Blocks.Add(block as Paragraph);
                    }
                }
                else if(element is StackPanel)
                {
                    RecursiveConvert(element as StackPanel);
                }
                else if(element is Border)
                {
                    Paragraph paragraph = new Paragraph();
                    stackElement.Children.Remove(element);
                    //Border border = element as Border;
                    //Color color = Colors.LightGray;

                    //border.Background = new Windows.UI.Xaml.Media.SolidColorBrush(color);
                    InlineUIContainer inlineUIContainer = new InlineUIContainer
                    {
                        Child = element
                    };
                    paragraph.Inlines.Add(inlineUIContainer);
                    richTextBlock.Blocks.Add(paragraph);
                }
                else if(element is TextBlock)
                {
                    Paragraph paragraph = new Paragraph();
                    foreach(var inline in (element as TextBlock).Inlines)
                    {
                        (element as TextBlock).Inlines.Remove(inline);
                        paragraph.Inlines.Add(inline);
                    }
                    richTextBlock.Blocks.Add(paragraph);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(element.GetType().ToString());
                }
            }
        }

        
    }
}
