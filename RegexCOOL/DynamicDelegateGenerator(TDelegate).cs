using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;


namespace CB.RegexCOOL
{
    public class DynamicDelegateGenerator<TDelegate> where TDelegate: class
    {
        #region Fields
        private const string INVOKE_METHOD = "Invoke";

        // ReSharper disable once StaticMemberInGenericType
        private static readonly string[] _defaultReferencedAssemblies =
        {
            "Microsoft.CSharp.dll",
            "System.dll",
            "System.Core.dll",
            "System.Data.dll",
            "System.Linq.dll"
        };
        #endregion


        #region  Constructors & Destructors
        static DynamicDelegateGenerator()
        {
            var type = typeof(TDelegate);
            if (!type.IsSubclassOf(typeof(Delegate)))
            {
                throw new InvalidOperationException(type.Name + " is not a Delegate type.");
            }
        }
        #endregion


        #region Methods
        public static TDelegate CreateDelegateFromBody(
            string delegateBody, params string[] referencedAssemblies)
        {
            var source = CreateSource(delegateBody);
            return CreateDelegateFromSource(source, referencedAssemblies);
        }

        public static TDelegate CreateDelegateFromFile(
            string filePath, params string[] referencedAssemblies)
        {
            using (var sr = new StreamReader(filePath))
            {
                return CreateDelegateFromStreamReader(sr, referencedAssemblies);
            }
        }

        public static TDelegate CreateDelegateFromSource(
            string source, params string[] referencedAssemblies)
        {
            var results = Compile(source, referencedAssemblies);

            if (results.Errors.Count != 0) throw CreateErrorException(results.Errors);

            var assembly = results.CompiledAssembly;
            var type = assembly.DefinedTypes.First();
            return CreateDelegateFromMethod(type.GetMethod(INVOKE_METHOD));
        }

        public static TDelegate CreateDelegateFromStream(
            Stream stream, params string[] referencedAssemblies)
        {
            using (var sr = new StreamReader(stream))
            {
                return CreateDelegateFromStreamReader(sr, referencedAssemblies);
            }
        }

        public static string GenerateCode()
        {
            return CreateSource("");
        }

        public static IEnumerable<string> GetDefaultReferenceAssemblies()
        {
            var tDelegateAssembly = typeof(TDelegate).Assembly.ManifestModule.Name;
            var defaultReferencedAssemblies= new SortedSet<string>(_defaultReferencedAssemblies);

            if (!defaultReferencedAssemblies.Contains(tDelegateAssembly))
            {
                defaultReferencedAssemblies.Add(tDelegateAssembly);
            }
            return defaultReferencedAssemblies;
        }
        #endregion


        #region Implementation
        private static void AddDefaultAssemblies(StringCollection referencedAssemblies)
        {
            referencedAssemblies.AddRange(GetDefaultReferenceAssemblies().ToArray());
        }

        private static CompilerResults Compile(string source, string[] referencedAssemblies)
        {
            var codeProvider = new CSharpCodeProvider();
            var compilerOptions = GetCompilerOptions(referencedAssemblies);
            var results = codeProvider.CompileAssemblyFromSource(compilerOptions, source);
            return results;
        }

        private static TDelegate CreateDelegateFromMethod(MethodInfo method)
        {
            return method?.CreateDelegate(typeof(TDelegate)) as TDelegate;
        }

        private static TDelegate CreateDelegateFromStreamReader(
            StreamReader sr, string[] referencedAssemblies)
        {
            var source = sr.ReadToEnd();
            return CreateDelegateFromSource(source, referencedAssemblies);
        }

        private static Exception CreateErrorException(IEnumerable errors)
        {
            var message = string.Join(Environment.NewLine + "    ", errors.Cast<CompilerError>());
            return new Exception(message);
        }

        private static string CreateMethodSignature()
        {
            var delegateType = typeof(TDelegate);
            var invokeMethod = delegateType.GetMethod(INVOKE_METHOD);
            var parameterInfos = invokeMethod.GetParameters();
            var parameters = string.Join(
                ", ", parameterInfos.Select(p => $@"{p.ParameterType} {p.Name}"));
            var methodSignature = $@"{invokeMethod.ReturnType} {INVOKE_METHOD}({parameters})";
            return methodSignature;
        }

        private static string CreateSource(string delegateBody)
        {
            var format = File.ReadAllText("source.txt");
            var methodSignature = CreateMethodSignature();
            var source = string.Format(format, methodSignature, delegateBody);
            return source;
        }

        private static CompilerParameters GetCompilerOptions(string[] referencedAssemblies)
        {
            var compilerOptions = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                WarningLevel = 3
            };

            AddDefaultAssemblies(compilerOptions.ReferencedAssemblies);
            compilerOptions.ReferencedAssemblies.AddRange(referencedAssemblies);
            return compilerOptions;
        }
        #endregion
    }
}