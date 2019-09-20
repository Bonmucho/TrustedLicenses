<img src="https://image.flaticon.com/icons/svg/2127/2127440.svg" width="48"><span style="font-family:Segoe UI;font-size:48px;font-weight:lighter"> <span style="color:#1E90FF">Trusted</span><span style="color:#AAAAAA">Licenses</span></span>

# <span style="font-family:Segoe UI"><span style="color:#1E90FF">Trusted</span><span style="color:#AAAAAA">Licenses</span></span> - *A lightweight .NET Licensing Framework*
> 📜 License your goods - trust in <span style="font-family:Segoe UI;font-weight:lighter;"> <span style="color:#1E90FF">Trusted</span><span style="color:#AAAAAA">Licenses</span></span>!

## Features
- ✅ **Client-sided** Licensing Framework (*no need to query server*)
- 🔑 **Secure**, **reliable** and **extensible**
- ✅ **Simple** and easy to use
- ✅ **Minimal**, **fast** and **efficient**
- ✅ 100% managed C# code for **.NET 4.5+**
- 🔐 Latest security technologies, using **DSA** and **RSA** signature algorithms
- ✅ **High platform compatibility** through use of international presentation standards, such as:
	- **DSA** and **RSA** signature algorithms
	- **SHA** hashing algorithms
	- **XML version 1.0** for actual license
	- Exclusive XML Canonicalization **XML-C14N**
- ✅ Easy license creation through use of LicenseBuilder
- ✅ Easy license validation with validation chain

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

## Changelog

> There is no changelog available until **Release 1.0**.

## Weakness
This framework is very strong and makes generating licenses for unauthorized parties practical impossible.
Also the source code of this library or your program can be visited by the public without any harm. Of course if you store your private keys safely.

Unauthorized parties however could decompile your program or this library, disable the validation checks with the well known *NOOP* operation and compile back the result. So they'll get a "cracked" version of your program. This library applies no code obfuscation (or other protection strategies) to your code - this is what you have to do at your own.

**Accordingly this library is not responsible for protecting your work for such (or similar) attacks!**

## Credits

### Flaticon

<div>Icons made by <a href="https://www.flaticon.com/authors/itim2101" title="itim2101">itim2101</a> from <a href="https://www.flaticon.com/"             title="Flaticon">www.flaticon.com</a></div>