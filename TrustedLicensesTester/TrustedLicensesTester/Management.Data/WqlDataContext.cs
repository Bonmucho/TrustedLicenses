using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace System.Management.Data
{

	public class WqlDataContext<T> : IEnumerable<T>, IDisposable
	{

		private const string WqlSyntaxSelect = "SELECT";
		private const string WqlSyntaxFrom = "FROM";

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ManagementObjectSearcher Searcher;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Dictionary<string, string> ColumnNameMapping;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool DisposedValue = false;

		public WqlDataContext()
		{
			object tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), false)?.FirstOrDefault();
			string tableName = tableAttribute != null ? ((TableAttribute)tableAttribute).Name : typeof(T).Name;

			IEnumerable<PropertyInfo> properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).
				Where(x => x.GetCustomAttribute<ColumnAttribute>(false) != null);

			ColumnNameMapping = properties.ToDictionary(x => x.Name,
				x => x.GetCustomAttribute<ColumnAttribute>(false).Name ?? x.Name);

			Searcher = new ManagementObjectSearcher
			{
				Query = new ObjectQuery(string.Join(" ", WqlSyntaxSelect,
					string.Join(", ", ColumnNameMapping.Values), WqlSyntaxFrom, tableName))
			};
		}

		public IEnumerator<T> GetEnumerator()
		{
			using (ManagementObjectCollection collection = Searcher.Get())
				foreach (ManagementObject obj in collection)
				{
					T instance = (T)Activator.CreateInstance(typeof(T));

					foreach (KeyValuePair<string, string> column in ColumnNameMapping)
					{
						PropertyInfo info = typeof(T).GetProperty(column.Key);
						info.SetValue(instance, Convert.ChangeType(
							obj[column.Value].ToString().Trim(), info.PropertyType));
					}
						
					yield return instance;
				}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public override string ToString() => Searcher.Query.QueryString;

		protected virtual void Dispose(bool disposing)
		{
			if (!DisposedValue)
			{
				if (disposing) Searcher.Dispose();
				DisposedValue = true;
			}
		}

		public void Dispose() => Dispose(true);

	}

}