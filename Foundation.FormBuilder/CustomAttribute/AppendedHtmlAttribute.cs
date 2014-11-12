using System.Web.Mvc;

namespace Foundation.FormBuilder.CustomAttribute
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class AppendedHtmlAttribute : System.Attribute
    {
        /// <summary>
        /// Attribute Name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Attribute Value.
        /// </summary>
        public string Value;
    }
}
