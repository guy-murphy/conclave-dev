using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Conclave.Map.Model {
	public interface IConsumeData<out TBuilder, TConcrete> {
		TBuilder FromConcrete(TConcrete other);
		TBuilder FromJson(JObject json);
		TConcrete ToConcrete();
	}
}
