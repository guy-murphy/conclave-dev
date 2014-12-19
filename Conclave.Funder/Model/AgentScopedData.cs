using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Inversion;


namespace Conclave.Funder.Model {
	public class AgentScopedData : ScopedData, IEquatable<AgentScopedData> {

		public static bool operator ==(AgentScopedData m1, AgentScopedData m2) {
			if (Object.ReferenceEquals(m1, m2)) return true;
			if (((object)m1 == null) || ((object)m2 == null)) return false;
			return m1.Equals(m2);
		}

		public static bool operator !=(AgentScopedData m1, AgentScopedData m2) {
			return !(m1 == m2);
		}

		private static readonly AgentScopedData _blank = new AgentScopedData(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty);

		/// <summary>
		/// Represents an empty AgentScopedData item as
		/// an alternative to a <b>null</b> value.
		/// </summary>
		public static new AgentScopedData Blank {
			get { return _blank; }
		}

		private readonly string _who;

		private int _hashcode = 0;
		private JObject _data;

		public string Who {
			get { return _who; }
		}

		/// <summary>
		/// Provides an abstract representation
		/// of the objects data expressed as a JSON object.
		/// </summary>
		/// <remarks>
		/// For this type the json object is created once.
		/// </remarks>
		public override JObject Data {
			get { return _data ?? (_data = this.ToJsonObject()); }
		}

		public AgentScopedData(string parent, string who, string name, string value) : this(parent, who, "default", name, value) { }
		public AgentScopedData(string parent, string who, string scope, string name, string value) : this(Guid.NewGuid().ToString(), parent, who, scope, name, value) { }

		public AgentScopedData(string id, string parent, string who, string scope, string name, string value)
			: base(id, parent, scope, name, value) {
			_who = who;
		}
		public AgentScopedData(AgentScopedData other) : this(other.Id, other.Parent, other.Who, other.Scope, other.Name, other.Value) { }


		public override bool Equals(object obj) {
			AgentScopedData other = obj as AgentScopedData;
			return (other != null) && this.Equals(other);
		}

		public bool Equals(AgentScopedData other) {
			if (object.ReferenceEquals(this, other)) return true;
			return (
				base.Equals(other) &&
				this.Who == other.Who
			);
		}

		public override int GetHashCode() {
			if (_hashcode == 0) {
				int hc = 17;
				hc = hc * 31 + "AgentScopedData".GetHashCode();
				hc = hc * 31 + base.GetHashCode();
				hc = hc * 31 + this.Who.GetHashCode();
				_hashcode = hc;
			}
			return _hashcode;
		}

		public override void ContentToXml(XmlWriter writer) {
			base.ContentToXml(writer);
			writer.WriteAttributeString("who", this.Who);
		}

		public override void ToXml(XmlWriter writer) {
			writer.WriteStartElement("agent-scoped-data");
			this.ContentToXml(writer);
			writer.WriteEndElement();
		}

		public override void ContentToJson(JsonWriter writer) {
			base.ContentToJson(writer);
			writer.WritePropertyName("who");
			writer.WriteValue(this.Who);
		}

		public override void ToJson(JsonWriter writer) {
			writer.WriteStartObject();
			writer.WritePropertyName("_type");
			writer.WriteValue("agentScopedData");
			this.ContentToJson(writer);
			writer.WriteEndObject();
		}

		public new class Builder : ScopedData.Builder {

			public static implicit operator AgentScopedData(Builder builder) {
				return builder.ToAgentScopedData();
			}

			public static implicit operator Builder(AgentScopedData agentScopedData) {
				return new Builder(agentScopedData);
			}

			public static ImmutableHashSet<AgentScopedData> CreateImmutableCollection(IEnumerable<AgentScopedData.Builder> meta) {
				return meta.Select(builder => builder.ToAgentScopedData()).ToImmutableHashSet();
			}

			public string Who { get; set; }

			public Builder() : base() {
				this.Who = String.Empty;
			}
			public Builder(string parent, string who, string name, string value) : this(parent, who, "default", name, value) { }
			public Builder(string parent, string who, string scope, string name, string value) : this(Guid.NewGuid().ToString(), parent, who, scope, name, value) { }

			public Builder(string id, string parent, string who, string scope, string name, string value)
				: base(id, parent, scope, name, value) {
				this.Who = who;
			}

			public Builder(AgentScopedData other) {
				this.FromAgentScopedData(other);
			}
			
			public Builder FromAgentScopedData(AgentScopedData other) {
				base.FromConcrete(other);
				this.Who = other.Who;
				return this;
			}

			public AgentScopedData ToAgentScopedData() {
				return new AgentScopedData(this.Id, this.Parent, this.Who, this.Scope, this.Name, this.Value);
			}

		}
	}
}
