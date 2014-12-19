using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json.Linq;

using Inversion;
using Conclave.Map.Model;

namespace Conclave.Funder.Model {
	public class Project: Node, IData {

		private readonly ImmutableHashSet<Goal> _goals;
		private readonly ImmutableHashSet<JournalEntry> _summaries;
		private readonly ImmutableHashSet<Locator> _locators;

		//private int _hashcode = 0;
		private JObject _data;

		public IEnumerable<Goal> Goals {
			get { return _goals; }
		}

		public IEnumerable<JournalEntry> Summaries {
			get { return _summaries; }
		}

		public IEnumerable<Locator> Locators {
			get { return _locators; }
		}

		public Goal CurrentGoal {
			get { return this.Goals.FirstOrDefault(g => g.Next == String.Empty); }
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

		public Project(string id, IEnumerable<Metadata> meta, IEnumerable<JournalEntry> summaries, IEnumerable<Locator> locators) : base(id, meta) {
			_summaries = (summaries == null) ? ImmutableHashSet.Create<JournalEntry>() : ImmutableHashSet.Create<JournalEntry>(summaries.ToArray());
			_locators = (locators == null) ? ImmutableHashSet.Create<Locator>() : ImmutableHashSet.Create<Locator>(locators.ToArray());
		}

		public Project(Project other) : this(other.Id, other.Metadata, other.Summaries, other.Locators) {}

		object ICloneable.Clone() {
			return new Project(this);
		}
		public Project Clone() {
			return new Project(this);
		}
	}
}
