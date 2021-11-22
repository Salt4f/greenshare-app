﻿using System;

namespace greenshare_app.Exceptions
{
    [Serializable]
    public class InvalidLoginException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginException"/> class
        /// </summary>
        public InvalidLoginException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public InvalidLoginException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public InvalidLoginException(string message, System.Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected InvalidLoginException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
