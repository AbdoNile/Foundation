using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Foundation.Web.Extensions
{
    public class NavHtmlTextWritter : HtmlTextWriter
    {
        private readonly HtmlTextWriterAttribute[] multiValueAttrs = {HtmlTextWriterAttribute.Class};

        private Dictionary<HtmlTextWriterAttribute, List<string>> attrValues =
            new Dictionary<HtmlTextWriterAttribute, List<string>>();

        public NavHtmlTextWritter(TextWriter writer) : base(writer)
        {
        }

        public override void AddAttribute(HtmlTextWriterAttribute key, string value)
        {
            if (multiValueAttrs != null && multiValueAttrs.Contains(key))
            {
                if (!attrValues.ContainsKey(key))
                    attrValues.Add(key, new List<string>());

                attrValues[key].Add(value);
            }
            else
            {
                base.AddAttribute(key, value);
            }
        }

        public override void RenderBeginTag(HtmlTextWriterTag tagKey)
        {
            AddMultiValuesAttrs();
            base.RenderBeginTag(tagKey);
            ClearAttributes();
        }

        public override void RenderBeginTag(string tagName)
        {
            AddMultiValuesAttrs();
            base.RenderBeginTag(tagName);
            ClearAttributes();
        }

        private void AddMultiValuesAttrs()
        {
            foreach (HtmlTextWriterAttribute key in attrValues.Keys)
                AddAttribute(key.ToString(), string.Join(" ", attrValues[key].ToArray()));

            attrValues = new Dictionary<HtmlTextWriterAttribute, List<string>>();
        }

        public void ClearAttributes()
        {
            attrValues.Clear();
        }
    }
}