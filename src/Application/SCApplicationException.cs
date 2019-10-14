namespace SprintCrowd.BackEnd.Application
{
  using System;
  using SprintCrowd.BackEnd.Application;

  /// <summary>
  /// Custom exception class for use application wide.
  /// </summary>
  [Serializable]
  public class SCApplicationException : Exception
  {

    /// <summary>
    /// constrcutor with message.
    /// <param name="message">exception message</param>
    /// </summary>
    public SCApplicationException(string message) : base(message)
    {
      this.ErrorCode = (int)ApplicationErrorCode.UnknownError;
    }

    /// <summary>
    /// default constrcutor.
    /// </summary>
    public SCApplicationException()
    {

    }

    /// <summary>
    /// Gets application error code.
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// constrcutor with message.
    /// <param name="errorCode">exception message</param>
    /// <param name="message">exception message</param>
    /// </summary>
    public SCApplicationException(int errorCode, string message) : base(message)
    {
      this.ErrorCode = errorCode;
    }

    /// <summary>
    /// constrcutor with message.
    /// <param name="message">exception message</param>
    /// <param name="inner">exception message</param>
    /// </summary>
    public SCApplicationException(string message, System.Exception inner) : base(message, inner) { }

    /// <summary>
    /// constrcutor with message.
    /// <param name="info"></param>
    /// <param name="context"></param>
    /// </summary>
    protected SCApplicationException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}