using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

using Inversion;
using Conclave.Map.Model;

namespace Conclave.Funder.Model.Tests {

	[TestClass]
	public class ScopedDataTests {

		public static void GenericTest<TBuilder, TConcrete>(string source, string target, Func<TBuilder, TConcrete> mutator, TConcrete exemplar)
			where TBuilder : IConsumeData<TBuilder, TConcrete>, new()
			where TConcrete : class, IData, IMutate<TBuilder, TConcrete> 
		{
			JObject sourceModel = JObject.Parse(source);
			JObject targetModel = JObject.Parse(target);

			TConcrete sdSource = new TBuilder().FromJson(sourceModel).ToConcrete();

			string producedJson = exemplar.ToJson();
			TConcrete sdProduced = new TBuilder().FromJson(JObject.Parse(producedJson)).ToConcrete();

			Assert.AreEqual(sdSource, exemplar);
			Assert.AreEqual(sdSource, sdProduced);
			Assert.AreEqual(exemplar, sdProduced);

			TConcrete sdMutated = sdProduced.Mutate(mutator);

			Assert.AreEqual(sdProduced, sdSource); // confirm sdProduced hasn't actually changed
			Assert.AreNotEqual(sdProduced, sdMutated);

			Assert.IsTrue(JToken.DeepEquals(targetModel, sdMutated.Data));
		}

		[TestMethod]
		public void JsonUsageTest() {
			GenericTest<ScopedData.Builder, ScopedData>(
				source: @"{
					  ""_type"": ""scopedData"",
					  ""id"": ""id-0"",
					  ""for"": ""parent-0"",
					  ""scope"": ""scope-0"",
					  ""name"": ""name-0"",
					  ""value"": ""value-0""
				}",
				target: @"{
					""_type"": ""scopedData"",
					""id"": ""id-1"",
					""for"": ""parent-1"",
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
	}
}
