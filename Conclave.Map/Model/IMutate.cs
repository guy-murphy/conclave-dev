using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conclave.Map.Model {
	public interface IMutate<out TBuilder, TConcrete> {
		TConcrete Mutate(Func<TBuilder, TConcrete> mutator);	
	}
}
