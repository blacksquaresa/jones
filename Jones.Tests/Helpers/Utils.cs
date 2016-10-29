using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework.Interfaces;

namespace Jones.Tests.Helpers
{
  public static class Utils
  {
    public static string GetFilePath(string source)
    {
      var root = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.FullName;
      return $@"{root}\{source}";
    }
  }
}
