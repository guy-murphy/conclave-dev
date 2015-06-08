using System;
using System.Collections;
using System.Data;

namespace Conclave.Data.Store {
	/// <summary>
	/// Extansion methods acting upon a data reader.
	/// </summary>
	public static class DataReaderEx {
		/// <summary>
		/// Iterates over each record, and performs the specified action upon it.
		/// </summary>
		/// <param name="self">The data reader to act upon.</param>
		/// <param name="action">The action to perform upon each record.</param>
		public static void ForEach(this IDataReader self, Action<IDataRecord> action) {
			while (self.Read()) {
				action(self);
			}
		}
		
	}
}
