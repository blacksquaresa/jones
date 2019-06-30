using System;
using System.Collections.Generic;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace Jones.DotNet.Cli
{
  [Command(Name = "compile", Description = "Compile the current project file into a project")]
  public class CompileCommand
  {    public void OnExecute()
    {
      Console.WriteLine($"Jones compiled");
    }
  }
}
