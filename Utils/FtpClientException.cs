using System;

namespace Network
{
	/// <summary>
	/// General exception class
	/// </summary>
	public sealed class FtpClientException : System.Exception
	{
		int m_iErrorCode = 0;

		/// <summary>
		/// An instance of FtpClientException
		/// </summary>
		public FtpClientException() : base() { }
		/// <summary>
		/// An instance of FtpClientException
		/// </summary>
		/// <param name="code">Error code of this exception</param>
		/// <param name="message">Explains what happend</param>
		public FtpClientException(int code, string message)  { m_iErrorCode = code; throw this; }
		/// <summary>
		/// Error code. This property is read-only.
		/// </summary>
		public int ErrorCode
		{
			get
			{
				return m_iErrorCode;
			}
		}
	}
}
