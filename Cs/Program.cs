using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Cs
{
	public static class Program
	{
		private static int Main(string[] args)
		{
			return Task.Run(async () => await RunAsync(args[0])).GetAwaiter().GetResult();
		}

		private static async Task<int> RunAsync(string path)
		{
			var code = await LoadAndSanitizeCodeAsync(path);

			try
			{
				var state = await CSharpScript.RunAsync(code);

				return GetExitCodeFromScriptState(state);
			}
			catch (CompilationErrorException e)
			{
				Console.WriteLine(e.Message);

				return 1;
			}
		}

		private static async Task<string> LoadAndSanitizeCodeAsync(string path)
		{
			var lines = await File.ReadAllLinesAsync(path);
			var sanitizedLines = SanitizeLines(lines);
			var code = string.Join(Environment.NewLine, sanitizedLines);

			return code;
		}

		// Sanitize lines beginning #! by commenting them out.
		// Commenting out ensures source line numbers match stack traces.
		private static IEnumerable<string> SanitizeLines(IEnumerable<string> lines)
		{
			foreach (var line in lines)
			{
				var sanitizedLine = line;

				if (line.StartsWith("#!"))
				{
					sanitizedLine = $"//{line}";
				}

				yield return sanitizedLine;
			}
		}

		private static int GetExitCodeFromScriptState(ScriptState<object> state)
		{
			const int success = 0;
			const int error = 1;

			// If the script doesn't return a value then treat this as a success
			if (state.ReturnValue == null)
			{
				return success;
			}

			// If the script return value is an integer then it should be the exit code
			if (state.ReturnValue is int result)
			{
				return result;
			}

			// Any non-integer status is treated as an error exit code
			return error;
		}
	}
}
