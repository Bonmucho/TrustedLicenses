using System.Data.Linq.Mapping;

namespace System.Management.Data.Models
{

	[Table(Name = "Win32_OperatingSystem")]
	public class OperatingSystem
	{

		[Column(Name = "BuildNumber")] public int BuildNumber { get; set; }
		[Column(Name = "Caption")] public string Caption { get; set; }
		[Column(Name = "CodeSet")] public int CodeSet { get; set; }
		[Column(Name = "CountryCode")] public int CountryCode { get; set; }
		[Column(Name = "CSName")] public string CSName { get; set; }
		[Column(Name = "CurrentTimeZone")] public int CurrentTimeZone { get; set; }
		[Column(Name = "Manufacturer")] public string Manufacturer { get; set; }
		[Column(Name = "OSArchitecture")] public string Architecture { get; set; }
		[Column(Name = "RegisteredUser")] public string RegisteredUser { get; set; }
		[Column(Name = "SerialNumber")] public string SerialNumber { get; set; }
		[Column(Name = "ServicePackMajorVersion")] public int SPMajor { get; set; }
		[Column(Name = "ServicePackMinorVersion")] public int SPMinor { get; set; }
		[Column(Name = "SystemDirectory")] public string SystemDirectory { get; set; }
		[Column(Name = "WindowsDirectory")] public string WindowsDirectory { get; set; }
		[Column(Name = "Version")] public string Version { get; set; }

		public override string ToString() => Caption;

	}

}