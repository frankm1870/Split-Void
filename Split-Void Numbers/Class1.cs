using System;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Microsoft.CSharp;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System.Runtime.Loader;
using Fractions;
using System.Security.Cryptography.X509Certificates;

namespace Split_Void_Numbers
{
    public class SplitVoid
    {
        public double re;
        public double unf;

        public static readonly SplitVoid u = new SplitVoid(0, 1);

        public static SplitVoid? Parse(string exprOld)
        {
            List<string> refs = new() { "System", "Split_Void_Numbers", "System.Runtime"};
            string pattern = @"(?<=[\+\*\-\/ \(\,]|^)(?:(?:([0-9]+(?(?=\.)\.[0-9]+)|)v)|(0))";
            var expr = new StringBuilder(exprOld);
            var matches = Regex.Matches(expr.ToString(), pattern);
            int offset = 0;
            int group = 0;
            foreach (Match match in matches.Cast<Match>())
            {
                if (match.Groups[0].Value != "0")
                    group = 1;
                expr.Remove(match.Index + offset, match.Length).Insert(match.Index + offset, $"new SplitVoid(0, {(match.Groups[group].Value != "" ? match.Groups[group].Value : 1)})");
                offset += ($"new SplitVoid(0, {(match.Groups[group].Value != "" ? match.Groups[group].Value : 1)})".Length - match.Length);
                group = 0;
            }
            StringBuilder otherAssemblies = new();
            foreach(var asm in refs)
            {
                otherAssemblies.Append("using " + asm + ";\n");
            }
            var code = otherAssemblies.ToString() + @"
                namespace Temp
                {
                    public class TempClass
                    {
                        public static SplitVoid Function()
                        {
                            return " + expr + @";
                        }
                    }
                }";

            var referencePaths = new List<string>();
            var tree = CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.CSharp8));
            var root = tree.GetRoot() as CompilationUnitSyntax;
            var references = root!.Usings;
            string basePath = Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location)!;
            referencePaths.AddRange(references.Select(x => Path.Combine(basePath, $"{x.Name}.dll")));
            referencePaths.Add(typeof(SplitVoid).Assembly.Location);
            referencePaths.Add(typeof(object).Assembly.Location);
            referencePaths.RemoveDuplicates();
            var executableReferences = new List<PortableExecutableReference>();
            foreach (var reference in referencePaths)
            {
                if(File.Exists(reference))
                {
                    executableReferences.Add(MetadataReference.CreateFromFile(reference));
                }
            }

            var compilation = CSharpCompilation.Create(Path.GetRandomFileName(), new[] { tree }, executableReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            SplitVoid? result;
            using (var memStream = new MemoryStream())
            {
                EmitResult compilationResult = compilation.Emit(memStream);
                if(!compilationResult.Success)
                {
                    var errors = compilationResult.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)?.ToList() ?? new List<Diagnostic>();
                    List<Exception> exceptions = new();
                    foreach (var error in errors)
                    {
                        exceptions.Add(new Exception(error.GetMessage()));
                    }
                    throw new AggregateException(exceptions);
                }
                else
                {
                    memStream.Seek(0, SeekOrigin.Begin);

                    AssemblyLoadContext assemblyContext = new AssemblyLoadContext(Path.GetRandomFileName(), true);
                    Assembly assembly = assemblyContext.LoadFromStream(memStream);

                    var method = assembly.GetTypes()[2]?.GetRuntimeMethod("Function", Array.Empty<Type>())!;
                    result = (SplitVoid?)method.Invoke(null, null);
                }
            }
            return result;
        }

        public SplitVoid(double re = 0, double unf = 0)
        {
            this.re = re;
            this.unf = unf;
        }

        public override bool Equals(object? obj)
        {
            if (obj is SplitVoid a)
                return a == this;
            else return false;
        }

        public override int GetHashCode()
        {
            return re.GetHashCode() ^ unf.GetHashCode();
        }

        public static SplitVoid operator +(SplitVoid lhs, SplitVoid rhs)
        {
            return new(lhs.re + rhs.re, rhs.unf + lhs.unf);
        }

        public static SplitVoid operator -(SplitVoid lhs, SplitVoid rhs)
        {
            return lhs + -rhs;
        }

        public static SplitVoid operator -(SplitVoid n)
        {
            return new(-n.re, -n.unf);
        }

        public static SplitVoid operator +(SplitVoid n)
        {
            return new(n.re, n.unf);
        }

        public static bool operator ==(SplitVoid lhs, SplitVoid rhs)
        {
            return lhs.re == rhs.re && lhs.unf == rhs.unf;
        }

        public static bool operator !=(SplitVoid lhs, SplitVoid rhs)
        {
            return !(lhs == rhs);
        }

        public static SplitVoid operator *(SplitVoid lhs, SplitVoid rhs)
        {
            var a = lhs.re;
            var b = lhs.unf;
            var c = rhs.re;
            var d = rhs.unf;

            if (lhs == 0 && rhs == 0)
                return 0;
            else if (lhs == 0 && rhs != 0)
                return new(d, 0);
            else if (lhs != 0 && rhs == 0)
                return new(b, 0);
            else
                return new(a * c, a * d + b * c + b * d);
        }

        public static implicit operator SplitVoid(double n)
        {
            return new(n, 0);
        }

        public static implicit operator SplitVoid(int n)
        {
            return new(Convert.ToDouble(n), 0);
        }

        public static SplitVoid Invert(SplitVoid n)
        {
            if (n == 0)
                return new(0, 1);
            if (n == 1)
                return 1;
            if (n == new SplitVoid(-1, 1))
                return 0;
            if (n == new SplitVoid(0, 1))
                return 0;

            if((n.re == 0 && n.unf != 0) || n.re + n.unf == 0 || (n.unf == 1 && n.re != 0 && n.re != -1))
            {
                throw new ArgumentException("Number does not have a multiplicative inverse.");
            }
            return new(1 / n.re, -n.unf / (n.re * n.re + n.re * n.unf));
        }

        public static SplitVoid operator /(SplitVoid lhs, SplitVoid rhs)
        {
            return lhs * Invert(rhs);
        }

        public override string ToString()
        {
            if (re == 0)
                return $"{unf}u";
            if (unf == 0)
                return $"{re}";
            return $"{re} + {unf}u";
        }

        public SplitVoid ApplyFunc(Func<double, double> func)
        {
            return func(re) + func(re + unf) * u - func(unf) * u;
        }

        public static SplitVoid ApplyFunc(Func<double, double> func, SplitVoid value) => value.ApplyFunc(func);

        public static SplitVoid Sqrt(SplitVoid n)
        {
            return Math.Sqrt(n.re) + (-Math.Sqrt(n.re) + Math.Sqrt(n.re + n.unf)) * u;
        }

        public SplitVoid Pow(double b)
        {
            if (!double.IsRealNumber(b))
                throw new ArgumentException("The number's real component cannot be infinite or NaN.", nameof(b));
            Fractions.TypeConverters.FractionTypeConverter converter = new();
            Fraction frac = (Fraction)converter.ConvertFrom(b)!;
            BigInteger numer = frac.Numerator;
            List<SplitVoid> tempNums = new();
            Func<SplitVoid, BigInteger, SplitVoid> repeatedSquare = new((n, times) =>
            {
                for (BigInteger i = 0; i < times; i++)
                    n *= n;
                return n;
            });
            for (BigInteger i = 0; i < numer.GetBitLength(); i++)
            {
                if (((numer >> (int)i) & 1) > 0)
                {
                    tempNums.Add(repeatedSquare((SplitVoid)MemberwiseClone(), i));
                }
            }
            var temp1 = tempNums.Aggregate((a, b) => a + b);
            int numRoots = (int)Math.Floor(Math.Log2((double)frac.Denominator));
            for (int i = 0; i < numRoots; i++)
            {
                temp1 = Sqrt(temp1);
            }
            return temp1;
        }

        public static SplitVoid operator ^(SplitVoid lhs, double rhs)
        {
            return lhs.Pow(rhs);
        }
    }

    public static class Extensions
    {
        public static void RemoveDuplicates<T>(this List<T> list)
        {
            var newList = new List<T>();
            foreach(var elem in list)
            {
                if (!newList.Contains(elem))
                    newList.Add(elem);
            }
            list = newList;
        }
    }
}