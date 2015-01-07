using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Inversion;

namespace Conclave.Funder.Model {
	public class Locator : IData, IEquatable<Locator> {

		public static bool operator ==(Locator o1, Locator o2) {
			if (Object.ReferenceEquals(o1, o2)) return true;
			if (((object)o1 == null) || ((object)o2 == null)) return false;
			return o1.Equals(o2);
		}

		public static bool operator !=(Locator o1, Locator o2) {
			return !(o1 == o2);
		}

		private static readonly Locator _blank = new Locator(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty);

		public static Locator Blank {
			get { return _blank; }
		}

		private readonly string _id;
		private readonly string _parent;
		private readonly string _scope;
		private readonly string _role;
		private readonly string _reference;

		private int _hashcode = 0;
		private JObject _data;

		public string Id {
			get { return _id; }
		}
		public string Parent {
			get { return _parent; }
		}
		public string Scope {
			get { return _scope; }
		}
		public string Role {
			get { return _role; }
		}
		public string Reference {
			get { return _reference; }
		}

		public JObject Data {
			get { return _data ?? (_data = this.ToJsonObject()); }
		}

		public Locator(string parent, string role, string reference) : this(parent, "default", role, reference) { }
		public Locator(string parent, string scope, string role, string reference) : this(Guid.NewGuid().ToString(), parent, scope, role, reference) { }

		public Locator(string id, string parent, string scope, string role, string reference) {
			if (id == null) throw new ArgumentNullException("id");
			if (parent == null) throw new ArgumentNullException("parent");
			if (scope == null) throw new ArgumentNullException("scope");
			if (role == null) throw new ArgumentNullException("role");
			if (reference == null) throw new ArgumentNullException("reference");

			_id = id;
			_parent = parent;
			_scope = scope;
			_role = role;
			_reference = reference;
		}

		public Locator(Locator other) : this(other.Id, other.Parent, other.Scope, other.Role, other.Reference) { }

		object ICloneable.Clone() {
			return new Locator(this);
		}

		public Locator Clone() {
			return new Locator(this);
		}

		public override bool Equals(object obj) {
			Locator other = obj as Locator;
			return (other != null) && this.Equals(other);
		}

		public bool Equals(Locator other) {
			if (Object.ReferenceEquals(this, other)) return true;
			return (
				this.Id == other.Id &&
					this.Parent == other.Parent &&
					this.Scope == other.Scope &&
					this.Role == other.Role &&
					this.Reference == other.Reference
				);
		}

		public override int GetHashCode() {
			if (_hashcode == 0) {
				int hc = "Locator".GetHashCode();
				hc = hc * 31 + this.Id.GetHashCode();
				hc = hc * 31 + this.Parent.GetHashCode();
				hc = hc * 31 + this.Scope.GetHashCode();
				hc = hc * 31 + this.Role.GetHashCode();
				hc = hc * 31 + this.Reference.GetHashCode();
				_hashcode = hc;
			}
			return _hashcode;
		}

		public Locator Mutate(Func<Builder, Locator> mutator) {
			Builder builder = new Builder(this);
			return mutator(builder);
		}

		public void ToXml(XmlWriter writer) {
			writer.WriteStartElement("locator");
			writer.WriteAttributeString("id", this.Id);
			writer.WriteAttributeString("parent", this.Parent);
			writer.WriteAttributeString("scope", this.Scope);
			writer.WriteAttributeString("role", this.Role);
			writer.WriteAttributeString("reference", this.Reference);
			writer.WriteEndElement(); // locator
		}

		public void ToJson(JsonWriter writer) {
			writer.WriteStartObject();
			writer.WritePropertyName("_type");
			writer.WriteValue("locator");
			writer.WritePropertyName("id");
			writer.WriteValue(this.Id);
			writer.WritePropertyName("parent");
			writer.WriteValue(this.Parent);
			writer.WritePropertyName("scope");
			writer.WriteValue(this.Scope);
			writer.WritePropertyName("role");
			writer.WriteValue(this.Role);
			writer.WritePropertyName("reference");
			writer.WriteValue(this.Reference);
			writer.WriteEndObject();
		}

		public class Builder {

			public static implicit operator Locator(Builder builder) {
				return builder.ToLocator();
			}

			public static implicit operator Builder(Locator loc) {
				return new Builder(loc);
			}

			public static ImmutableHashSet<Locator> CreateImmutableCollection(IEnumerable<Locator.Builder> locs) {
				return locs.Select(builder => builder.ToLocator()).ToImmutableHashSet();
			}

			public string Id { get; set; }
			public string Parent { get; set; }
			public string Scope { get; set; }
			public string Role { get; set; }
			public string Reference { get; set; }


			public Builder() : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty) {}
			public Builder(string parent, string scope, string role, string reference) : this(Guid.NewGuid().ToString(), parent, scope, role, reference) { }

			public Builder(string id, string parent, string scope, string role, string reference) {
				this.Id = id;
				this.Parent = parent;
				this.Scope = scope;
				this.Role = role;
				this.Reference = reference;
			}

			public Builder(Locator loc) {
				this.FromLocator(loc);
			}

			public Builder FromLocator(Locator other) {
				this.Id = other.Id;
				this.Parent = other.Parent;
				this.Scope = other.Scope;
				this.Role = other.Role;
				this.Reference = other.Reference;
				return this;
			}

			public Locator ToLocator() {
				return new Locator(this.Id, this.Parent, this.Scope, this.Role, this.Reference);
			}

		}

	}
}
