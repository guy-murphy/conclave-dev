using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Inversion;

namespace Conclave.Funder.Model {
	public class JournalEntry : AgentScopedData, IEquatable<JournalEntry> {

		public static bool operator ==(JournalEntry m1, JournalEntry m2) {
			if (Object.ReferenceEquals(m1, m2)) return true;
			if (((object)m1 == null) || ((object)m2 == null)) return false;
			return m1.Equals(m2);
		}

		public static bool operator !=(JournalEntry m1, JournalEntry m2) {
			return !(m1 == m2);
		}

		private static readonly JournalEntry _blank = new JournalEntry(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, default(DateTime));

		public static new JournalEntry Blank {
			get { return _blank; }
		}

		private readonly DateTime _when;

		private int _hashcode = 0;
		private JObject _data;

		public DateTime When {
			get { return _when; }
		}

		public override JObject Data {
			get { return _data ?? (_data = this.ToJsonObject()); }
		}

		public JournalEntry(string parent, string who, string scope, string name, string value) : this(parent, who, scope, name, value, DateTime.Now) { }
		public JournalEntry(string parent, string who, string scope, string name, string value, DateTime when) : this(Guid.NewGuid().ToString(), parent, who, scope, name, value, when) { }
		public JournalEntry(JournalEntry other) : this(other.Id, other.Parent, other.Who, other.Scope, other.Name, other.Value, other.When) { }

		public JournalEntry(string id, string parent, string who, string scope, string name, string value, DateTime when)
			: base(id, parent, who, scope, name, value) {
			_when = when;
		}

		public override bool Equals(object obj) {
			JournalEntry other = obj as JournalEntry;
			return (other != null) && this.Equals(other);
		}

		public bool Equals(JournalEntry other) {
			if (object.ReferenceEquals(this, other)) return true;
			return (
				base.Equals(other) &&
				this.When == other.When
			);
		}

		public override int GetHashCode() {
			if (_hashcode == 0) {			
				int hc = "JournalEntry".GetHashCode();
				hc = hc * 31 + base.GetHashCode();
				hc = hc * 31 + this.Who.GetHashCode();
				_hashcode = hc;
			}
			return _hashcode;
		}

		public override void ContentToXml(XmlWriter writer) {
			base.ContentToXml(writer);
			writer.WriteAttributeString("when", this.When.ToString());
		}

		public override void ToXml(XmlWriter writer) {
			writer.WriteStartElement("journal-entry");
			this.ContentToXml(writer);
			writer.WriteEndElement();
		}

		public override void ContentToJson(JsonWriter writer) {
			base.ContentToJson(writer);
			writer.WritePropertyName("when");
			writer.WriteValue(this.When.ToString());
		}

		public override void ToJson(JsonWriter writer) {
			writer.WriteStartObject();
			writer.WritePropertyName("_type");
			writer.WriteValue("journalEntry");
			this.ContentToJson(writer);
			writer.WriteEndObject();
		}

		public new class Builder : AgentScopedData.Builder, IConsumeData<Builder, JournalEntry> {

			public static implicit operator JournalEntry(Builder builder) {
				return builder.ToConcrete();
			}

			public static implicit operator Builder(JournalEntry entry) {
				return new Builder(entry);
			}

			public static ImmutableHashSet<JournalEntry> CreateImmutableCollection(IEnumerable<JournalEntry.Builder> meta) {
				HashSet<JournalEntry> temp = new HashSet<JournalEntry>();
				foreach (JournalEntry item in meta) {
					temp.Add(item);
				}
				return ImmutableHashSet.Create<JournalEntry>(temp.ToArray());
			}

			public DateTime When { get; set; }

			public Builder() : base() {
				this.When = DateTime.Now;
			}
			public Builder(string parent, string who, string scope, string name, string value) : this(parent, who, scope, name, value, DateTime.Now) { }
			public Builder(string parent, string who, string scope, string name, string value, DateTime when) : this(Guid.NewGuid().ToString(), parent, who, scope, name, value, when) { }

			public Builder(string id, string parent, string who, string scope, string name, string value, DateTime when)
				: base(id, parent, who, scope, name, value) {
				this.When = when;
			}

			public Builder(JournalEntry other) {
				this.FromConcrete(other);
			}

			public Builder FromConcrete(JournalEntry other) {
				base.FromConcrete(other);
				this.When = other.When;
				return this;
			}

			public new Builder FromJson(JObject json) {
				if (json["_type"].Value<string>() != "journalEntry") throw new InvalidOperationException("The json being used does not represent the type it is being read into.");

				this.Id = json["id"].Value<string>();
				this.Parent = json["parent"].Value<string>();
				this.Who = json["who"].Value<string>();
				this.Scope = json["scope"].Value<string>();
				this.Name = json["name"].Value<string>();
				this.Value = json["value"].Value<string>();
				this.When = json["when"].Value<DateTime>();

				return this;
			}

			public new JournalEntry ToConcrete() {
				return new JournalEntry(this.Id, this.Parent, this.Who, this.Scope, this.Name, this.Value, this.When);
			}

		}

	}
}
