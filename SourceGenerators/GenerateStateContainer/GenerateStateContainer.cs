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
        public string StateName { get; set; }
    }

    internal class StateSyntax
    {
        public SyntaxTree SyntaxTree { get; set; }

        public override string ToString()
        {
            return SyntaxTree.GetText().ToString();
        }
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
                string                 stateName              = stateContainerSyntax.StateName;
                INamedTypeSymbol       classDeclTypeSymbol    = semanticModel.GetDeclaredSymbol(classDeclSyntax);
                string                 constituentNamespaces  = string.Join("", classDeclTypeSymbol.ContainingNamespace.ConstituentNamespaces);

                //File.WriteAllText($"C:\\Users\\juliedva\\Desktop\\diag\\debug{i}.txt", string.Join("\n", receiver.States));
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

            ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
            if (!HasAttribute(classDeclarationSyntax, "GenerateStateContainer"))
                return;

            if (!TryGetBaseType(context.SemanticModel, classDeclarationSyntax, "IStateContainer<", out var baseTypeSyntax))
                return;

            StateContainerSyntax stateContainerSyntax   = new StateContainerSyntax();
            stateContainerSyntax.SemanticModel          = context.SemanticModel;
            stateContainerSyntax.SyntaxTree             = classDeclarationSyntax.SyntaxTree;
            stateContainerSyntax.ClassDeclarationSyntax = classDeclarationSyntax;

            TypeArgumentListSyntax typeArgumentListSyntax = baseTypeSyntax.DescendantNodes().First((n) => n is TypeArgumentListSyntax) as TypeArgumentListSyntax;
            stateContainerSyntax.StateMachineName         = typeArgumentListSyntax.Arguments[0].ToString();
            stateContainerSyntax.StateName                = typeArgumentListSyntax.Arguments.Count > 1 ? typeArgumentListSyntax.Arguments[1].ToString() : $"State<{stateContainerSyntax.StateMachineName}>";

            AddStateContainer(stateContainerSyntax);
        }

        private void LocateStateDeclaration(GeneratorSyntaxContext context)
        {
            if (!(context.Node is ClassDeclarationSyntax))
                return;

            ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
            if (!TryGetBaseType(context.SemanticModel, classDeclarationSyntax, "State<", out var baseTypeSyntax))
                return;

            TypeArgumentListSyntax typeArgumentListSyntax = baseTypeSyntax.DescendantNodes().First((n) => n is TypeArgumentListSyntax) as TypeArgumentListSyntax;
            string stateMachineName = typeArgumentListSyntax.Arguments[0].ToString();

            StateSyntax stateSyntax = new StateSyntax();
            stateSyntax.SyntaxTree = classDeclarationSyntax.SyntaxTree;

            AddState(stateMachineName, stateSyntax);
        }

        private bool HasAttribute(MemberDeclarationSyntax classDeclarationSyntax, string attribute)
        {
            return classDeclarationSyntax.AttributeLists.Any((a) => a.Attributes.Any((n) => n.ToString().StartsWith(attribute)));
        }

        private bool TryGetBaseType(SemanticModel semanticModel, BaseTypeDeclarationSyntax baseTypeDeclSyntax, string baseType, out BaseTypeSyntax baseTypeSyntax)
        {
            //StreamWriter streamWriter;
            
            if (baseTypeDeclSyntax.BaseList is null)
            {
                baseTypeSyntax = null;
                return false;
            }

            foreach (BaseTypeSyntax t in baseTypeDeclSyntax.BaseList.Types)
            {
                ITypeSymbol typeSymbol = semanticModel.GetTypeInfo(t.Type).Type;

                //streamWriter = File.AppendText("C:\\Users\\juliedva\\Desktop\\diag\\debug0.txt");
                //streamWriter.WriteLine(string.Join("\n", typeSymbol.AllInterfaces));
                //streamWriter.Close();

                if (t.ToString().StartsWith(baseType) || typeSymbol.AllInterfaces.Any((i) => i.ToString().StartsWith(baseType)))
                {
                    baseTypeSyntax = t;
                    return true;
                }
            }

            baseTypeSyntax = null;
            return false;
        }

        private void AddStateContainer(StateContainerSyntax stateContainerSyntax)
        {
            StateContainers.Add(stateContainerSyntax);
        }

        private void AddState(string stateMachineName, StateSyntax stateSyntax)
        {
            if (States.ContainsKey(stateMachineName))
                States[stateMachineName].Add(stateSyntax);
            else
                States.Add(stateMachineName, new List<StateSyntax>() { stateSyntax });
        }
    }
}
