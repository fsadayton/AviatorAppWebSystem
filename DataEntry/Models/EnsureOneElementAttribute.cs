// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnsureOneElementAttribute.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The ensure one element attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Models
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The ensure one element attribute.
    /// </summary>
    public class EnsureOneElementAttribute : ValidationAttribute
    {
        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
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