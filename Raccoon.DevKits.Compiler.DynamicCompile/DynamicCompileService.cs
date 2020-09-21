using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.DevKits.Compiler.DynamicCompile
{
    public class DynamicCompileService
    {
        #region private
        private readonly CSharpCompilationOptions _compilationOptions = new(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release);

        private CSharpCompilation CreateCompilation(string assemblyName, 
            IEnumerable<string> codes,params Assembly[] referenceAssemblies)=>
            CSharpCompilation.Create(assemblyName)
                .WithReferences(referenceAssemblies.Select(assembly => MetadataReference.CreateFromFile(assembly.Location)))
                .WithOptions(_compilationOptions)
                .AddSyntaxTrees(codes.Select(code => CSharpSyntaxTree.ParseText(code)));

        private CSharpCompilation CreateCompilation(string assemblyName,
            string code, params Assembly[] referenceAssemblies)=>
            CSharpCompilation.Create(assemblyName)
                .WithReferences(referenceAssemblies.Select(assembly => MetadataReference.CreateFromFile(assembly.Location)))
                .WithOptions(_compilationOptions)
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(code));

        private void CheckEmitResult(EmitResult emitResult)
        {
            if (!emitResult.Success)
            {
                IEnumerable<string> diagnostics = emitResult.Diagnostics.Select(item => item.ToString());
                StringBuilder builder = new();
                foreach (string diagnostic in diagnostics) { builder.AppendLine(diagnostic); }
                throw new InvalidOperationException(builder.ToString());
            }
        }
        #endregion

        public Assembly Compile(string assemblyName,string code, params Assembly[] referenceAssemblies)
        {
            MemoryStream stream = new();
            CSharpCompilation compilation = CreateCompilation(assemblyName, code, referenceAssemblies);
            CheckEmitResult(compilation.Emit(stream));
            stream.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(stream.GetBuffer());
        }

        public Assembly Compile(string assemblyName,IEnumerable<string> codes,params Assembly[] referenceAssemblies)
        {
            MemoryStream stream = new();
            CSharpCompilation compilation = CreateCompilation(assemblyName, codes, referenceAssemblies);
            CheckEmitResult(compilation.Emit(stream));
            stream.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(stream.GetBuffer());
        }
    }
}
