using System.Diagnostics;

namespace System.Security.Licensing
{

	/// <summary>
	/// Represents a custom constructor for the <see cref="License{T}"/> class.
	/// </summary>
	/// <typeparam name="T">The type of data embedded in the <see cref="License{T}"/>.</typeparam>
	public class LicenseBuilder<T>
	{

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly License<T> License;

		/// <summary>
		/// Creates a new instance of the <see cref="LicenseBuilder{T}"/> class using a <see cref="License{T}"/>.
		/// </summary>
		/// <param name="license">An instance of the <see cref="License{T}"/> class.</param>
		public LicenseBuilder(License<T> license) => License = license;

		/// <summary>
		/// Sets the validity period for the <see cref="License{T}"/>.
		/// </summary>
		/// <param name="lifespan">The lifespan of the validity for this <see cref="License{T}"/>, starting from now.</param>
		/// <returns><see cref="LicenseBuilder{T}"/></returns>
		public LicenseBuilder<T> ValidityTime(TimeSpan lifespan) => ValidityTime(DateTime.Now, DateTime.Now + lifespan);

		/// <summary>
		/// Sets the validity period for the <see cref="License{T}"/>.
		/// </summary>
		/// <param name="validity">The start time for the validity of the <see cref="License{T}"/>.</param>
		/// <param name="lifespan">The lifespan of the validity for this <see cref="License{T}"/>, starting from the start time.</param>
		/// <returns><see cref="LicenseBuilder{T}"/></returns>
		public LicenseBuilder<T> ValidityTime(DateTime validity, TimeSpan lifespan) =>
			ValidityTime(validity, DateTime.Now + lifespan);

		/// <summary>
		/// Sets the validity period for the <see cref="License{T}"/>.
		/// </summary>
		/// <param name="validity">The start time for the validity of the <see cref="License{T}"/>.</param>
		/// <param name="expiry">The expiration time for the validity of the <see cref="License{T}"/>.</param>
		/// <returns><see cref="LicenseBuilder{T}"/></returns>
		public LicenseBuilder<T> ValidityTime(DateTime validity, DateTime expiry)
		{
			License.Validity = validity;
			License.Expiry = expiry;
			return this;
		}

		/// <summary>
		/// Sets the module version for the validity of the <see cref="License{T}"/>.
		/// </summary>
		/// <param name="version">The module version.</param>
		/// <returns><see cref="LicenseBuilder{T}"/></returns>
		public LicenseBuilder<T> ForVersion(ModuleVersion version) => ForVersion(version, version);

		/// <summary>
		/// Sets the module version for the validity of the <see cref="License{T}"/>.
		/// </summary>
		/// <param name="minVersion">The minimum module version.</param>
		/// <param name="maxVersion">The maximum module version.</param>
		/// <returns><see cref="LicenseBuilder{T}"/></returns>
		public LicenseBuilder<T> ForVersion(ModuleVersion minVersion, ModuleVersion maxVersion)
		{
			License.MinimumVersion = minVersion;
			License.MaximumVersion = maxVersion;
			return this;
		}

		/// <summary>
		/// Assigns the generic license data object to the <see cref="License{T}"/>.
		/// </summary>
		/// <param name="data">The license data object.</param>
		/// <returns><see cref="LicenseBuilder{T}"/></returns>
		public LicenseBuilder<T> AssignData(T data)
		{
			License.DataObject = data;
			return this;
		}

		/// <summary>
		/// Returns the final <see cref="License{T}"/> result instance.
		/// </summary>
		/// <returns><see cref="License{T}"/></returns>
		public License<T> GetResult() => License;

	}

}