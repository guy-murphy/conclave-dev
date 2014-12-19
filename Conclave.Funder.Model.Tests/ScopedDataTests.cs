using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Inversion;
using Newtonsoft.Json.Linq;

namespace Conclave.Funder.Model.Tests {
	[TestClass]
	public class ScopedDataTests {
		[TestMethod]
		public void JsonUsageTest() {
			const string originalJson = @"{
				  ""_type"": ""scopedData"",
				  ""id"": ""id-0"",
				  ""for"": ""parent-0"",
				  ""scope"": ""scope-0"",
				  ""name"": ""name-0"",
				  ""value"": ""value-0""
			}";
			const string otherJson = @"{
				  ""_type"": ""scopedData"",
				  ""id"": ""id-1"",
				  ""for"": ""parent-1"",
				  ""scope"": ""scope-1"",
				  ""name"": ""name-1"",
				  ""value"": ""value-1""
			}";
			JObject data = JObject.Parse(originalJson);
			JObject other = JObject.Parse(otherJson);

			ScopedData sd0 = new ScopedData.Builder(data);
			ScopedData sd1 = new ScopedData("id-0", "parent-0", "scope-0", "name-0", "value-0");

			string producedJson = sd1.ToJson();
			ScopedData sd2 = new ScopedData.Builder(JObject.Parse(producedJson));

			Assert.AreEqual(sd0, sd1);
			Assert.AreEqual(sd0, sd2);
			Assert.AreEqual(sd1, sd2);

			Assert.IsTrue(JToken.DeepEquals(data, sd2.Data));

			ScopedData sd3 = sd2.Mutate(sd => {
				sd.Id = "id-1";
				sd.Parent = "parent-1";
				sd.Scope = "scope-1";
				sd.Name = "name-1";
				sd.Value = "value-1";
				return sd;
			});

			Assert.AreEqual(sd2, sd0); // confirm sd2 hasn't actually changed
			Assert.AreNotEqual(sd2, sd3);

			Assert.IsTrue(JToken.DeepEquals(other, sd3.Data));
		}
	}
}
