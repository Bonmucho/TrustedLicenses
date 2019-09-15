namespace System.Security.Licensing
{

	/// <summary>
	/// Specifies the reason for the validation failure.
	/// </summary>
	public enum ValidationFault
	{

		/// <summary>
		/// Generic reason. This is generated in a generic part of the validation chain.
		/// </summary>
		Generic,

		/// <summary>
		/// The signature of the license is invalid.
		/// </summary>
		InvalidSignature,

		/// <summary>
		/// The license is expired.
		/// </summary>
		Expired,

		/// <summary>
		/// The license is invalid for the specified version.
		/// </summary>
		IncorrectVersion

	}

}