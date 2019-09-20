using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace TrustedLicenses.Example
{

	class Program
	{

		static void Main()
		{

			Console.Title = Assembly.GetExecutingAssembly().GetName().Name;

			CreateKey();

			// TODO: Add further steps.

			Console.Read();

		}

		// For server/provider use only.
		public static void CreateKey()
		{
			// Create a new DSA CSP instance that stores our key.
			using (DSACryptoServiceProvider dsaKey = new DSACryptoServiceProvider()) // You can also use RSACryptoServiceProvider.
			{
				Console.WriteLine($"Successfully created a {dsaKey.KeySize} bit private key.");

				// Write it to dsa-private.key. The private key always contains the public key.
				File.WriteAllText("dsa-private.key", dsaKey.ToXmlString(true));
			}
		}

		public static AsymmetricAlgorithm LoadKey()
		{
			throw new NotImplementedException();
		}

		// Gets the current machine GUID for this windows installation.
		public static Guid GetMachineGuid()
		{
			const string path = @"SOFTWARE\Microsoft\Cryptography";
			const string name = @"MachineGuid";

			using (RegistryKey key = RegistryKey.OpenBaseKey(
				RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 :
				RegistryView.Registry32)?.OpenSubKey(path, RegistryKeyPermissionCheck.ReadSubTree))
			{
				if (key == null) throw new KeyNotFoundException($"Key '{path}' not found.");
				object result = key.GetValue(name) ?? throw new NullReferenceException(
					$"Key value of '{Path.Combine(path, name)}' not found.");
				if (!Guid.TryParse(result.ToString(), out Guid guid)) throw new InvalidDataException(
					$"Key value of '{Path.Combine(path, name)}' is not a valid {nameof(Guid)}.");
				return guid;
			}
		}

	}

}