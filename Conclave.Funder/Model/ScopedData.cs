﻿using System;
using System.Linq;
using System.Xml;

using System.Collections.Immutable;
using System.Collections.Generic;
using Inversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Conclave.Funder.Model {
	public class ScopedData : IData, IEquatable<ScopedData> {

		public static bool operator ==(ScopedData m1, ScopedData m2) {
			if (Object.ReferenceEquals(m1, m2)) return true;
			if (((object)m1 == null) || ((object)m2 == null)) return false;
			return m1.Equals(m2);
		}

		public static bool operator !=(ScopedData m1, ScopedData m2) {
			return !(m1 == m2);
		}

		private static readonly ScopedData _blank = new ScopedData(String.Empty, String.Empty, String.Empty, String.Empty);

		/// <summary>
		/// Represents an empty ScopedData item as
		/// an alternative to a <b>null</b> value.
		/// </summary>
		public static ScopedData Blank {
			get { return _blank; }
		}

		private readonly string _id;
		private readonly string _parent;
		private readonly string _scope;
		private readonly string _name;
		private readonly string _value;

		// we memoize the result of GetHashCode()
		// which is only calculated if (_hashcode == 0)
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
		public virtual JObject Data {
			get { return _data ?? (_data = this.ToJsonObject()); }
		}

		public ScopedData(string parent, string name, string value) : this(parent, "default", name, value) { }
		public ScopedData(string parent, string scope, string name, string value) : this(Guid.NewGuid().ToString(), parent, scope, name, value) { }
		
		public ScopedData(string id, string parent, string scope, string name, string value) {
			_id = id;
			_parent = parent;
			_scope = scope;
			_name = name;
			_value = value;
		}

		public ScopedData(ScopedData other) : this(other.Id, other.Parent, other.Scope, other.Name, other.Value) { }

		object ICloneable.Clone() {
			return new ScopedData(this);
		}

		public ScopedData Clone() {
			return new ScopedData(this);
		}

		public override bool Equals(object obj) {
			ScopedData other = obj as ScopedData;
			return (other != null) && this.Equals(other);
		}

		public bool Equals(ScopedData other) {
			if (object.ReferenceEquals(this, other)) return true;
			return (
				this.Id == other.Id &&
				this.Parent == other.Parent &&
				this.Scope == other.Scope &&
				this.Name == other.Name &&
				this.Value == other.Value
			);
		}

		public override int GetHashCode() {
			if (_hashcode == 0) {
				_hashcode = "ScopedData".GetHashCode();
				_hashcode = _hashcode * 31 + this.Id.GetHashCode();
				_hashcode = _hashcode * 31 + this.Parent.GetHashCode();
				_hashcode = _hashcode * 31 + this.Scope.GetHashCode();
				_hashcode = _hashcode * 31 + this.Name.GetHashCode();
				_hashcode = _hashcode * 31 + this.Value.GetHashCode();
			}
			return _hashcode;
		}

		public ScopedData Mutate(Func<Builder, ScopedData> mutator) {
			Builder builder = new Builder(this);
			return mutator(builder);
		}

		public virtual void ContentToXml(XmlWriter writer) {
			writer.WriteAttributeString("id", this.Id);
			writer.WriteAttributeString("for", this.Parent);
			writer.WriteAttributeString("scope", this.Scope);
			writer.WriteAttributeString("name", this.Name);
			writer.WriteAttributeString("value", this.Value);
		}

		public virtual void ToXml(XmlWriter writer) {
			writer.WriteStartElement("scoped-data");
			this.ContentToXml(writer);
			writer.WriteEndElement();
		}

		public virtual void ContentToJson(JsonWriter writer) {
			writer.WritePropertyName("id");
			writer.WriteValue(this.Id);
			writer.WritePropertyName("for");
			writer.WriteValue(this.Parent);
			writer.WritePropertyName("scope");
			writer.WriteValue(this.Scope);
			writer.WritePropertyName("name");
			writer.WriteValue(this.Name);
			writer.WritePropertyName("value");
			writer.WriteValue(this.Value);
		}

		public virtual void ToJson(JsonWriter writer) {
			writer.WriteStartObject();
			writer.WritePropertyName("_type");
			writer.WriteValue("scopedData");
			this.ContentToJson(writer);
			writer.WriteEndObject();
		}

		public class Builder {

			public static implicit operator ScopedData(Builder builder) {
				return builder.ToScopedData();
			}

			public static implicit operator Builder(ScopedData scopedData) {
				return new Builder(scopedData);
			}

			public static ImmutableHashSet<ScopedData> CreateImmutableCollection(IEnumerable<ScopedData.Builder> meta) {
				HashSet<ScopedData> temp = new HashSet<ScopedData>();
				foreach (ScopedData item in meta) {
					temp.Add(item);
				}
				return ImmutableHashSet.Create<ScopedData>(temp.ToArray());
			}

			public string Id { get; set; }
			public string Parent { get; set; }
			public string Scope { get; set; }
			public string Name { get; set; }
			public string Value { get; set; }

			public Builder() {
				this.Id = Guid.NewGuid().ToString();
				this.Scope = "default";
			}

			public Builder(ScopedData ScopedData) {
				this.FromScopedData(ScopedData);
			}

			public Builder(string parent, string name, string value) : this(parent, "default", name, value) { }
			public Builder(string parent, string scope, string name, string value) : this(Guid.NewGuid().ToString(), parent, scope, name, value) { }

			public Builder(string id, string parent, string scope, string name, string value) {
				this.Id = id;
				this.Parent = parent;
				this.Scope = scope;
				this.Name = name;
				this.Value = value;
			}

			public Builder FromScopedData(ScopedData other) {
				this.Id = other.Id;
				this.Parent = other.Parent;
				this.Scope = other.Scope;
				this.Name = other.Name;
				this.Value = other.Value;

				return this;
			}

			public ScopedData ToScopedData() {
				return new ScopedData(this.Id, this.Parent, this.Scope, this.Name, this.Value);
			}
		}

	}
}