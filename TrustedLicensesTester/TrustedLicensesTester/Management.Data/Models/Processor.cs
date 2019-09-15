using System.Data.Linq.Mapping;

namespace System.Management.Data.Models
{

	[Table(Name = "Win32_Processor")]
	public class Processor
	{

		[Column(Name = "AddressWidth")] public int AddressWidth { get; set; }
		[Column(Name = "Architecture")] public int Architecture { get; set; }
		[Column(Name = "Availability")] public int Availability { get; set; }
		[Column(Name = "Caption")] public string Caption { get; set; }
		[Column(Name = "CurrentClockSpeed")] public int CurrentClockSpeed { get; set; }
		[Column(Name = "CurrentVoltage")] public int CurrentVoltage { get; set; }
		[Column(Name = "DataWidth")] public int DataWidth { get; set; }
		[Column(Name = "Description")] public string Description { get; set; }
		[Column(Name = "DeviceID")] public string DeviceId { get; set; }
		[Column(Name = "Manufacturer")] public string Manufacturer { get; set; }
		[Column(Name = "MaxClockSpeed")] public int MaxClockSpeed { get; set; }
		[Column(Name = "Name")] public string Name { get; set; }
		[Column(Name = "NumberOfCores")] public int PhysicalCoreCount { get; set; }
		[Column(Name = "NumberOfLogicalProcessors")] public int LogicalCoreCount { get; set; }
		[Column(Name = "ProcessorId")] public string ProcessorId { get; set; }
		[Column(Name = "SocketDesignation")] public string SocketDesignation { get; set; }
		[Column(Name = "ThreadCount")] public int ThreadCount { get; set; }

		public override string ToString() => Name;

	}

}