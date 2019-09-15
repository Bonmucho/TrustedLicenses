namespace System.Security.Licensing
{

	/// <summary>
	/// The exception that is thrown when a license validation failed.
	/// </summary>
	public class LicenseValidationException : Exception
	{

		/// <summary>
		/// Gets the reason for the validation failure.
		/// </summary>
		public ValidationFault Fault { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LicenseValidationException"/> class with a <see cref="ValidationFault"/>.
		/// </summary>
		/// <param name="fault">The <see cref="ValidationFault"/>.</param>
		public LicenseValidationException(ValidationFault fault) => Fault = fault;

		/// <summary>
		/// Initializes a new instance of the <see cref="LicenseValidationException"/> class with a specified error message and a <see cref="ValidationFault"/>.
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="fault">The <see cref="ValidationFault"/>.</param>
		public LicenseValidationException(string message, ValidationFault fault) : base(message) => Fault = fault;

		/// <summary>
		/// Initializes a new instance of the <see cref="LicenseValidationException"/> class with a specified error message, a reference to the inner exception that is the cause of this exception and a <see cref="ValidationFault"/>.
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="innerException">The inner exception.</param>
		/// <param name="fault">The <see cref="ValidationFault"/>.</param>
		public LicenseValidationException(string message, Exception innerException, ValidationFault fault) :
			base(message, innerException) => Fault = fault;

	}

}