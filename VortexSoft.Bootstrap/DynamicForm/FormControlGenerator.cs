﻿using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using JQueryUIHelpers;
using VortexSoft.Bootstrap.Extensions;

namespace VortexSoft.Bootstrap
{
    public class FormControlGenerator<TModel>
    {
        private readonly HtmlHelper<TModel> htmlHelper;

        public FormControlGenerator(HtmlHelper<TModel> htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public virtual void RenderBoolean(NavHtmlTextWritter writer, PropertyInfo property, object value)
        {
            if (Convert.ToBoolean(value))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, property.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, property.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "true");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // input

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, property.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "false");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // input
        }

        public virtual void RenderDateTime(NavHtmlTextWritter writer, PropertyInfo property, object value, bool isRequired)
        {
            DateTime? dateTimeValue =  Convert.ToDateTime(value);
            var datePicker = htmlHelper.JQueryUI().Datepicker(property.Name, dateTimeValue.Value).ChangeYear(true).ChangeMonth(true);
            writer.ClearAttributes();
            writer.Write(datePicker.ToHtmlString());
        }

        public virtual void RenderWholeNumber(NavHtmlTextWritter writer, PropertyInfo property, object value, bool isRequired)
        {
            if (isRequired)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "validate[required,custom[integer]]");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "validate[custom[integer]]");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Value, Convert.ToString(value));
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, property.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, property.Name);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // input
        }

        public virtual void RenderPassword(NavHtmlTextWritter writer, FormElement formElement, object value, bool isRequired)
        {
            if (isRequired)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "validate[required]");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "password");
            if (formElement.ControlSpecs.MaxLength != 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, formElement.ControlSpecs.MaxLength.ToString(CultureInfo.InvariantCulture));
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Name, formElement.PropertyInfo.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, formElement.PropertyInfo.Name);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // </input>
        }

        public virtual void RenderTextBox(NavHtmlTextWritter writer, FormElement formElement, object value, bool isRequired)
        {
            if (isRequired)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "validate[required]");
            }
            
            writer.AddAttribute(HtmlTextWriterAttribute.Value, Convert.ToString(value));
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            
            if (formElement.ControlSpecs.MaxLength != 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, formElement.ControlSpecs.MaxLength.ToString(CultureInfo.InvariantCulture));
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Name, formElement.PropertyInfo.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, formElement.PropertyInfo.Name);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // </input>
        }

        public virtual void RenderHidden(NavHtmlTextWritter writer, FormElement formElement, object value, bool isRequired)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, formElement.PropertyInfo.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, formElement.PropertyInfo.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            var hiddenValue = (value == null) ? string.Empty : value.ToString();
            writer.AddAttribute(HtmlTextWriterAttribute.Value, hiddenValue);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public virtual void RenderTextArea(NavHtmlTextWritter writer, FormElement formElement, object value, bool isRequired)
        {
            if (isRequired)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "validate[required]");
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Name, formElement.PropertyInfo.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, formElement.PropertyInfo.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Cols, formElement.ControlSpecs.Cols.ToString(CultureInfo.InvariantCulture));
            writer.AddAttribute(HtmlTextWriterAttribute.Rows, formElement.ControlSpecs.Rows.ToString(CultureInfo.InvariantCulture));
            writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
            writer.Write(value);
            writer.RenderEndTag(); // </textarea>
        }

        public virtual void RenderFloatingPointNumber(NavHtmlTextWritter writer, PropertyInfo property, object value, bool isRequired)
        {
            if (isRequired)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "validate[required,custom[number]]");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "validate[custom[number]]");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Value, Convert.ToString(value));
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, property.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, property.Name);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // input
        }

        public virtual void RenderEnum(NavHtmlTextWritter writer, PropertyInfo property, object value, bool isRequired)
        {
            var dropDownList = new DropDownList();
            dropDownList.ID = property.Name;

            foreach (var fieldInfo in property.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static).OrderBy(x => x.Name))
            {
                var item = new ListItem(fieldInfo.Name.SpacePascal(), fieldInfo.GetRawConstantValue().ToString());
                dropDownList.Items.Add(item);
            }

            if (isRequired)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "validate[required]");
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Name, property.Name);
            dropDownList.RenderControl(writer);
        }

        public virtual void RenderStaticText(NavHtmlTextWritter writer, PropertyInfo property, object value, bool isRequired)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, property.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, property.Name);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "form-control-static");
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write(Convert.ToString(value));
            writer.RenderEndTag(); // input
        }
    }
}