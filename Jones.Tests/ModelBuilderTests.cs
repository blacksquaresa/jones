using System;
using System.CodeDom;
using System.IO;
using System.Net.Mime;
using Jones.Templates;
using Jones.Tests.Helpers;
using NUnit.Framework;
using Shouldly;
using NSubstitute;

namespace Jones.Tests
{
  [TestFixture]
  public class ModelBuilderTests
  {
    [Test]
    public void TestMethod()
    {
      var builder = new ModelBuilder();
      var assembly = builder.GenerateModel(Utils.GetFilePath(@"SourceFiles\basic.xml"));

      var entityType = assembly.GetType("Jones.Models.Entity");
      entityType.ShouldNotBeNull();
      var nameProperty = entityType.GetProperty("Name");
      nameProperty.ShouldNotBeNull();
      nameProperty.GetMethod.IsPublic.ShouldBe(true);
      nameProperty.GetMethod.ReturnType.ShouldBe(typeof(string));
    }
  }
}
