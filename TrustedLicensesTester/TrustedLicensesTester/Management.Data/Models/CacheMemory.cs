using System.Data.Linq.Mapping;

namespace System.Management.Data.Models
{

	[Table(Name = "Win32_CacheMemory")]
	public class CacheMemory
	{

		[Column(Name = "BlockSize")] public int BlockSize { get; set; }
		[Column(Name = "Caption")] public string Caption { get; set; }
		[Column(Name = "Description")] public string Description { get; set; }
		[Column(Name = "DeviceID")] public string DeviceId { get; set; }
		[Column(Name = "InstalledSize")] public int InstalledSize { get; set; }
		[Column(Name = "Level")] public int Level { get; set; }
		[Column(Name = "MaxCacheSize")] public int MaxCacheSize { get; set; }
		[Column(Name = "Name")] public string Name { get; set; }
		[Column(Name = "NumberOfBlocks")] public int NumberOfBlocks { get; set; }
		[Column(Name = "Purpose")] public string Purpose { get; set; }

		public override string ToString() => Purpose;

	}

}