using System;

namespace TreeislandStudio.Engine.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
// ReSharper disable once CheckNamespace
    public class ExposeHeader : Attribute {
        /// <summary>
        /// Constructor
        /// </summary>
        public ExposeHeader() 
        {
        }
    }
}
