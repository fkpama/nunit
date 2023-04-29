using System.Collections;
using System.Diagnostics;
using NUnit.Framework.Interfaces;
using CList = System.Collections.Generic.List<object>;
namespace NUnit.Framework
{
    /// <summary>
    /// Test Explorer Hierarchy related extensions
    /// </summary>
    public static class HierarchyExtensions
    {
        const char Separator = '|';
        static readonly string s_emptyArray = string.Join(Separator.ToString(), new string[HierarchyConstants.Levels.TotalLevelCount]);
        static IList EnsureHierarchyProperty(ITest test)
        {
            var lst = test.Properties[HierarchyConstants.HierarchyPropertyId];
            if(lst is null)
            {
                test.Properties[HierarchyConstants.HierarchyPropertyId] = new CList();
            }

            if (lst.Count == 0)
            {
                lst.Add(s_emptyArray);
            }
            return lst;
        }
        private static void SetHierarchyProperty(this ITest test, string[] values)
        {
            var lst = EnsureHierarchyProperty(test);
            lst[0] = string.Join(Separator.ToString(), values);

        }
        private static string[] GetHierarchyProperty(this ITest test)
        {
            var lst = EnsureHierarchyProperty(test);
            var str = ((string)lst[0]).Split(Separator);
            Debug.Assert(str.Length == HierarchyConstants.Levels.TotalLevelCount);
            return str;
        }

        /// <summary>
        /// Sets the name of the Namespace hierarchy in test explorer
        /// </summary>
        /// <param name="test"></param>
        /// <param name="namespace"></param>
        public static void SetTestExplorerNamespace(this ITest test, string @namespace)
        {
            var ar = test.GetHierarchyProperty();
            ar[HierarchyConstants.Levels.NamespaceIndex] = @namespace;
            test.SetHierarchyProperty(ar);
        }

        /// <summary>
        /// Sets the name of the Container hierarchy in test explorer
        /// </summary>
        /// <param name="test"></param>
        /// <param name="container"></param>
        public static void SetTestExplorerContainer(this ITest test, string container)
        {
            var ar = test.GetHierarchyProperty();
            ar[HierarchyConstants.Levels.ContainerIndex] = container;
            test.SetHierarchyProperty(ar);
        }

        /// <summary>
        /// Sets the name of the ClassName hierarchy in test explorer
        /// </summary>
        /// <param name="test"></param>
        /// <param name="container"></param>
        public static void SetTestExplorerClassName(this ITest test, string container)
        {
            var ar = test.GetHierarchyProperty();
            ar[HierarchyConstants.Levels.ClassIndex] = container;
            test.SetHierarchyProperty(ar);
        }

        /// <summary>
        /// Sets the name of the ClassName hierarchy in test explorer
        /// </summary>
        /// <param name="test"></param>
        /// <param name="container"></param>
        public static void SetTestExplorerTestGroup(this ITest test, string container)
        {
            var ar = test.GetHierarchyProperty();
            ar[HierarchyConstants.Levels.TestGroupIndex] = container;
            test.SetHierarchyProperty(ar);
        }
    }
}
