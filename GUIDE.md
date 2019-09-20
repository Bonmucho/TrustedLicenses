# TrustedLicenses - A lightweight .NET Licensing Framework
> 📜 License your goods - trust in TrustedLicenses!

## How to use TrustedLicenses

Creating a asymmetric key for signing:

```C#
using System.Security.Cryptography;

// Create a new DSA CSP instance that stores our key.
DSACryptoServiceProvider key = new DSACryptoServiceProvider();

// You can export your key as xml string. This is a public key - feel free to distribute it to your customers.
string xml = key.ToXmlString(false);

// Export of the private key is mostly identical - just set the parameter to 'true'. Never distribute this key!
string xml = key.ToXmlString(true);

// Import of the key is also very straightforward.
key.FromXmlString(xml);
```

Creating a ``Customer`` class that stores our customer (and machine) relevant information:

```C#
public class Customer
{

	public string Name { get; set; }

	// Here we will store an hardware ID for the consumer system.
	public Guid MachineGuid { get; set; }

	// Do not forget to create an empty constructor due to serialization.
	public Customer() { }

	public Customer(string name, Guid machineGuid)
	{
		Name = name;
		MachineGuid = machineGuid;
	}

}
```

Creating a license and signing it:

```C#
using System.Security.Licensing;

// Creating a new license object.
License<Customer> license = License<Customer>.Create().
	ValidityTime(TimeSpan.FromDays(20)). // where the expiration date is in 20 days.
	ForVersion(ModuleVersion.Parse("1.2")). // and it is only valid for your program in version 1.2.
	AssignData(new Customer
	(
		"Test Company",
		Guid.Parse("69ff93d5-caf2-4aaf-882a-30bb0b81c5d6")
	)). // Assign your customer object with your collected data.
	GetResult();

// Sign your license.
XmlDocument xmlLicense = license.Sign(key);
```

Verifying your license:

```C#
// Validating and verifying your license (any validation is optional).
bool result = License<Customer>.ValidateAgainst(xmlLicense, out License<Customer> outLicense).
	ForValidityTime(DateTime.Now). // Checks if the license is expired.
	ForVersion(ModuleVersion.Parse(Application.ProductVersion.ToString())). // Checks if the license is valid for this version of your program.
	For(x => x.MachineGuid == GetMachineGuid(), "MachineGuid"). // Custom validation against our hardware ID. GetMachineGuid() is a call to a function wich creates a unique hardware ID for the current system.
	Verify(); // Finally verifying your license with its embedded signature.
```