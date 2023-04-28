using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GenerateStateContainer
{
    internal class StateContainerSyntax
    {
        public SemanticModel SemanticModel { get; set; }
        public SyntaxTree SyntaxTree { get; set; }
        public ClassDeclarationSyntax ClassDeclarationSyntax { get; set; }
        public string StateMachineName { get; set; }
        public string BaseStateName { get; set; }
    }

    internal class StateSyntax
    {
        public string StateMachineName { get; set; }
        public string BaseStateName { get; set; }
        public string StateName { get; set; }
        public string StateNameWithoutNamespace { get; set; }
    }

    internal static class GeneratorUtility
    {
        public const string BaseNamespaceName                      = "Template.Core";
        public const string ContainerAttributeNameWithoutNamespace = "GenerateStateContainer";
        public const string ContainerAttributeName                 = BaseNamespaceName + "." + ContainerAttributeNameWithoutNamespace;
        public const string BaseContainerNameWithoutNamespace      = "IStateContainer";
        public const string BaseContainerName                      = BaseNamespaceName + "." + BaseContainerNameWithoutNamespace;
        public const string BaseStateNameWithoutNamespace          = "State";
        public const string BaseStateName                          = BaseNamespaceName + "." + BaseStateNameWithoutNamespace;

        public static bool TryGetContainingNamespace(INamedTypeSymbol typeSymbol, out string containingNamespace)
        {
            containingNamespace = string.Empty;
            if (typeSymbol.ContainingNamespace is null)
                return false;

            containingNamespace = string.Join(".", typeSymbol.ContainingNamespace.ConstituentNamespaces);
            if (containingNamespace == "<global namespace>")
                return false;

            return true;
        }
    }

    [Generator]
    public class StateContainerGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
                return;

            foreach (var states in receiver.States.Values)
                states.Sort((s1, s2) => s1.StateNameWithoutNamespace.CompareTo(s2.StateNameWithoutNamespace));

            for (int i = 0; i < receiver.StateContainers.Count; i++)
            {
                StateContainerSyntax   stateContainerSyntax = receiver.StateContainers[i];
                SemanticModel          semanticModel        = stateContainerSyntax.SemanticModel;
                ClassDeclarationSyntax classDeclSyntax      = stateContainerSyntax.ClassDeclarationSyntax;
                string                 stateMachineName     = stateContainerSyntax.StateMachineName;
                string                 baseStateName        = stateContainerSyntax.BaseStateName;
                INamedTypeSymbol       classDeclTypeSymbol  = semanticModel.GetDeclaredSymbol(classDeclSyntax);
                string[]               usingDirectives      = GetUsingDirectives(stateContainerSyntax.SyntaxTree);
                int                    indentationLevel     = 0;

                StringBuilder sourceBuilder = new StringBuilder();
                sourceBuilder.AppendLine("#pragma warning disable CS0105 // Using directive appeared previously in this namespace");
                sourceBuilder.AppendLine("// Inherited using directives");
                foreach (string usingDirective in usingDirectives)
                    sourceBuilder.AppendLine(usingDirective);

                sourceBuilder.AppendLine();

                sourceBuilder.AppendLine("// Required using directives");
                sourceBuilder.AppendLine("using System;");
                sourceBuilder.AppendLine("using System.Collections.Generic;");
                sourceBuilder.AppendLine("using System.Collections.ObjectModel;");
                sourceBuilder.AppendLine("using UnityEngine;");
                sourceBuilder.AppendLine("#pragma warning restore CS0105 // Using directive appeared previously in this namespace");

                if (usingDirectives.Length > 0)
                    sourceBuilder.AppendLine();

                bool hasNamespace = GeneratorUtility.TryGetContainingNamespace(classDeclTypeSymbol, out var containingNamespace);
                if (hasNamespace)
                {
                    sourceBuilder.Append("namespace ");
                    sourceBuilder.AppendLine(containingNamespace);
                    sourceBuilder.AppendLine("{");

                    indentationLevel++;
                }

                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.Append(string.Join(" ", classDeclSyntax.Modifiers));
                sourceBuilder.Append(" ");
                sourceBuilder.Append(classDeclSyntax.Keyword);
                sourceBuilder.Append(" ");
                sourceBuilder.Append(classDeclSyntax.Identifier);
                sourceBuilder.Append(classDeclSyntax.TypeParameterList);
                if (!(classDeclSyntax.BaseList is null))
                {
                    sourceBuilder.Append(" ");
                    sourceBuilder.Append(classDeclSyntax.BaseList);
                }
                if (classDeclSyntax.ConstraintClauses.Count > 0)
                {
                    sourceBuilder.Append(" ");
                    sourceBuilder.Append(string.Join(" ", classDeclSyntax.ConstraintClauses));
                }

                sourceBuilder.AppendLine();
                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.AppendLine("{");
                indentationLevel++;

                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.Append("private Dictionary<Type, ");
                sourceBuilder.Append(baseStateName);
                sourceBuilder.AppendLine("> _states;");

                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.Append("public ReadOnlyDictionary<Type, ");
                sourceBuilder.Append(baseStateName);
                sourceBuilder.AppendLine("> States { get; }");

                sourceBuilder.AppendLine();

                foreach (StateSyntax stateSyntax in receiver.States[stateMachineName])
                {
                    AppendIndentation(sourceBuilder, indentationLevel);
                    sourceBuilder.Append("[field: SerializeField] public ");
                    sourceBuilder.Append(stateSyntax.StateName);
                    sourceBuilder.Append(" ");
                    sourceBuilder.Append(stateSyntax.StateNameWithoutNamespace);
                    sourceBuilder.Append(" { get; private set; } = new ");
                    sourceBuilder.Append(stateSyntax.StateName);
                    sourceBuilder.AppendLine("();");
                }

                sourceBuilder.AppendLine();

                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.Append("public ");
                sourceBuilder.Append(classDeclSyntax.Identifier.ToString());
                sourceBuilder.AppendLine("()");
                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.AppendLine("{");
                indentationLevel++;

                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.Append("_states = new Dictionary<Type, ");
                sourceBuilder.Append(baseStateName);
                sourceBuilder.Append(">(");
                sourceBuilder.Append(receiver.States[stateMachineName].Count);
                sourceBuilder.Append(")");
                sourceBuilder.AppendLine();

                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.AppendLine("{");
                indentationLevel++;

                foreach (StateSyntax stateSyntax in receiver.States[stateMachineName])
                {
                    AppendIndentation(sourceBuilder, indentationLevel);
                    sourceBuilder.Append("{ typeof(");
                    sourceBuilder.Append(stateSyntax.StateName);
                    sourceBuilder.Append("), ");
                    sourceBuilder.Append(stateSyntax.StateNameWithoutNamespace);
                    sourceBuilder.AppendLine(" },");
                }

                indentationLevel--;
                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.AppendLine("};");

                sourceBuilder.AppendLine();

                AppendIndentation(sourceBuilder, indentationLevel);
                sourceBuilder.Append("States = new ReadOnlyDictionary<Type, ");
                sourceBuilder.Append(baseStateName);
                sourceBuilder.AppendLine(">(_states);");

                for (int j = indentationLevel; j > 0; j--)
                {
                    AppendIndentation(sourceBuilder, j - 1);
                    sourceBuilder.AppendLine("}");
                }

                string sourceName = hasNamespace ? containingNamespace + "." + classDeclSyntax.Identifier.ToString() : classDeclSyntax.Identifier.ToString();
                context.AddSource($"{sourceName}_g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));

                //File.WriteAllText($"C:\\Diagnosis\\StateContainer_{sourceName}.txt", string.Join("\n", sourceBuilder.ToString()));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        private string[] GetUsingDirectives(SyntaxTree syntaxTree)
        {
            return syntaxTree
                .GetRoot()
                .DescendantNodesAndSelf()
                .Where((n) => n is UsingDirectiveSyntax)
                .Select((n) => n.ToString())
                .ToArray();
        }

        private void AppendIndentation(StringBuilder stringBuilder, int indentation)
        {
            for (int i = 0; i < indentation; i++) stringBuilder.Append("\t");
        }
    }

    internal class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<StateContainerSyntax> StateContainers { get; } = new List<StateContainerSyntax>();
        public Dictionary<string, List<StateSyntax>> States { get; } = new Dictionary<string, List<StateSyntax>>();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            LocateStateContainerDeclaration(context);
            LocateStateDeclaration(context);
        }

        private void LocateStateContainerDeclaration(GeneratorSyntaxContext context)
        {
            if (!(context.Node is ClassDeclarationSyntax))
                return;

            ClassDeclarationSyntax classDeclSyntax = (ClassDeclarationSyntax)context.Node;
            if (context.SemanticModel.GetDeclaredSymbol(classDeclSyntax).IsAbstract)
                return;

            if (!HasAttribute(classDeclSyntax, GeneratorUtility.ContainerAttributeNameWithoutNamespace))
                return;

            if (!TryGetBaseType(context.SemanticModel, classDeclSyntax, $"{GeneratorUtility.BaseContainerNameWithoutNamespace}<", true, out var baseTypeSymbol))
                return;

            string[]             typeArgumentNames      = GetTypeArgumentNames(baseTypeSymbol, true);
            StateContainerSyntax stateContainerSyntax   = new StateContainerSyntax();
            stateContainerSyntax.SemanticModel          = context.SemanticModel;
            stateContainerSyntax.SyntaxTree             = classDeclSyntax.SyntaxTree;
            stateContainerSyntax.ClassDeclarationSyntax = classDeclSyntax;
            stateContainerSyntax.StateMachineName       = typeArgumentNames[0];
            stateContainerSyntax.BaseStateName          = typeArgumentNames.Length > 1 ? typeArgumentNames[1] : $"{GeneratorUtility.BaseStateName}<{stateContainerSyntax.StateMachineName}>";

            AddStateContainer(stateContainerSyntax);
        }

        private void LocateStateDeclaration(GeneratorSyntaxContext context)
        {
            if (!(context.Node is ClassDeclarationSyntax))
                return;

            ClassDeclarationSyntax classDeclSyntax     = (ClassDeclarationSyntax)context.Node;
            INamedTypeSymbol       classDeclTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclSyntax);
            if (classDeclTypeSymbol.IsAbstract)
                return;

            if (!TryGetBaseType(context.SemanticModel, classDeclSyntax, $"{GeneratorUtility.BaseStateNameWithoutNamespace}<", true, out var baseTypeSymbol))
                return;

            string[]    typeArgumentNames = GetTypeArgumentNames(baseTypeSymbol, true);
            StateSyntax stateSyntax       = new StateSyntax();
            stateSyntax.StateMachineName  = typeArgumentNames[0];
            stateSyntax.BaseStateName     = typeArgumentNames.Length > 1 ? typeArgumentNames[1] : $"{GeneratorUtility.BaseStateName}<{stateSyntax.StateMachineName}>";

            bool hasNamespace                     = GeneratorUtility.TryGetContainingNamespace(classDeclTypeSymbol, out var containingNamespace);
            stateSyntax.StateName                 = hasNamespace ? containingNamespace + "." + classDeclSyntax.Identifier.ToString() : classDeclSyntax.Identifier.ToString();
            stateSyntax.StateNameWithoutNamespace = classDeclSyntax.Identifier.ToString();

            AddState(stateSyntax);
        }

        private bool HasAttribute(MemberDeclarationSyntax classDeclSyntax, string attribute)
        {
            return classDeclSyntax.AttributeLists.Any((a) => a.Attributes.Any((n) => n.ToString().StartsWith(attribute)));
        }

        private bool TryGetBaseType(SemanticModel semanticModel, BaseTypeDeclarationSyntax typeDeclSyntax, string baseType, bool includeNamespace, out INamedTypeSymbol baseTypeSymbol)
        {
            baseTypeSymbol = null;

            if (typeDeclSyntax.BaseList is null)
                return false;

            foreach (BaseTypeSyntax baseTypeSyntax in typeDeclSyntax.BaseList.Types)
            {
                INamedTypeSymbol typeSymbol = semanticModel.GetTypeInfo(baseTypeSyntax.Type).Type as INamedTypeSymbol;
                if (typeSymbol is null)
                    continue;

                if (IsDerivedFrom(typeSymbol, baseType, includeNamespace, out baseTypeSymbol))
                    return true;
            }

            return false;
        }

        private bool IsDerivedFrom(INamedTypeSymbol typeSymbol, string targetType, bool includeNamespace, out INamedTypeSymbol baseTypeSymbol)
        {
            while (typeSymbol != null)
            {
                if (GetFullTypeName(typeSymbol, includeNamespace).StartsWith(targetType))
                {
                    baseTypeSymbol = typeSymbol;
                    return true;
                }

                typeSymbol = typeSymbol.BaseType;
            }

            baseTypeSymbol = null;
            return false;
        }

        private string GetFullTypeName(INamedTypeSymbol typeSymbol, bool includeNamespace)
        {
            StringBuilder fullTypeNameBuilder = new StringBuilder();
            fullTypeNameBuilder.Append(typeSymbol.Name);
            if (typeSymbol.TypeArguments.Length > 0)
            {
                fullTypeNameBuilder.Append("<");
                fullTypeNameBuilder.Append(string.Join(", ", GetTypeArgumentNames(typeSymbol, includeNamespace)));
                fullTypeNameBuilder.Append(">");
            }

            return fullTypeNameBuilder.ToString();
        }

        private string[] GetTypeArgumentNames(INamedTypeSymbol typeSymbol, bool includeNamespace)
        {
            return typeSymbol.TypeArguments.Select((a) =>
            {
                string fullArgumentName = a.ToString();
                if (includeNamespace)
                    return fullArgumentName;

                return fullArgumentName.Substring(fullArgumentName.LastIndexOf('.') + 1);

            }).ToArray();
        }

        private void AddStateContainer(StateContainerSyntax stateContainerSyntax)
        {
            StateContainers.Add(stateContainerSyntax);
        }

        private void AddState(StateSyntax stateSyntax)
        {
            if (States.ContainsKey(stateSyntax.StateMachineName))
                States[stateSyntax.StateMachineName].Add(stateSyntax);
            else
                States.Add(stateSyntax.StateMachineName, new List<StateSyntax>() { stateSyntax });
        }
    }
}
