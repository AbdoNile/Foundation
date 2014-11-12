using System;
using System.IO;
using System.Web.Mvc;

namespace Foundation.Web.Extensions
{
    public class MvcPanel : IDisposable
    {
        private readonly HtmlHelper htmlHelper;
        private bool disposed;

        public MvcPanel(HtmlHelper htmlHelper)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            this.htmlHelper = htmlHelper;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static void EndPanel(HtmlHelper htmlHelper)
        {
            TextWriter writer = htmlHelper.ViewContext.Writer;

            writer.Write("</div></div>");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                EndPanel(htmlHelper);
            }
        }
    }
}