using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CSharp;

namespace Jones
{
  public class ModelBuilder
  {
    public Assembly GenerateModel(string sourcefile)
    {
      var source = sourcefile;
      var metaData = XDocument.Load(source);
      
      var nameSpace = GetXmlNodeAttributeValue(metaData.Root, "nameSpace", "Jones.Model");

      var compileUnit = new CodeCompileUnit();
      var codeNameSpace = new CodeNamespace(nameSpace);
      codeNameSpace.Imports.Add(new CodeNamespaceImport("System"));
      compileUnit.Namespaces.Add(codeNameSpace);

      foreach (var c in from c in metaData.Root?.Descendants("class") select c) {
        GenerateClass(c, codeNameSpace);
      }

      return CompileCSharpCode(compileUnit);
    }

    private void GenerateClass(XElement classNode, CodeNamespace nameSpace)
    {
      var typeName = GetXmlNodeAttributeValue(classNode, "name", $"class{nameSpace.Types.Count}");
      var typeObject = new CodeTypeDeclaration(typeName);
      nameSpace.Types.Add(typeObject);

      foreach (var prop in from p in classNode?.Descendants("property") select p) {
        GenerateProperty(prop, typeObject);
      }
    }

    private void GenerateProperty(XElement property, CodeTypeDeclaration typeObject)
    {
      var type = Type.GetType(GetXmlNodeAttributeValue(property, "type", "string")) ?? typeof(string);
      var name = GetXmlNodeAttributeValue(property, "name", $"property{property.ElementsBeforeSelf().Count()}");

      // Add underlying field
      var valueField = new CodeMemberField {
        Attributes = MemberAttributes.Private,
        Name = $"_{name}Value",
        Type = new CodeTypeReference(type)
      };
      valueField.Comments.Add(new CodeCommentStatement($"The underlying {name} field for corresponding property."));
      typeObject.Members.Add(valueField);

      // Add property
      var propObject = new CodeMemberProperty {
        Name = name,
        HasGet = true,
        Type = new CodeTypeReference(type),
        Attributes = MemberAttributes.Public
      };
      propObject.Comments.Add(new CodeCommentStatement($"The {name} property for the {typeObject.Name} class."));
      propObject.GetStatements.Add(new CodeMethodReturnStatement(
          new CodeFieldReferenceExpression(
          new CodeThisReferenceExpression(), $"_{name}Value")));

      typeObject.Members.Add(propObject);
    }

    private Assembly CompileCSharpCode(CodeCompileUnit source)
    {
      var provider = new CSharpCodeProvider();

      var cp = new CompilerParameters();
      cp.ReferencedAssemblies.Add("System.dll");
      cp.GenerateExecutable = false;
      cp.GenerateInMemory = true;

      // Compile the code DOM
      var cr = provider.CompileAssemblyFromDom(cp, source);

      if (cr.Errors.Count <= 0) {
        return cr.CompiledAssembly;
      }

      var message = new StringBuilder();
      foreach (CompilerError err in cr.Errors) {
        message.AppendLine(err.ErrorText);
      }

      throw new Exception($"Compilation had {cr.Errors.Count} errors: {message}");
    }

    private string GetXmlNodeAttributeValue(XElement node, string attribute, string defaultValue)
    {
      return node.Attributes(attribute).FirstOrDefault()?.Value ?? defaultValue;
    }
  }
}
