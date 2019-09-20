using System;

namespace TrustedLicenses.Example
{

	public class Customer
	{

		public string Name { get; set; }
		public Guid MachineGuid { get; set; }

		public Customer() { }

		public Customer(string name, Guid machineGuid)
		{
			Name = name;
			MachineGuid = machineGuid;
		}

		public override string ToString() => Name;

	}

}