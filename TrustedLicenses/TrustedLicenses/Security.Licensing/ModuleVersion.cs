using System.Globalization;
using System.Xml.Serialization;

namespace System.Security.Licensing
{

	/// <summary>
	/// Represents the version number of an assembly, operating system, or the common language runtime.
	/// </summary>
	[Serializable]
	public class ModuleVersion : ICloneable, IComparable
	{

		/// <summary>
		/// Gets the value of the major component of the version number for the current <see cref="ModuleVersion"/> object.
		/// </summary>
		[XmlAttribute] public int Major { get; set; }

		/// <summary>
		/// Gets the value of the minor component of the version number for the current <see cref="ModuleVersion"/> object.
		/// </summary>
		[XmlAttribute] public int Minor { get; set; }

		/// <summary>
		/// Gets the value of the build component of the version number for the current <see cref="ModuleVersion"/> object.
		/// </summary>
		[XmlAttribute] public int Build { get; set; }

		/// <summary>
		/// Gets the value of the revision component of the version number for the current <see cref="ModuleVersion"/> object.
		/// </summary>
		[XmlAttribute] public int Revision { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleVersion"/> class.
		/// </summary>
		public ModuleVersion()
		{
			Build = -1;
			Revision = -1;
			Major = 0;
			Minor = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleVersion"/> class using the specified string.
		/// </summary>
		/// <param name="version">A string containing the major, minor, build, and revision numbers, where each number is delimited with a period character ('.').</param>
		public ModuleVersion(string version) => Initialize(version, false);

		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleVersion"/> class using the specified major and minor values.
		/// </summary>
		/// <param name="major">The major version number.</param>
		/// <param name="minor">The minor version number.</param>
		public ModuleVersion(int major, int minor) => Initialize(major, minor, null, null);

		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleVersion"/> class using the specified major, minor, and build values.
		/// </summary>
		/// <param name="major">The major version number.</param>
		/// <param name="minor">The minor version number.</param>
		/// <param name="build">The build number.</param>
		public ModuleVersion(int major, int minor, int build) => Initialize(major, minor, build, null);

		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleVersion"/> class with the specified major, minor, build, and revision numbers.
		/// </summary>
		/// <param name="major">The major version number.</param>
		/// <param name="minor">The minor version number.</param>
		/// <param name="build">The build number.</param>
		/// <param name="revision">The revision number.</param>
		public ModuleVersion(int major, int minor, int build, int revision) => Initialize(major, minor, build, revision);

		private bool Initialize(string version, bool tryMode)
		{
			if (version == null)
				if (tryMode) return false;
				else throw new ArgumentNullException(nameof(version));

			string[] parts = version.Split(new[] { '.' });
			if ((parts.Length < 2) || (parts.Length > 4))
				if (tryMode) return false;
				else throw new ArgumentException(nameof(version));

			if (tryMode)
			{
				bool majorResult = int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int major);
				bool minorResult = int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int minor);

				bool buildResult = int.TryParse(parts.Length >= 3 ? parts[2] : string.Empty, NumberStyles.Integer,
					CultureInfo.InvariantCulture, out int buildValue);
				bool revisionResult = int.TryParse(parts.Length >= 4 ? parts[3] : string.Empty, NumberStyles.Integer,
					CultureInfo.InvariantCulture, out int revisionValue);

				Initialize(major, minor, buildResult ? (int?)buildValue : null, revisionResult ? (int?)revisionValue : null);

				return majorResult && minorResult;
			}
			else
			{
				Initialize(int.Parse(parts[0], CultureInfo.InvariantCulture),
					int.Parse(parts[1], CultureInfo.InvariantCulture),
					parts.Length >= 3 ? (int?)int.Parse(parts[2], CultureInfo.InvariantCulture) : null,
					parts.Length >= 4 ? (int?)int.Parse(parts[3], CultureInfo.InvariantCulture) : null);

				return true;
			}
		}

		private void Initialize(int major, int minor, int? build, int? revision)
		{
			if (major < 0) throw new ArgumentOutOfRangeException(nameof(Major));
			if (minor < 0) throw new ArgumentOutOfRangeException(nameof(Minor));
			if ((build ?? 0) < 0) throw new ArgumentOutOfRangeException(nameof(Build));
			if ((revision ?? 0) < 0) throw new ArgumentOutOfRangeException(nameof(Revision));

			Major = major;
			Minor = minor;
			Build = build ?? -1;
			Revision = revision ?? -1;
		}

		/// <summary>
		/// Converts the string representation of a version number to an equivalent <see cref="ModuleVersion"/> object.
		/// </summary>
		/// <param name="s">A string that contains a version number to convert.</param>
		/// <returns><see cref="ModuleVersion"/></returns>
		public static ModuleVersion Parse(string s) => new ModuleVersion(s);

		/// <summary>
		/// Converts the string representation of a version number to an equivalent <see cref="ModuleVersion"/> object.
		/// </summary>
		/// <param name="s">A string that contains a version number to convert.</param>
		/// <param name="version">When this method returns, contains the <see cref="ModuleVersion"/> equivalent of the number that is contained in input, if the conversion succeeded. If input is null, Empty, or if the conversion fails, result is null when the method returns.</param>
		/// <returns></returns>
		public static bool TryParse(string s, out ModuleVersion version)
		{
			version = new ModuleVersion();
			bool result = version.Initialize(s, true);
			if (!result) version = null;
			return result;
		}

