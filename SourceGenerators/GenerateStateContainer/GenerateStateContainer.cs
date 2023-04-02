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
    }

    [Generator]
    public class StateContainerGenerator : ISourceGenerator
    {
        private const string _attributeText = @"
using System;

namespace Template.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GenerateStateContainerAttribute : Attribute
    {

    }
}
";

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
                return;

            for (int i = 0; i < receiver.StateContainers.Count; i++)
            {
                StateContainerSyntax   stateContainerSyntax   = receiver.StateContainers[i];
                SemanticModel          semanticModel          = stateContainerSyntax.SemanticModel;
                SyntaxTree             syntaxTree             = semanticModel.SyntaxTree;
                ClassDeclarationSyntax classDeclSyntax        = stateContainerSyntax.ClassDeclarationSyntax;
                string                 stateMachineName       = stateContainerSyntax.StateMachineName;
                string                 baseStateName          = stateContainerSyntax.BaseStateName;
                INamedTypeSymbol       classDeclTypeSymbol    = semanticModel.GetDeclaredSymbol(classDeclSyntax);
                string                 constituentNamespaces  = string.Join("", classDeclTypeSymbol.ContainingNamespace.ConstituentNamespaces);

                File.WriteAllText($"C:\\Diagnosis\\debug{i}.txt", string.Join("\n", receiver.States[stateMachineName].Select((s) => s.StateName)));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(i => i.AddSource("GenerateStateContainerAttribute_g.cs", _attributeText));
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
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

            if (!HasAttribute(classDeclSyntax, "GenerateStateContainer"))
                return;

            if (!TryGetBaseType(context.SemanticModel, classDeclSyntax, "IStateContainer<", true, out var baseTypeSymbol))
                return;

            string[]             typeArgumentNames      = GetTypeArgumentNames(baseTypeSymbol, true);
            StateContainerSyntax stateContainerSyntax   = new StateContainerSyntax();
            stateContainerSyntax.SemanticModel          = context.SemanticModel;
            stateContainerSyntax.SyntaxTree             = classDeclSyntax.SyntaxTree;
            stateContainerSyntax.ClassDeclarationSyntax = classDeclSyntax;
            stateContainerSyntax.StateMachineName       = typeArgumentNames[0];
            stateContainerSyntax.BaseStateName          = typeArgumentNames.Length > 1 ? typeArgumentNames[1] : $"State<{stateContainerSyntax.StateMachineName}>";

            AddStateContainer(stateContainerSyntax);
        }

        private void LocateStateDeclaration(GeneratorSyntaxContext context)
        {
            if (!(context.Node is ClassDeclarationSyntax))
                return;

            ClassDeclarationSyntax classDeclSyntax = (ClassDeclarationSyntax)context.Node;
            if (context.SemanticModel.GetDeclaredSymbol(classDeclSyntax).IsAbstract)
                return;

            if (!TryGetBaseType(context.SemanticModel, classDeclSyntax, "State<", true, out var baseTypeSymbol))
                return;

            string[]    typeArgumentNames = GetTypeArgumentNames(baseTypeSymbol, true);
            StateSyntax stateSyntax       = new StateSyntax();
            stateSyntax.StateMachineName  = typeArgumentNames[0];
            stateSyntax.BaseStateName     = typeArgumentNames.Length > 1 ? typeArgumentNames[1] : $"State<{stateSyntax.StateMachineName}>";
            stateSyntax.StateName         = classDeclSyntax.Identifier.ToString();

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
        private bool IsDerivedFrom(INamedTypeSymbol typeSymbol, string targetType, bool includeNamespace)
        {
            return IsDerivedFrom(typeSymbol, targetType, includeNamespace, out var _);
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
