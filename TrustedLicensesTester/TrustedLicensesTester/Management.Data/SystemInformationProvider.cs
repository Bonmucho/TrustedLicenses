using System.Collections.Generic;
using System.Management.Data.Models;
using OS = System.Management.Data.Models.OperatingSystem;
using CPU = System.Management.Data.Models.Processor;
using Microsoft.Win32;
using System.IO;

namespace System.Management.Data
{

	public static class SystemInformationProvider
	{

		public static IEnumerable<OS> GetOperatingSystems() => new WqlDataContext<OS>();
		public static IEnumerable<CPU> GetProcessors() => new WqlDataContext<CPU>();
		public static IEnumerable<CacheMemory> GetCaches() => new WqlDataContext<CacheMemory>();

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