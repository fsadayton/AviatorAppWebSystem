// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnsureOneElementAttribute.cs" company="UDRI">
//   
// </copyright>
// <summary>
//   The ensure one element attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Helpers
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The ensure one element attribute.
    /// </summary>
    public class EnsureOneElementAttribute : ValidationAttribute
    {
        /// <summary>
        /// Checks if a list in a model has at least one item in it.
        /// </summary>
        /// <param name="value"> The value to check. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 0;
            }

            return false;
        }
    }
}