using System;

namespace greenshare_app.Exceptions
{
    [Serializable]
    public class NullOfferException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NullOfferException"/> class
        /// </summary>
        public NullOfferException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NullLocationException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public NullOfferException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NullOfferException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public NullOfferException(string message, System.Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NullOfferException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected NullOfferException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
