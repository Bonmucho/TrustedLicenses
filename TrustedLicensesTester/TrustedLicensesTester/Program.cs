using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Data;
using System.Management.Data.Models;
using System.Security.Cryptography;
using System.Security.Licensing;
using System.Xml;
using OperatingSystem = System.Management.Data.Models.OperatingSystem;

namespace TrustedLicensesTester
{

	public class Program
	{

		/// <summary>
		/// Oaky?
		/// </summary>
		public class Customer
		{

			public string Name { get; set; }
			public OperatingSystem OperatingSystem { get; set; }
			public Guid MachineGuid { get; set; }

			public Customer() { }

			public Customer(string name, OperatingSystem os, Guid machineGuid)
			{
				Name = name;
				OperatingSystem = os;
				MachineGuid = machineGuid;
			}

		}

		public static void Main()
		{

			DSACryptoServiceProvider key = new DSACryptoServiceProvider();

			License<Customer>.ThrowExcpetionsOnValidation = true;

			License<Customer> license = License<Customer>.Create().
				ValidityTime(TimeSpan.FromDays(20)).
				ForVersion(ModuleVersion.Parse("1.2")).
				AssignData(new Customer
				(
					"Testkunde Deutschland GmbH",
					SystemInformationProvider.GetOperatingSystems().FirstOrDefault(),
					SystemInformationProvider.GetMachineGuid()
				)).
				GetResult();
			
			XmlDocument xmlLicense = license.Sign(key);

			bool result = License<Customer>.ValidateAgainst(xmlLicense, out License<Customer> outLicense).
				ForValidityTime(DateTime.Now).
				ForVersion(ModuleVersion.Parse("1.2")).
				For(x => x.MachineGuid == SystemInformationProvider.GetMachineGuid(), "MachineGuid").
				Verify();

			//bool res = outLicense.ValidateFor().For(x => x.Name == "Test").GetResult();

			Console.ReadKey();

		}

	}

}