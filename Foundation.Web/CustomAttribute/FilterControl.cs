using System;

namespace Foundation.Web.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterControl : Attribute
    {
        /// <summary>
        ///     Whether applying the filter should be case sensitive.
        /// </summary>
        public bool CaseSensitive;

        /// <summary>
        ///     The associated data element.
        /// </summary>
        public string DataElement;

        /// <summary>
        ///     The operator to apply when filtering
        /// </summary>
        public Operator OperatorOption;
    }
}