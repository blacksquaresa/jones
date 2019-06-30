using System;
using McMaster.Extensions.CommandLineUtils;

namespace Jones.DotNet.Cli
{
  [Subcommand(typeof(CompileCommand))]
  public class Program
  {
    public static int Main(string[] args)
      => CommandLineApplication.Execute<Program>(args);

    public void OnExecute() {
      Console.WriteLine($"Hello");
    }
  }
}
