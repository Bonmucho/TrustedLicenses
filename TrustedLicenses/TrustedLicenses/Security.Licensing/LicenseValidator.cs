using System.Diagnostics;
using System.Xml;

namespace System.Security.Licensing
{

	/// <summary>
	/// Provides methods for custom execution of a validation chain for the <see cref="License{T}"/> class.
	/// </summary>
	/// <typeparam name="T">The type of data embedded in the <see cref="License{T}"/>.</typeparam>
	public class LicenseValidator<T>
	{

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly License<T> License;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly XmlDocument Document;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool Result;

		/// <summary>
		/// Creates a new instance of the <see cref="LicenseValidator{T}"/> class using a <see cref="License{T}"/> and an <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="license">An instance of the <see cref="License{T}"/> class.</param>
		/// <param name="document">The corresponding <see cref="XmlDocument"/>.</param>
		public LicenseValidator(License<T> license, XmlDocument document)
		{
			License = license;
			Document = document;
			Result = true;		
		}

		/// <summary>
		/// Validates the underlying <see cref="License{T}"/> based on its validation period.
		/// </summary>
		/// <returns><see cref="LicenseValidator{T}"/></returns>
		public LicenseValidator<T> ForValidityTime() => ForValidityTime(DateTime.Now);

		/// <summary>
		/// Validates the underlying <see cref="License{T}"/> based on its validation period using a specified time.
		/// </summary>
		/// <param name="dateTime">The specified time.</param>
		/// <returns><see cref="LicenseValidator{T}"/></returns>
		public LicenseValidator<T> ForValidityTime(DateTime dateTime)
		{
			Result = Result && License.Validity <= dateTime && License.Expiry > dateTime;
			if (License<T>.ThrowExcpetionsOnValidation && !Result)
				throw new LicenseValidationException("License validation failed. The specified license is expired.",
					ValidationFault.Expired);
			return this;
		}

		/// <summary>
		/// Validates the underlying <see cref="License{T}"/> based on its module version using a specified module version.
		/// </summary>
		/// <param name="version">The specified module version.</param>
		/// <returns><see cref="LicenseValidator{T}"/></returns>
		public LicenseValidator<T> ForVersion(ModuleVersion version)
		{
			Result = Result && License.MinimumVersion <= version && License.MaximumVersion >= version;
			if (License<T>.ThrowExcpetionsOnValidation && !Result)
				throw new LicenseValidationException(
					"License validation failed. The specified license was not issued for this release.",
					ValidationFault.IncorrectVersion);
			return this;
		}

		/// <summary>
		/// Validates the underlying <see cref="License{T}"/> by means of a generic validation function.
		/// </summary>
		/// <param name="func">The generic validation function.</param>
		/// <returns><see cref="LicenseValidator{T}"/></returns>
		public LicenseValidator<T> For(Func<T, bool> func) => For(func, nameof(ValidationFault.Generic));

		/// <summary>
		/// Validates the underlying <see cref="License{T}"/> by means of a generic validation function.
		/// </summary>
		/// <param name="func">The generic validation function.</param>
		/// <param name="stepName">A name for this validation step.</param>
		/// <returns><see cref="LicenseValidator{T}"/></returns>
		public LicenseValidator<T> For(Func<T, bool> func, string stepName)
		{
			Result = Result && func.Invoke(License.DataObject);
			if (License<T>.ThrowExcpetionsOnValidation && !Result)
				throw new LicenseValidationException(
					$"License validation failed. The specified license has been invalidated by generic validation (validation step: '{stepName}').",
					ValidationFault.Generic);
			return this;
		}

		/// <summary>
		/// Returns the result of the validation chain.
		/// </summary>
		/// <returns><see cref="bool"/></returns>
		public bool GetResult() => Result;

		/// <summary>
		/// Verifies the underlying <see cref="License{T}"/> with its signature and returns the result of the validation chain.
		/// </summary>
		/// <returns><see cref="bool"/></returns>
		public bool Verify()
		{
			Result = Result && XmlSigner.VerifyXmlDocument(Document);
			if (License<T>.ThrowExcpetionsOnValidation && !Result)
				throw new LicenseValidationException(
					"License validation failed. The specified license does not have a valid signature.",
					ValidationFault.InvalidSignature);
			return Result;
		}

	}

}