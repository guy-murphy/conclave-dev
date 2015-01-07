using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;

using Newtonsoft.Json;

using Inversion;
using Conclave.Map.Model;
using Newtonsoft.Json.Linq;

namespace Conclave.Funder.Model {
	public class Agent : Node, IData, IEquatable<Agent>, IMutate<Agent.Builder, Agent> {

		private readonly ImmutableHashSet<Metadata> _contacts;
		private readonly ImmutableHashSet<Metadata> _names;
		private readonly ImmutableHashSet<Metadata> _addresses;

		private int _hashcode = 0;
		private JObject _data;

		public IEnumerable<Metadata> Contacts {
			get { return _contacts; }
		}
		public IEnumerable<Metadata> Names {
			get { return _names; }
		}
		public IEnumerable<Metadata> Addresses {
			get { return _addresses; }
		}

		/// <summary>
		/// Provides an abstract representation
		/// of the objects data expressed as a JSON object.
		/// </summary>
		/// <remarks>
		/// For this type the json object is created once.
		/// </remarks>
		public JObject Data {
			get { return _data ?? (_data = this.ToJsonObject()); }
		}

		public Agent(string id, 
			IEnumerable<Metadata> meta,
			IEnumerable<Metadata> names,
			IEnumerable<Metadata> contacts,
			IEnumerable<Metadata> addresses
		) : base(id, meta) {
			_names = (names == null) ? ImmutableHashSet.Create<Metadata>() : names.ToImmutableHashSet();
			_contacts = (contacts == null) ? ImmutableHashSet.Create<Metadata>() : contacts.ToImmutableHashSet();
			_addresses = (addresses == null) ? ImmutableHashSet.Create<Metadata>() : addresses.ToImmutableHashSet();
		}

		public Agent(IEnumerable<Metadata> meta, IEnumerable<Metadata> names, IEnumerable<Metadata> contacts, IEnumerable<Metadata> addresses) : this(Guid.NewGuid().ToString(), meta, names, contacts, addresses) { }
		public Agent(IEnumerable<Metadata> meta, IEnumerable<Metadata> names, IEnumerable<Metadata> contacts) : this(meta, names, contacts, null) { }
		public Agent(IEnumerable<Metadata> meta, IEnumerable<Metadata> names) : this(meta, names, null, null) { }
		public Agent(IEnumerable<Metadata> meta) : this(meta, null, null, null) { }

		public Agent(Agent other) : this(other.Id, other.Metadata, other.Names, other.Contacts, other.Addresses) { }

		object ICloneable.Clone() {
			return new Agent(this);
		}

		public Agent Clone() {
			return new Agent(this);
		}

		public override bool Equals(object obj) {
			Agent other = obj as Agent;
			return (other != null) && this.Equals(other);
		}

		public bool Equals(Agent other) {
			if (Object.ReferenceEquals(this, other)) return true;
			return (
				base.Equals(other) &&
				this.Names.SequenceEqual(other.Names) &&
				this.Contacts.SequenceEqual(other.Contacts) &&
				this.Addresses.SequenceEqual(other.Addresses)
			);
		}

		public override int GetHashCode() {
			if (_hashcode == 0) {
				int hc = 17;
				hc = hc * 31 + base.GetHashCode();
				foreach (Metadata item in this.Names) {
					hc = hc * 31 + item.GetHashCode();
				}
				foreach (Metadata item in this.Contacts) {
					hc = hc * 31 + item.GetHashCode();
				}
				foreach (Metadata item in this.Addresses) {
					hc = hc * 31 + item.GetHashCode();
				}
				_hashcode = hc;
			}
			return _hashcode;
		}

		public Agent Mutate(Func<Builder, Agent> mutator) {
			Builder builder = new Builder(this);
			return mutator(builder);
		}

		public override void ToXml(XmlWriter writer) {
			writer.WriteStartElement("agent");
			base.ContentToXml(writer);
			writer.WriteStartElement("names");
			this.Names.ForEach(item => item.ToXml(writer));
			writer.WriteEndElement(); // names
			writer.WriteStartElement("contacts");
			this.Contacts.ForEach(item => item.ToXml(writer));
			writer.WriteEndElement(); // contacts
			writer.WriteStartElement("addresses");
			this.Addresses.ForEach(item => item.ToXml(writer));
			writer.WriteEndElement(); // addresses
			writer.WriteEndElement(); // agent
		}

