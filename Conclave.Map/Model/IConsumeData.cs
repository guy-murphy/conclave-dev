using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Conclave.Map.Model {
	public interface IConsumeData<TBuilder, TConcrete> {
		TBuilder FromJson(JObject json);
		TConcrete ToConcrete();
	}
}
