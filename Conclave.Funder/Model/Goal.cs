using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Inversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Conclave.Funder.Model {
	public class Goal : IData, IEquatable<Goal> {

		public static bool operator ==(Goal o1, Goal o2) {
			if (Object.ReferenceEquals(o1, o2)) return true;
			if (((object)o1 == null) || ((object)o2 == null)) return false;
			return o1.Equals(o2);
		}

		public static bool operator !=(Goal o1, Goal o2) {
			return !(o1 == o2);
		}

		private static readonly Goal _blank = new Goal(String.Empty, String.Empty, default(DateTime), default(DateTime), default(decimal));

		public static Goal Blank {
			get { return _blank; }
		}

		private readonly string _id;
		private readonly string _parent;
		private readonly string _next;
		private readonly string _previous;
		private readonly DateTime _start;
		private readonly DateTime _end;
		private readonly decimal _amount;

		private int _hashcode = 0;
		private JObject _data;

		public string Id {
			get { return _id; }
		}
		public string Parent {
			get { return _parent; }
		}
		public string Next {
			get { return _next; }
		}
		public string Previous {
			get { return _previous; }
		}

		public DateTime Start {
			get { return _start; }
		}
		public DateTime End {
			get { return _end; }
		}

		public decimal Amount {
			get { return _amount; }
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

		public Goal(Goal other) : this(other.Id, other.Parent, other.Previous, other.Next, other.Start, other.End, other.Amount) { }

		public Goal(string id, string parent, DateTime start, DateTime end, decimal amount)
			: this(id, parent, String.Empty, String.Empty, start, end, amount) { }

		public Goal(string id, string parent, string previous, string next, DateTime start, DateTime end, decimal amount) {
			if (id == null) throw new ArgumentNullException("id");
			if (parent == null) throw new ArgumentNullException("parent");
			if (start == default(DateTime)) throw new ArgumentNullException("start");
			if (end == default(DateTime)) throw new ArgumentNullException("end");
			if (amount == default(decimal)) throw new ArgumentNullException("amount");
			// null values for pervious and next are permitted

			_id = id;
			_parent = parent;
			_previous = previous ?? String.Empty;
			_next = next ?? String.Empty;
			_start = start;
			_end = end;
			_amount = amount;
		}

		object ICloneable.Clone() {
			return new Goal(this);
		}

		public Goal Clone() {
			return new Goal(this);
		}

		public override bool Equals(object obj) {
			Goal other = obj as Goal;
			return (other != null) && this.Equals(other);
		}

		public bool Equals(Goal other) {
			if (Object.ReferenceEquals(this, other)) return true;
			return (
				this.Id == other.Id &&
					this.Parent == other.Parent &&
					this.Previous == other.Previous &&
					this.Next == other.Next &&
					this.Start == other.Start &&
					this.End == other.End &&
					this.Amount == other.Amount
				);
		}

		public override int GetHashCode() {
			if (_hashcode == 0) {
				int hc =  "Goal".GetHashCode();
				hc = hc * 31 + this.Id.GetHashCode();
				hc = hc * 31 + this.Parent.GetHashCode();
				hc = hc * 31 + this.Previous.GetHashCode();
				hc = hc * 31 + this.Next.GetHashCode();
				hc = hc * 31 + this.Start.GetHashCode();
				hc = hc * 31 + this.End.GetHashCode();
				hc = hc * 31 + this.Amount.GetHashCode();
				_hashcode = hc;
			}
			return _hashcode;
		}

		public Goal Mutate(Func<Builder, Goal> mutator) {
			Builder builder = new Builder(this);
			return mutator(builder);
		}

		/// <summary>
		/// Produces an xml representation of the model.
		/// </summary>
		/// <param name="writer">The writer to used to write the xml to. </param>
		public void ToXml(XmlWriter writer) {
			writer.WriteStartElement("goal");
			writer.WriteAttributeString("id", this.Id);
			writer.WriteAttributeString("for", this.Parent);
			writer.WriteAttributeString("previous", this.Previous);
			writer.WriteAttributeString("next", this.Next);
			writer.WriteAttributeString("start", this.Start.ToString());
			writer.WriteAttributeString("end", this.End.ToString());
			writer.WriteAttributeString("amount", this.Amount.ToString());
			writer.WriteEndElement(); // goal
		}

		/// <summary>
		/// Produces a json respresentation of the model.
		/// </summary>
		/// <param name="writer">The writer to use for producing json.</param>
		public void ToJson(JsonWriter writer) {
			writer.WriteStartObject();
			writer.WritePropertyName("_type");
			writer.WriteValue("goal");
			writer.WritePropertyName("id");
			writer.WriteValue(this.Id);
			writer.WritePropertyName("for");
			writer.WriteValue(this.Parent);
			writer.WritePropertyName("previous");
			writer.WriteValue(this.Previous);
			writer.WritePropertyName("next");
			writer.WriteValue(this.Next);
			writer.WritePropertyName("start");
			writer.WriteValue(this.Start.ToString());
			writer.WritePropertyName("end");
			writer.WriteValue(this.End.ToString());
			writer.WritePropertyName("amount");
			writer.WriteValue(this.Amount.ToString());
			writer.WriteEndObject();
		}

		public class Builder {
			public static implicit operator Goal(Builder builder) {
				return builder.ToGoal();
			}

			public static implicit operator Builder(Goal goal) {
				return new Builder(goal);
			}

			public static ImmutableHashSet<Goal> CreateImmutableCollection(IEnumerable<Goal.Builder> goal) {
				HashSet<Goal> temp = new HashSet<Goal>();
				if (goal != null) {
					foreach (Goal item in goal) {
						temp.Add(item);
					}
				}
				return ImmutableHashSet.Create<Goal>(temp.ToArray());
			}

			public string Id { get; set; }
			public string Parent { get; set; }
			public string Previous { get; set; }
			public string Next { get; set; }
			public DateTime Start { get; set; }
			public DateTime End { get; set; }
			public decimal Amount { get; set; }

			public Builder(): this(String.Empty, String.Empty, String.Empty, String.Empty, default(DateTime), default(DateTime), default(decimal)) { }

			public Builder(string id, string parent, DateTime start, DateTime end, decimal amount)
				: this(id, parent, String.Empty, String.Empty, start, end, amount) { }

			public Builder(string id, string parent, string previous, string next, DateTime start, DateTime end, decimal amount) {
				this.Id = id;
				this.Parent = parent;
				this.Previous = previous;
				this.Next = next;
				this.Start = start;
				this.End = end;
				this.Amount = amount;
			}

			public Builder(Goal goal) {
				this.FromGoal(goal);
			}

			public Builder FromGoal(Goal goal) {
				this.Id = goal.Id;
				this.Parent = goal.Parent;
				this.Previous = goal.Previous;
				this.Next = goal.Next;
				this.Start = goal.Start;
				this.End = goal.End;
				this.Amount = goal.Amount;
				return this;
			}

			public Goal ToGoal() {
				return new Goal(this.Id, this.Parent, this.Previous, this.Next, this.Start, this.End, this.Amount);
			}
		}

	}
}