		public override void ToJson(JsonWriter writer) {
			writer.WriteStartObject();
			writer.WritePropertyName("_type");
			writer.WriteValue("agent");

			base.ContentToJson(writer);

			writer.WritePropertyName("names");
			writer.WriteStartArray();
			this.Names.ForEach(item => item.ToJson(writer));
			writer.WriteEndArray();

			writer.WritePropertyName("contacts");
			writer.WriteStartArray();
			this.Contacts.ForEach(item => item.ToJson(writer));
			writer.WriteEndArray();

			writer.WritePropertyName("addresses");
			writer.WriteStartArray();
			this.Addresses.ForEach(item => item.ToJson(writer));
			writer.WriteEndArray();

			writer.WriteEndObject();
		}

		public override string ToString() {
			return this.ToJson();
		}

		public new class Builder : Node.Builder, IConsumeData<Builder, Agent> {

			public static implicit operator Agent(Builder builder) {
				return builder.ToConcrete();
			}

			public static implicit operator Builder(Agent agent) {
				return new Builder(agent);
			}

			public static ImmutableHashSet<Agent> CreateImmutableCollection(IEnumerable<Agent.Builder> meta) {
				return meta.Select(builder => builder.ToConcrete()).ToImmutableHashSet();
			}

			public HashSet<Metadata.Builder> Names { get; set; }
			public HashSet<Metadata.Builder> Contacts { get; set; }
			public HashSet<Metadata.Builder> Addresses { get; set; }

			public Builder() {
				this.Names = new HashSet<Metadata.Builder>();
				this.Contacts = new HashSet<Metadata.Builder>();
				this.Addresses = new HashSet<Metadata.Builder>();
			}

			public Builder(string id, IEnumerable<Metadata.Builder> meta, IEnumerable<Metadata.Builder> names, IEnumerable<Metadata.Builder> contacts,
				IEnumerable<Metadata.Builder> addresses)
				: base(id, meta) {
					this.Names = (names == null) ? new HashSet<Metadata.Builder>() : new HashSet<Metadata.Builder>(names);
					this.Contacts = (contacts == null) ? new HashSet<Metadata.Builder>() : new HashSet<Metadata.Builder>(contacts);
					this.Addresses = (addresses == null) ? new HashSet<Metadata.Builder>() : new HashSet<Metadata.Builder>(addresses);
			}

			public Builder(Agent other) {
				this.FromConcrete(other);
			}

			public Builder FromConcrete(Agent other) {
				base.FromNode(other);
				this.Names = new HashSet<Metadata.Builder>(other.Names.Select(item => (Metadata.Builder)item));
				this.Contacts = new HashSet<Metadata.Builder>(other.Contacts.Select(item => (Metadata.Builder)item));
				this.Addresses = new HashSet<Metadata.Builder>(other.Addresses.Select(item => (Metadata.Builder)item));
				return this;
			}

			public Agent ToConcrete() {
				return new Agent(this.Id, this.Metadata.Cast<Metadata>(), this.Names.Cast<Metadata>(), this.Contacts.Cast<Metadata>(), this.Addresses.Cast<Metadata>());
			}

			public Builder FromJson(JObject json) {
				if (json["_type"].Value<string>() != "scopedData") throw new InvalidOperationException("The json being used does not represent the type it is being read into.");

				this.Id = json["id"].Value<string>();
				this.Metadata = new HashSet<Metadata.Builder>(json["metadata"].Value<JArray>().Select(obj => new Metadata.Builder(obj.Value<JObject>())));
				this.Names = new HashSet<Metadata.Builder>(json["names"].Value<JArray>().Select(obj => new Metadata.Builder(obj.Value<JObject>())));
				this.Contacts = new HashSet<Metadata.Builder>(json["contacts"].Value<JArray>().Select(obj => new Metadata.Builder(obj.Value<JObject>())));
				this.Addresses = new HashSet<Metadata.Builder>(json["addresses"].Value<JArray>().Select(obj => new Metadata.Builder(obj.Value<JObject>())));

				return this;
			}
			
		}
	}
}
