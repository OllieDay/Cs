using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

		public int Exec(string path, params object[] args)
		{
			using (var process = Process.Start(path, BuildArgs(args)))
			{
				process.WaitForExit();

				return process.ExitCode;
			}
		}

		public Task<int> ExecAsync(string path, params object[] args)
		{
			return ExecAsync(path, default(CancellationToken), args);
		}

		public Task<int> ExecAsync(string path, CancellationToken cancellationToken, params object[] args)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = path,
					Arguments = BuildArgs(args)
				},
				EnableRaisingEvents = true
			};

			var taskCompletionSource = new TaskCompletionSource<int>();

			var cancellationTokenRegistration = cancellationToken.Register(() =>
			{
				taskCompletionSource.SetCanceled();
				process.Kill();
				process.Dispose();
			});

			process.Exited += (sender, e) =>
			{
				taskCompletionSource.TrySetResult(process.ExitCode);
				process.Dispose();
			};

			process.Start();

			return taskCompletionSource.Task;
		}

		private static string BuildArgs(object[] args)
		{
			// Arguments should be quoted in case they contain spaces
			return string.Join(' ', args.Select(x => $@"""{x}"""));
		}
	}
}
