using System;
using System.Data;

namespace Conclave.Data.Store {

	/// <summary>
	/// Extansion methods acting upon a data record.
	/// </summary>
	public static class DataRecordExtensions {

		/// <summary>
		/// Determines whether or not a data record has a specified field.
		/// </summary>
		/// <param name="self">The data reader to act upon.</param>
		/// <param name="name">The name of the field to check for.</param>
		/// <returns>Returns true if the reader has the specified field; otherwise, returns false.</returns>
		public static bool HasField(this IDataRecord self, string name) {
			for (int i = 0; i < self.FieldCount; i++) {
				if (self.GetName(i).Equals(name, StringComparison.InvariantCulture)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Reads the value of the named field from the data record.
		/// </summary>
		/// <param name="self">The data record to act upon.</param>
		/// <param name="name">The name of the field to read.</param>
		/// <returns>Returns the value of the specified field.</returns>
		public static object ReadObject(this IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? null : self.GetValue(ordinal);
		}

		/// <summary>
		/// Reads the value of the named field from the data record.
		/// </summary>
		/// <param name="self">The data record to act upon.</param>
		/// <param name="name">The name of the field to read.</param>
		/// <returns>Returns the value of the specified field.</returns>
		public static byte[] ReadData(this IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			if (self.IsDBNull(ordinal)) {
				return new byte[0];
			} else {
				byte[] data = (byte[])self.GetValue(ordinal); // possibly implimentation specific
				return data;
			}
		}

		/// <summary>
		/// Reads the string value of the named field from the data record.
		/// </summary>
		/// <param name="self">The data record to act upon.</param>
		/// <param name="name">The name of the field to read.</param>
		/// <returns>Returns the string value of the specified field.</returns>
		public static string ReadString(this IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? null : self.GetString(ordinal);
		}

		/// <summary>
		/// Reads the int value of the named field from the data record.
		/// </summary>
		/// <param name="self">The data record to act upon.</param>
		/// <param name="name">The name of the field to read.</param>
		/// <returns>Returns the int value of the specified field.</returns>
		public static int ReadInt(this IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? default(int) : self.GetInt32(ordinal);
		}

		public static decimal ReadDecimal(this IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? default(decimal) : self.GetDecimal(ordinal);
		}

		public static bool ReadBool(this IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? default(bool) : self.GetBoolean(ordinal);
		}

		public static long ReadLong(this IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? default(long) : self.GetInt64(ordinal);
		}

		public static Guid ReadGuid(this IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? Guid.Empty : self.GetGuid(ordinal);
		}

		/// <summary>
		/// Reads the date-time value of the named field from the data record or null.
		/// </summary>
		/// <param name="self">The data record to act upon.</param>
		/// <param name="name">The name of the field to read.</param>
		/// <returns>Returns the date-time value of the specified field or null.</returns>
		public static DateTime? ReadDateTimeOrNull(IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? null : (DateTime?)self.GetDateTime(ordinal);
		}

		/// <summary>
		/// Reads the  date-timevalue of the named field from the data record.
		/// </summary>
		/// <param name="self">The data record to act upon.</param>
		/// <param name="name">The name of the field to read.</param>
		/// <returns>Returns the date-time value of the specified field.</returns>
		public static DateTime ReadDateTime(IDataRecord self, string name) {
			int ordinal = self.GetOrdinal(name);
			return self.IsDBNull(ordinal) ? default(DateTime) : self.GetDateTime(ordinal);
		}
		
	}
}
