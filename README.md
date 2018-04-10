# Cs

Create and execute shell scripts written in C#.

## Installation

```sh
$ sudo ./install.sh
```

## Usage

### Create a C# script

Create script files (e.g. `script.cs`); any file extension works.

```csharp
#!/usr/local/bin/cs
// ^ Required by the program loader to find the interpreter

// These imports are implicitly included in every script
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// Top-level functions
int Add(int x, int y)
{
    return x + y;
}

// Global variables
var a = 1;
var b = 2;

var result = Add(a, b);

// Write to stdout
Console.WriteLine($"{a} + {b} = {result}");

// Access script arguments through the global Args string array
var arg = Args[0];

// Access environment variables through the global Env dictionary
var env = Env["SOME_ENVIRONMENT_VARIABLE"];

// Execute commands and other scripts
Exec("ls");
Exec("some-other-script.cs", "some-arg", 42, DateTime.Now);

var exitCode = await ExecAsync("some-other-script.cs");

// Return an exit code (defaults to 0 if not specified)
return 1;
```

### Make the script executable

```sh
$ chmod +x ./script.cs
```

### Run it!

```sh
$ ./script.cs
```
