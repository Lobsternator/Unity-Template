using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GenerateStateContainer
{
    class StateContainerSyntax
    {
        public string GenericParameterA { get; set; }
        public string GenericParameterB { get; set; }
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

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.AttributeLists.Count > 0)
            {
                if (!classDeclarationSyntax.AttributeLists.Any((a) => a.Attributes.Any((n) => n.ToString() == "GenerateStateContainer")))
                    return;

                SimpleBaseTypeSyntax baseType = classDeclarationSyntax.BaseList.DescendantNodes().SingleOrDefault((n) => n is SimpleBaseTypeSyntax) as SimpleBaseTypeSyntax;
                if (baseType.GetText().ToString() != "IStateContainer")
                    return;

                StateContainerSyntax stateContainerSyntax = new StateContainerSyntax();
                TypeArgumentListSyntax argumentList       = baseType.DescendantNodes().SingleOrDefault((n) => n is TypeArgumentListSyntax) as TypeArgumentListSyntax;
                if (argumentList.Arguments.Count == 0)
                    return;
                
                if (argumentList.Arguments.Count == 1)
                {
                    stateContainerSyntax.GenericParameterA = argumentList.Arguments[0].ToString();
                    stateContainerSyntax.GenericParameterB = string.Empty;
                }
                else
                {
                    stateContainerSyntax.GenericParameterA = argumentList.Arguments[0].ToString();
                    stateContainerSyntax.GenericParameterB = argumentList.Arguments[2].ToString();
                }

                StateContainers.Add(stateContainerSyntax);
            }
        }
    }
}
