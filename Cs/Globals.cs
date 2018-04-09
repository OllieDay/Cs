using System;
using System.Linq;

namespace Cs
{
	public sealed class Globals
	{
		// Skip first arg as it is /usr/local/Cs/Cs.dll
		public string[] Args => Environment.GetCommandLineArgs().Skip(1).ToArray();
	}
}