		/// <summary>
		/// Returns a new <see cref="ModuleVersion"/> object whose value is the same as the current <see cref="ModuleVersion"/> object.
		/// </summary>
		/// <returns><see cref="ModuleVersion"/></returns>
		public object Clone() =>
			new ModuleVersion
			{
				Major = Major,
				Minor = Minor,
				Build = Build,
				Revision = Revision
			};

		/// <summary>
		/// Compares the current <see cref="ModuleVersion"/> object to a specified object and returns an indication of their relative values.
		/// </summary>
		/// <param name="obj">An object to compare, or null.</param>
		/// <returns><see cref="int"/></returns>
		public int CompareTo(object obj)
		{
			if (obj == null) return 1;

			ModuleVersion version = (ModuleVersion)(obj ?? throw new ArgumentException(nameof(obj)));

			int major = Major.CompareTo(version.Major);
			if (major != 0) return major;

			int minor = Minor.CompareTo(version.Minor);
			if (minor != 0) return minor;

			int build = Build.CompareTo(version.Build);
			if (build != 0) return build;

			int revision = Revision.CompareTo(version.Revision);
			if (revision != 0) return revision;

			return 0;
		}

		/// <summary>
		/// Returns a value indicating whether the current <see cref="ModuleVersion"/> object is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with the current <see cref="ModuleVersion"/> object, or null.</param>
		/// <returns><see cref="bool"/></returns>
		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is ModuleVersion)) return false;
			ModuleVersion version = (ModuleVersion)obj;

			return (Major == version.Major) && (Minor == version.Minor) &&
				(Build == version.Build) && (Revision == version.Revision);
		}

		/// <summary>
		/// Returns a hash code for the current <see cref="ModuleVersion"/> object.
		/// </summary>
		/// <returns><see cref="int"/></returns>
		public override int GetHashCode() =>
			((Major & 15) << 0x1c) | ((Minor & 0xff) << 20) | ((Build & 0xff) << 12) | (Revision & 0xfff);

		/// <summary>
		/// Determines whether two specified <see cref="ModuleVersion"/> objects are equal.
		/// </summary>
		/// <param name="left">The first Version object.</param>
		/// <param name="right">The second Version object.</param>
		/// <returns><see cref="bool"/></returns>
		public static bool operator ==(ModuleVersion left, ModuleVersion right) => left.Equals(right);

		/// <summary>
		/// Determines whether two specified <see cref="ModuleVersion"/> objects are not equal.
		/// </summary>
		/// <param name="left">The first Version object.</param>
		/// <param name="right">The second Version object.</param>
		/// <returns><see cref="bool"/></returns>
		public static bool operator !=(ModuleVersion left, ModuleVersion right) => !left.Equals(right);

		/// <summary>
		/// Determines whether the first specified <see cref="ModuleVersion"/> object is greater than the second specified <see cref="ModuleVersion"/> object.
		/// </summary>
		/// <param name="left">The first Version object.</param>
		/// <param name="right">The second Version object.</param>
		/// <returns><see cref="bool"/></returns>
		public static bool operator >(ModuleVersion left, ModuleVersion right) => left.CompareTo(right) > 0;

		/// <summary>
		/// Determines whether the first specified <see cref="ModuleVersion"/> object is less than the second specified <see cref="ModuleVersion"/> object.
		/// </summary>
		/// <param name="left">The first Version object.</param>
		/// <param name="right">The second Version object.</param>
		/// <returns><see cref="bool"/></returns>
		public static bool operator <(ModuleVersion left, ModuleVersion right) => left.CompareTo(right) < 0;

		/// <summary>
		/// Determines whether the first specified <see cref="ModuleVersion"/> object is greater than or equal to the second specified <see cref="ModuleVersion"/> object.
		/// </summary>
		/// <param name="left">The first Version object.</param>
		/// <param name="right">The second Version object.</param>
		/// <returns><see cref="bool"/></returns>
		public static bool operator >=(ModuleVersion left, ModuleVersion right) => left.CompareTo(right) >= 0;

		/// <summary>
		/// Determines whether the first specified <see cref="ModuleVersion"/> object is less than or equal to the second <see cref="ModuleVersion"/> object.
		/// </summary>
		/// <param name="left">The first Version object.</param>
		/// <param name="right">The second Version object.</param>
		/// <returns><see cref="bool"/></returns>
		public static bool operator <=(ModuleVersion left, ModuleVersion right) => left.CompareTo(right) <= 0;

		/// <summary>
		/// Converts the value of the current <see cref="ModuleVersion"/> object to its equivalent string representation.
		/// </summary>
		/// <returns><see cref="string"/></returns>
		public override string ToString() => Build == -1 ? ToString(2) : (Revision == -1 ? ToString(3) : ToString(4));

		/// <summary>
		/// Converts the value of the current <see cref="ModuleVersion"/> object to its equivalent string representation. A specified count indicates the number of components to return.
		/// </summary>
		/// <param name="fieldCount"></param>
		/// <returns><see cref="string"/></returns>
		public string ToString(int fieldCount)
		{
			if (fieldCount < 0 | fieldCount > 4 | (fieldCount >= 3 && Build == -1) || (fieldCount > 4 && Revision == -1))
				throw new ArgumentOutOfRangeException(nameof(fieldCount));
			return string.Join(".", new[] { Major.ToString(), Minor.ToString(),
				Build.ToString(), Revision.ToString() }, 0, fieldCount);
		}

	}

}