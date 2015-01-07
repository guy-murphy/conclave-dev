using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

using Inversion;
using Conclave.Map.Model;

namespace Conclave.Funder.Model.Tests {

	/// <summary>
	/// <para>
	/// The purpose of this test is to ensure the functionality provided by `IData`, `IMutate`, and `IConsumeData`
	/// which are concerned with representing an object as JSON, yielding mutating state-change with immutable
	/// entities using the builder pattern, and the consuming of JSON object representations by builders
	/// to produce concrete immutable instances.
	/// </para>
	/// <para>
	/// The test is laid out as a generic test acting upon the types `TBuilder` which is constrained to being of type `IConsumeData` and
	/// a `TBuilder` type which is constrained to being `IData` and `IMutate`. We will perform our test touching only these interfaces and
	/// ensure that we can round-trip from JSON via a builder, to a concrete type and back again to JSON.
	/// </para>
	/// </summary>
	[TestClass]
	public class JsonUsageTests {

		public static void GenericTest<TBuilder, TConcrete>(string source, string target, Func<TBuilder, TConcrete> mutator, TConcrete exemplar)
			where TBuilder : IConsumeData<TBuilder, TConcrete>, new()
			where TConcrete : class, IData, IMutate<TBuilder, TConcrete> 
		{
			JObject sourceModel = JObject.Parse(source);
			JObject targetModel = JObject.Parse(target);

			TConcrete objSource = new TBuilder().FromJson(sourceModel).ToConcrete();

			string producedJson = exemplar.ToJson();
			TConcrete objProduced = new TBuilder().FromJson(JObject.Parse(producedJson)).ToConcrete();

			Assert.AreEqual(objSource, exemplar);
			Assert.AreEqual(objSource, objProduced);
			Assert.AreEqual(exemplar, objProduced);

			TConcrete objMutated = objProduced.Mutate(mutator);

			Assert.AreEqual(objProduced, objSource); // confirm objProduced hasn't actually changed
			Assert.AreNotEqual(objProduced, objMutated);

			Assert.IsTrue(JToken.DeepEquals(targetModel, objMutated.Data));
		}

		[TestMethod]
		public void ScopedData() {
			GenericTest<ScopedData.Builder, ScopedData>(
				source: @"{
					  ""_type"": ""scopedData"",
					  ""id"": ""id-0"",
					  ""parent"": ""parent-0"",
					  ""scope"": ""scope-0"",
					  ""name"": ""name-0"",
					  ""value"": ""value-0""
				}",
				target: @"{
					""_type"": ""scopedData"",
					""id"": ""id-1"",
					""parent"": ""parent-1"",
					""scope"": ""scope-1"",
					""name"": ""name-1"",
					""value"": ""value-1""
				}",
				 mutator: sd => {
					 sd.Id = "id-1";
					 sd.Parent = "parent-1";
					 sd.Scope = "scope-1";
					 sd.Name = "name-1";
					 sd.Value = "value-1";
					 return sd;
				},
				exemplar: new ScopedData("id-0", "parent-0", "scope-0", "name-0", "value-0")
			  );
		}

		[TestMethod]
		public void AgentScopedData() {
			GenericTest<AgentScopedData.Builder, AgentScopedData>(
				source: @"{
					""_type"": ""agentScopedData"",
					""id"": ""id-0"",
					""parent"": ""parent-0"",
					""who"": ""who-0"",
					""scope"": ""scope-0"",
					""name"": ""name-0"",
					""value"": ""value-0""
				}",
				target: @"{
					""_type"": ""agentScopedData"",
					""id"": ""id-1"",
					""parent"": ""parent-1"",
					""who"": ""who-1"",
					""scope"": ""scope-1"",
					""name"": ""name-1"",
					""value"": ""value-1""
				}",
				mutator: sd => {
					sd.Id = "id-1";
					sd.Parent = "parent-1";
					sd.Who = "who-1";
					sd.Scope = "scope-1";
					sd.Name = "name-1";
					sd.Value = "value-1";
					return sd;
				},
				exemplar: new AgentScopedData("id-0", "parent-0", "who-0", "scope-0", "name-0", "value-0")
			  );
		}
	}
}
