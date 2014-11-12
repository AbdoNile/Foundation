using System;
using System.IO;
using System.Web.Mvc;

namespace Foundation.Web.Extensions
{
    public static class ButtonGroupExtension
    {
        public static IDisposable ButtonGroup(this HtmlHelper htmlHelper, string dropDownButtonText = "Actions",
            SlideDirection direction = SlideDirection.Down)
        {
            TextWriter writer = htmlHelper.ViewContext.Writer;
            return new ButtonGroupContainer(writer, dropDownButtonText, direction);
        }

        private class ButtonGroupContainer : IDisposable
        {
            private readonly TextWriter writer;

            public ButtonGroupContainer(TextWriter contextTextWriter, string dropDownButtonText,
                SlideDirection direction)
            {
                writer = contextTextWriter;
                string starterTemplate =
                    "<div class=\"btn-group\"><a class=\"btn btn-primary btn-group-xs dropdown-toggle\" data-toggle=\"dropdown\">{0}<span class=\"caret\"></span></a><ul class=\"dropdown-menu\">";
                starterTemplate = string.Format(starterTemplate, dropDownButtonText);

                writer.WriteLine(starterTemplate);
            }

            public void Dispose()
            {
                writer.WriteLine("</ul></div>");
            }
        }
    }
}