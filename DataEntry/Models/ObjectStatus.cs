// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectStatus.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The object state.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Models
{
    /// <summary>
    /// The object state.
    /// </summary>
    public class ObjectStatus
    {
        /// <summary>
        /// Flags for what state the object is in.
        /// </summary>
        public enum ObjectState
        {
            /// <summary>
            /// Object needs to be created in database.
            /// </summary>
            Create,

            /// <summary>
            /// Default. Used for View data.
            /// </summary>
            Read,

            /// <summary>
            /// Object needs to be updated in database.
            /// </summary>
            Update,

            /// <summary>
            /// Object needs to be deleted in database.
            /// </summary>
            Delete,

            /// <summary>
            /// Used for testing.
            /// </summary>
            Test
        }
    }
}
