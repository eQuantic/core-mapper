﻿using System.Runtime.Serialization;

namespace eQuantic.Mapper.Exceptions;

/// <summary>
/// Exception thrown when a requested mapper is not found.
/// </summary>
[Serializable]
public class MapperNotFoundException: Exception
{
    /// <summary>
    /// Initializes a new instance of the MapperNotFoundException class.
    /// </summary>
    public MapperNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the MapperNotFoundException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public MapperNotFoundException(string message) 
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the MapperNotFoundException class with a specified error message and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public MapperNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the MapperNotFoundException class with serialized data.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
    public MapperNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
}