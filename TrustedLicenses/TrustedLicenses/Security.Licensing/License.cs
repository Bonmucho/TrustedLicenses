using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace System.Security.Licensing
{

	/// <summary>
	/// Represents a <see cref="License{T}"/> and provides functions for signing and validating license data.
	/// </summary>
	/// <typeparam name="T">The type of data embedded in the <see cref="License{T}"/>.</typeparam>
	[Serializable, XmlType("License")]
	public class License<T> : ISerializable
	{

		/// <summary>
		/// Gets or sets whether exceptions are generated when license validation fails.
		/// </summary>
		public static bool ThrowExcpetionsOnValidation { get; set; } = false;

		/// <summary>
		/// Gets or sets the unique identifier for this <see cref="License{T}"/> instance.
		/// </summary>
		public Guid UniqueIdentifier { get; set; }

		/// <summary>
		/// Gets or sets the start time for the validity of the <see cref="License{T}"/>.
		/// </summary>
		public DateTime Validity { get; set; }

		/// <summary>
		/// Gets or sets the expiration time for the validity of the <see cref="License{T}"/>.
		/// </summary>
		public DateTime Expiry { get; set; }

		/// <summary>
		/// Gets or sets the minimum module version for the validity of the <see cref="License{T}"/>.
		/// </summary>
		public ModuleVersion MinimumVersion { get; set; }

		/// <summary>
		/// Gets or sets the maximum module version for the validity of the <see cref="License{T}"/>.
		/// </summary>
		public ModuleVersion MaximumVersion { get; set; }

		/// <summary>
		/// Gets or sets the generic license data object.
		/// </summary>
		public T DataObject { get; set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private XmlDocument LicenseCache;

		/// <summary>
		/// Creates a new instance of the <see cref="License{T}"/> class.
		/// </summary>
		public License() => UniqueIdentifier = Guid.NewGuid();

		/// <summary>
		/// Creates a new instance of the <see cref="License{T}"/> class using a unique identifier.
		/// </summary>
		/// <param name="guid">The unique identifier.</param>
		public License(Guid guid) => UniqueIdentifier = guid;

		/// <summary>
		/// Creates a new instance of the <see cref="License{T}"/> class.
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> object.</param>
		/// <param name="context">A <see cref="StreamingContext"/> object.</param>
		public License(SerializationInfo info, StreamingContext context)
		{
			UniqueIdentifier = (Guid)info.GetValue(nameof(UniqueIdentifier), typeof(Guid));
			Validity = (DateTime)info.GetValue(nameof(Validity), typeof(DateTime));
			Expiry = (DateTime)info.GetValue(nameof(Expiry), typeof(DateTime));
			MinimumVersion = (ModuleVersion)info.GetValue(nameof(MinimumVersion), typeof(ModuleVersion));
			MaximumVersion = (ModuleVersion)info.GetValue(nameof(MaximumVersion), typeof(ModuleVersion));
			DataObject = (T)info.GetValue(nameof(DataObject), typeof(T));
		}

		/// <summary>
		/// Gets object data for serialization purposes.
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> object.</param>
		/// <param name="context">A <see cref="StreamingContext"/> object.</param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(UniqueIdentifier), UniqueIdentifier);
			info.AddValue(nameof(Validity), Validity);
			info.AddValue(nameof(Expiry), Expiry);
			info.AddValue(nameof(MinimumVersion), MinimumVersion);
			info.AddValue(nameof(MaximumVersion), MaximumVersion);
			info.AddValue(nameof(DataObject), DataObject);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="License{T}"/> class using the <see cref="LicenseBuilder{T}"/>.
		/// </summary>
		/// <returns><see cref="LicenseBuilder{T}"/></returns>
		public static LicenseBuilder<T> Create() => new LicenseBuilder<T>(new License<T>());

		/// <summary>
		/// Creates a new instance of the <see cref="License{T}"/> class using the <see cref="LicenseBuilder{T}"/>.
		/// </summary>
		/// <param name="guid">The unique identifier.</param>
		/// <returns><see cref="LicenseBuilder{T}"/></returns>
		public static LicenseBuilder<T> Create(Guid guid) => new LicenseBuilder<T>(new License<T>(guid));

		/// <summary>
		/// Signs this instance of the <see cref="License{T}"/> class using a private asymmetric key and returns the result as an <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="key">The private <see cref="RSA"/> key.</param>
		/// <returns><see cref="XmlDocument"/></returns>
		public XmlDocument Sign(RSA key) => Sign((AsymmetricAlgorithm)key);

		/// <summary>
		/// Signs this instance of the <see cref="License{T}"/> class using a private asymmetric key and returns the result as an <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="key">The private <see cref="DSA"/> key.</param>
		/// <returns><see cref="XmlDocument"/></returns>
		public XmlDocument Sign(DSA key) => Sign((AsymmetricAlgorithm)key);

		/// <summary>
		/// Signs this instance of the <see cref="License{T}"/> class using a private asymmetric key and returns the result as an <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="key">The private asymmetric key.</param>
		/// <returns><see cref="XmlDocument"/></returns>
		public XmlDocument Sign(AsymmetricAlgorithm key)
		{
			LicenseCache = XmlSigner.SignXmlDocument(ToXmlDocument(), key);
			return (XmlDocument)LicenseCache.Clone();
		}

		/// <summary>
		/// Initiates a validation chain against a signed <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="document">The signed <see cref="XmlDocument"/>.</param>
		/// <param name="license">Returns a deserialized <see cref="License{T}"/> instance.</param>
		/// <returns><see cref="LicenseValidator{T}"/></returns>
		public static LicenseValidator<T> ValidateAgainst(XmlDocument document, out License<T> license)
		{
			license = FromXmlDocument(document);
			license.LicenseCache = document;
			return new LicenseValidator<T>(license, document);
		}

		/// <summary>
		/// Initiates a validation chain for this instance.
		/// </summary>
		/// <returns><see cref="LicenseValidator{T}"/></returns>
		public LicenseValidator<T> ValidateFor() => new LicenseValidator<T>(this, LicenseCache);

		/// <summary>
		/// Reconstructs a <see cref="License{T}"/> object from an <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="document">The <see cref="XmlDocument"/> used to restore the license object.</param>
		/// <returns><see cref="License{T}"/></returns>
		public static License<T> FromXmlDocument(XmlDocument document)
		{
			using (MemoryStream buffer = new MemoryStream())
			{
				document.Save(buffer);
				buffer.Seek(0, SeekOrigin.Begin);

				License<T> license = LoadXml(buffer);
				license.LicenseCache = (XmlDocument)document.Clone();
				return license;
			}
		}

		/// <summary>
		/// Reconstructs a <see cref="License{T}"/> object from an xml-string.
		/// </summary>
		/// <param name="xml">The xml string used to restore the license object.</param>
		/// <returns><see cref="License{T}"/></returns>
		public static License<T> FromXmlString(string xml)
		{
			using (MemoryStream buffer = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
			{
				License<T> license = LoadXml(buffer);
				XmlDocument xmlLicense = new XmlDocument();

				xmlLicense.LoadXml(xml);
				license.LicenseCache = xmlLicense;
				return license;
			}
				
		}

		private static License<T> LoadXml(Stream stream) =>
			(License<T>)new XmlSerializer(typeof(License<T>)).Deserialize(stream);

		/// <summary>
		/// Creates and returns an <see cref="XmlDocument"/> of the current <see cref="License{T}"/> object.
		/// </summary>
		/// <returns><see cref="License{T}"/></returns>
		public XmlDocument ToXmlDocument()
		{
			using (MemoryStream buffer = new MemoryStream())
			{
				SaveXml(buffer);
				buffer.Seek(0, SeekOrigin.Begin);

				XmlDocument document = new XmlDocument();
				document.Load(buffer);

				return document;
			}
		}

		/// <summary>
		/// Creates and returns an xml representation of the current <see cref="License{T}"/> object.
		/// </summary>
		/// <returns><see cref="License{T}"/></returns>
		public string ToXmlString()
		{
			using (MemoryStream buffer = new MemoryStream())
			{
				SaveXml(buffer);
				return Encoding.UTF8.GetString(buffer.ToArray());
			}
		}

		private void SaveXml(Stream stream) =>
			new XmlSerializer(typeof(License<T>)).Serialize(stream, this);

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns><see cref="string"/></returns>
		public override string ToString() => UniqueIdentifier.ToString();
	
	}

}