using System;
using System.Linq;
using System.Threading;
using System.Xml;

using System.Collections.Immutable;
using System.Collections.Generic;
using Inversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Conclave.Map.Model {
	public sealed class Metadata : IData, IEquatable<Metadata> {

		public static bool operator ==(Metadata m1, Metadata m2) {
			if (Object.ReferenceEquals(m1, m2)) return true;
			if (((object)m1 == null) || ((object)m2 == null)) return false;
			return m1.Equals(m2);
		}

		public static bool operator !=(Metadata m1, Metadata m2) {
			return !(m1 == m2);
		}

		private static readonly Metadata _blank = new Metadata(String.Empty, String.Empty, String.Empty, String.Empty);

		/// <summary>
		/// Represents an empty metadata item as
		/// an alternative to a <b>null</b> value.
		/// </summary>
		public static Metadata Blank {
			get { return _blank; }
		}

		private readonly string _parent;
		private readonly string _scope;
		private readonly string _name;
		private readonly string _value;

		// we memoize the result of GetHashCode()
		// which is only calculated if (_hashcode == 0)
		private int _hashcode = 0;
		private JObject _data;

		public string Parent {
			get { return _parent; }
		}

		public string Scope {
			get { return _scope; }
		}

		public string Name {
			get { return _name; }
		}

		public string Value {
			get { return _value; }
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

		public Metadata(string parent, string name, string value) : this(parent, "default", name, value) { }

		public Metadata(Metadata meta) : this(meta.Parent, meta.Scope, meta.Name, meta.Value) { }

		public Metadata(string parent, string scope, string name, string value) {
			_parent = parent;
			_scope = scope;
			_name = name;
			_value = value;
		}

		object ICloneable.Clone() {
			return new Metadata(this);
		}

		public Metadata Clone() {
			return new Metadata(this);
		}

		public override bool Equals(object obj) {
			Metadata other = obj as Metadata;
			return (other != null) && this.Equals(other);
		}

		public bool Equals(Metadata other) {
			if (object.ReferenceEquals(this, other)) return true;
			return (
				this.Parent == other.Parent &&
				this.Scope == other.Scope &&
				this.Name == other.Name &&
				this.Value == other.Value
			);
		}

		public override int GetHashCode() {
			if (_hashcode == 0) {
				int hc = 17;
				hc = hc * 31 + "Metadata".GetHashCode();
				hc = hc * 31 + this.Parent.GetHashCode();
				hc = hc * 31 + this.Scope.GetHashCode();
				hc = hc * 31 + this.Name.GetHashCode();
				hc = hc * 31 + this.Value.GetHashCode();
				_hashcode = hc;
			}
			return _hashcode;
		}

		public Metadata Mutate(Func<Builder, Metadata> mutator) {
			Builder builder = new Builder(this);
			return mutator(builder);
		}

		public void ContentToXml(XmlWriter writer) {
			writer.WriteAttributeString("parent", this.Parent);
			writer.WriteAttributeString("scope", this.Scope);
			writer.WriteAttributeString("name", this.Name);
			writer.WriteAttributeString("value", this.Value);
		}

		public void ToXml(XmlWriter writer) {
			writer.WriteStartElement("metadata");
			this.ContentToXml(writer);
			writer.WriteEndElement();
		}

		public void ContentToJson(JsonWriter writer) {
			writer.WritePropertyName("parent");
			writer.WriteValue(this.Parent);
			writer.WritePropertyName("scope");
			writer.WriteValue(this.Scope);
			writer.WritePropertyName("name");
			writer.WriteValue(this.Name);
			writer.WritePropertyName("value");
			writer.WriteValue(this.Value);
		}

		public void ToJson(JsonWriter writer) {
			writer.WriteStartObject();
			writer.WritePropertyName("_type");
			writer.WriteValue("metadata");
			this.ContentToJson(writer);
			writer.WriteEndObject();
		}

		/// <summary>
		/// The builder for <see cref="Metadata"/>, providing a mutable equivalent
		/// of the concrete, immutable model.
		/// </summary>
		public class Builder {

			/// <summary>
			/// Provides for an implicit cast from a <see cref="Metadata.Builder"/> object
			/// to a <see cref="Metadata"/> object.
			/// </summary>
			/// <param name="builder">The <see cref="Metadata.Builder"/> that is to be cast.</param>
			/// <returns>
			/// Returns a <see cref="Metadata"/> object with the same member values
			/// as the builder being cast from.
			/// </returns>
			public static implicit operator Metadata(Builder builder) {
				return builder.ToMetadata();
			}

			public static implicit operator Builder(Metadata metadata) {
				return new Builder(metadata);
			}

			public static ImmutableHashSet<Metadata> CreateImmutableCollection(IEnumerable<Metadata.Builder> meta) {
				HashSet<Metadata> temp = new HashSet<Metadata>();
				foreach (Metadata item in meta) {
					temp.Add(item);
				}
				return ImmutableHashSet.Create<Metadata>(temp.ToArray());
			}

			public string Parent { get; set; }
			public string Scope { get; set; }
			public string Name { get; set; }
			public string Value { get; set; }

			public Builder() {
				this.Scope = "default";
			}
			public Builder(string parent, string name, string value) : this(parent, "default", name, value) { }

			public Builder(Metadata metadata) {
				this.FromMetadata(metadata);
			}

			public Builder(JObject json) {
				this.FromJson(json);
			}

			public Builder(string parent, string scope, string name, string value) {
				this.Parent = parent;
				this.Scope = scope;
				this.Name = name;
				this.Value = value;
			}

			public Builder FromMetadata(Metadata metadata) {
				this.Parent = metadata.Parent;
				this.Scope = metadata.Scope;
				this.Name = metadata.Name;
				this.Value = metadata.Value;

				return this;
			}
			
			public Metadata ToMetadata() {
				return new Metadata(this.Parent, this.Scope, this.Name, this.Value);
			}

			public Builder FromJson(JObject json) {
				if (json["_type"].Value<string>() != "metadata") throw new InvalidOperationException("The json being used does not represent the type it is being read into.");

				this.Parent = json["parent"].Value<string>();
				this.Scope = json["scope"].Value<string>();
				this.Name = json["name"].Value<string>();
				this.Value = json["value"].Value<string>();

				return this;
			}
		}

	}
}
