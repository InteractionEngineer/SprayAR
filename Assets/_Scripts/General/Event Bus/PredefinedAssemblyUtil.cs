using System;
using System.Collections.Generic;

namespace SprayAR.General
{
    /// <summary>
    /// Utility class for working with predefined assemblies in Unity.
    /// </summary>
    public static class PredefinedAssemblyUtil
    {
        //TODO: Add user defined assemblies if necessary.
        enum AssemblyType
        {
            AssemblyCSharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstpass,
            AssemblyCSharpFirstPass
        }

        /// <summary>
        /// Gets the type of the assembly based on the provided assembly name.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <returns>The type of the assembly.</returns>
        static AssemblyType? GetAssemblyType(string assemblyName)
        {
            return assemblyName switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstpass,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                _ => null,
            };
        }

        /// <summary>
        /// Adds types from the specified assembly to the collection of types, based on the provided interface type.
        /// </summary>
        /// <param name="assembly">The assembly containing the types to be added.</param>
        /// <param name="types">The collection of types to add the types from the assembly to.</param>
        /// <param name="interfaceType">The interface type that the types should implement or inherit from.</param>
        static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type interfaceType)
        {
            if (assembly == null) return;

            for (int i = 0; i < assembly.Length; i++)
            {
                Type type = assembly[i];
                if (type != interfaceType && interfaceType.IsAssignableFrom(type))
                {
                    types.Add(type);
                }
            }
        }

        /// <summary>
        /// Retrieves a list of types that implement a specified interface from all loaded assemblies.
        /// </summary>
        /// <param name="interfaceType">The interface type to search for.</param>
        /// <returns>A list of types that implement the specified interface.</returns>
        public static List<Type> GetTypes(Type interfaceType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            List<Type> types = new List<Type>();
            for (int i = 0; i < assemblies.Length; i++)
            {
                AssemblyType? assemblyType = GetAssemblyType(assemblies[i].GetName().Name);
                if (assemblyType != null)
                {
                    assemblyTypes.Add((AssemblyType)assemblyType, assemblies[i].GetTypes());
                }
            }

            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharp], types, interfaceType);
            // AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharpFirstPass], types, interfaceType);

            return types;
        }
    }
}