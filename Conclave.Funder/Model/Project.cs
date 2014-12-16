using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using Inversion;
using Conclave.Map.Model;

namespace Conclave.Funder.Model {
	public class Project: Node, IData {

		private int _hashcode = 0;
		private JObject _data;

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

		public Project(Node node) : base(node) {}
		public Project(string id, IEnumerable<Metadata> meta) : base(id, meta) {}
		public Project(string id, IEnumerable<Metadata.Builder> meta) : base(id, meta) {}
		public Project(string id) : base(id) {}
		public Project(Project other) : this(other.Id, other.Metadata) {}

		object ICloneable.Clone() {
			return new Project(this);
		}
		public Project Clone() {
			return new Project(this);
		}
	}
}
