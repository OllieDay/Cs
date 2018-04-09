using System;
using System.Collections.Generic;
using System.Linq;

namespace Cs
{
	public sealed class Globals
	{
		// Skip first arg as it is /usr/local/Cs/Cs.dll
		public string[] Args => Environment.GetCommandLineArgs().Skip(1).ToArray();

		public IDictionary<string, string> Env
		{
			get
			{
				var environmentVariables = Environment.GetEnvironmentVariables();

				return environmentVariables.Keys
					.Cast<string>()
					.ToDictionary(key => key, key => (string)environmentVariables[key]);
			}
		}
	}
}
