using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace System.Security.Licensing
{

	/// <summary>
	/// Provides methods for signing and validating xml data.
	/// </summary>
	public static class XmlSigner
	{

		/// <summary>
		/// Signs an <see cref="XmlDocument"/> with the specified private asymmetric key.
		/// </summary>
		/// <param name="doc">The <see cref="XmlDocument"/>.</param>
		/// <param name="key">The private <see cref="RSA"/> key.</param>
		/// <returns><see cref="XmlDocument"/></returns>
		public static XmlDocument SignXmlDocument(XmlDocument doc, RSA key) => SignXmlDocument(doc, (AsymmetricAlgorithm)key);

		/// <summary>
		/// Signs an <see cref="XmlDocument"/> with the specified private asymmetric key.
		/// </summary>
		/// <param name="doc">The <see cref="XmlDocument"/>.</param>
		/// <param name="key">The private <see cref="DSA"/> key.</param>
		/// <returns>see cref="XmlDocument"/></returns>
		public static XmlDocument SignXmlDocument(XmlDocument doc, DSA key) => SignXmlDocument(doc, (AsymmetricAlgorithm)key);

		/// <summary>
		/// Signs an <see cref="XmlDocument"/> with the specified private asymmetric key.
		/// </summary>
		/// <param name="doc">The <see cref="XmlDocument"/>.</param>
		/// <param name="key">The private asymmetric key.</param>
		/// <returns></returns>
		public static XmlDocument SignXmlDocument(XmlDocument doc, AsymmetricAlgorithm key)
		{
			SignedXml signedXml = new SignedXml(doc)
			{
				SigningKey = key
			};

			signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

			XmlDsigExcC14NTransform canMethod = (XmlDsigExcC14NTransform)signedXml.SignedInfo.CanonicalizationMethodObject;
			canMethod.InclusiveNamespacesPrefixList = "Sign";

			Reference reference = new Reference()
			{
				Uri = string.Empty
			};

			reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
			signedXml.AddReference(reference);

			KeyInfo keyInfo = new KeyInfo();

			if (key.GetType().BaseType == typeof(RSA)) keyInfo.AddClause(new RSAKeyValue((RSA)key));
			else if (key.GetType().BaseType == typeof(DSA)) keyInfo.AddClause(new DSAKeyValue((DSA)key));

			signedXml.KeyInfo = keyInfo;

			signedXml.ComputeSignature();

			doc.DocumentElement.AppendChild(doc.ImportNode(signedXml.GetXml(), true));
			if (doc.FirstChild is XmlDeclaration) doc.RemoveChild(doc.FirstChild);

			return doc;
		}

		/// <summary>
		/// Verifies the <see cref="XmlDocument"/> with its signature.
		/// </summary>
		/// <param name="doc">The <see cref="XmlDocument"/>.</param>
		/// <returns><see cref="bool"/></returns>
		public static bool VerifyXmlDocument(XmlDocument doc)
		{
			SignedXml signedXml = new SignedXml(doc);
			signedXml.LoadXml((XmlElement)doc.GetElementsByTagName("Signature")[0]);
			return signedXml.CheckSignature();
		}

	}

}