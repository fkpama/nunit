// ***********************************************************************
// Copyright (c) 2007 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using NUnit.TestUtilities.Collections;

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class CollectionSupersetConstraintTests : ConstraintTestBaseNoData
    {
        [SetUp]
        public void SetUp()
        {
            TheConstraint = new CollectionSupersetConstraint(new int[] { 1, 2, 3, 4, 5 });
            StringRepresentation = "<supersetof System.Int32[]>";
            ExpectedDescription = "superset of < 1, 2, 3, 4, 5 >";
        }

        static object[] SuccessData = new object[]
        {
            new int[] { 1, 2, 3, 4, 5, 6 }
            , new int[] { 1, 2, 3, 4, 5 }
            , new int[] { 1, 2, 2, 2, 3, 4, 5, 3 }
            , new int[] { 1, 2, 2, 2, 3, 4, 5, 7 }
        };

        static object[] FailureData = new object[]
        {
            new object[] { new int[] { 1, 3, 7 }, "< 1, 3, 7 >", "< 2, 4, 5 >" }
            , new object[] { new int[] { 1, 2, 2, 2, 5 }, "< 1, 2, 2, 2, 5 >", "< 3, 4 >" }
            , new object[] { new int[] { 1, 2, 3, 5 }, "< 1, 2, 3, 5 >", "< 4 >" }
            , new object[] { new int[] { 1, 2, 3, 5, 7 }, "< 1, 2, 3, 5, 7 >", "< 4 >" }
        };

        [Test, TestCaseSource(nameof(SuccessData))]
        public void SucceedsWithGoodValues(object actualValue)
        {
            Assert.That(actualValue, TheConstraint);
        }

        [Test, TestCaseSource(nameof(FailureData))]
        public void FailsWithBadValues(object badActualValue, string actualMessage, string missingMessage)
        {
            var constraintResult = TheConstraint.ApplyTo(badActualValue);
            Assert.IsFalse(constraintResult.IsSuccess);

            TextMessageWriter writer = new TextMessageWriter();
            constraintResult.WriteMessageTo(writer);
            Assert.That(writer.ToString(), Is.EqualTo(
                TextMessageWriter.Pfx_Expected + ExpectedDescription + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + actualMessage + Environment.NewLine +
                "  Missing items: " + missingMessage + Environment.NewLine));
        }

        [Test]
        [TestCaseSource(typeof(IgnoreCaseDataProvider), nameof(IgnoreCaseDataProvider.TestCases))]
        public void HonorsIgnoreCase(IEnumerable expected, IEnumerable actual)
        {
            var constraint = new CollectionSupersetConstraint(expected).IgnoreCase;
            var constraintResult = constraint.ApplyTo(actual);
            if (!constraintResult.IsSuccess)
            {
                MessageWriter writer = new TextMessageWriter();
                constraintResult.WriteMessageTo(writer);
                Assert.Fail(writer.ToString());
            }
        }

#if !NET35
        [Test]
        public void WorksOnTuples()
        {
            var actual = new[] { Tuple.Create('a', 1), Tuple.Create('b', 2), Tuple.Create('c', 3), Tuple.Create('d', 4) };
            var expected = new[] { Tuple.Create('b', 2), Tuple.Create('c', 3) };

            var constraint = new CollectionSupersetConstraint(expected);
            var constraintResult = constraint.ApplyTo(actual);

            Assert.That(constraintResult.IsSuccess, Is.True);
        }

        [Test]
        public void WorksOnTuples_OneTypeIsntIComparer()
        {
            var a = new S { C = 'a' };
            var b = new S { C = 'b' };
            var c = new S { C = 'c' };
            var d = new S { C = 'd' };

            var actual = new[] { Tuple.Create(a, 1), Tuple.Create(b, 2), Tuple.Create(c, 3), Tuple.Create(d, 4) };
            var expected = new[] { Tuple.Create(b, 2), Tuple.Create(c, 3) };

            var constraint = new CollectionSupersetConstraint(expected);
            var constraintResult = constraint.ApplyTo(actual);

            Assert.That(constraintResult.IsSuccess, Is.True);
        }

        [Test]
        public void WorksOnValueTuples()
        {
            var actual = new[] { ('a', 1), ('b', 2), ('c', 3), ('d', 4) };
            var expected = new[] { ('b', 2), ('c', 3) };

            var constraint = new CollectionSupersetConstraint(expected);
            var constraintResult = constraint.ApplyTo(actual);

            Assert.That(constraintResult.IsSuccess, Is.True);
        }

        [Test]
        public void WorksOnValueTuples_OneTypeIsntIComparer()
        {
            var a = new S { C = 'a' };
            var b = new S { C = 'b' };
            var c = new S { C = 'c' };
            var d = new S { C = 'd' };

            var actual = new[] { (a, 1), (b, 2), (c, 3), (d, 4) };
            var expected = new[] { (b, 2), (c, 3) };

            var constraint = new CollectionSupersetConstraint(expected);
            var constraintResult = constraint.ApplyTo(actual);

            Assert.That(constraintResult.IsSuccess, Is.True);
        }
        
        private class S
        {
            public char C;
        }
#endif

        public class IgnoreCaseDataProvider
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new SimpleObjectCollection("z", "Y", "X"), new SimpleObjectCollection("w", "x", "y", "z"));
                    yield return new TestCaseData(new object[] { 'a', 'b', 'c' }, new[] { 'A', 'B', 'C', 'D', 'E' });
                    yield return new TestCaseData(new object[] { "A", "C", "B" }, new[] { "a", "b", "c", "d", "e" });
                    yield return new TestCaseData(new Dictionary<int, string> { { 1, "A" } }, new Dictionary<int, string> { { 1, "a" }, { 2, "b" } });
                    yield return new TestCaseData(new Dictionary<int, char> { { 1, 'a' } }, new Dictionary<int, char> { { 1, 'A' }, { 2, 'B' } });
                    yield return new TestCaseData(new Dictionary<string, int> { { "b", 2 } }, new Dictionary<string, int> { { "b", 2 }, { "a", 1 } });
                    yield return new TestCaseData(new Dictionary<char, int> { { 'a', 1 } }, new Dictionary<char, int> { { 'A', 1 }, { 'B', 2 } });

                    yield return new TestCaseData(new Hashtable { { 1, "A" } }, new Hashtable { { 1, "a" }, { 2, "b" } });
                    yield return new TestCaseData(new Hashtable { { 2, 'b' } }, new Hashtable { { 1, 'A' }, { 2, 'B' } });
                    yield return new TestCaseData(new Hashtable { { "A", 1 } }, new Hashtable { { "b", 2 }, { "a", 1 } });
                    yield return new TestCaseData(new Hashtable { { 'a', 1 } }, new Hashtable { { 'A', 1 }, { 'B', 2 } });
                }
            }
        }

        [Test]
        public void IsSuperSetHonorsUsingWhenCollectionsAreOfDifferentTypes()
        {
            ICollection set = new SimpleObjectCollection("2", "3");
            ICollection superSet = new SimpleObjectCollection(1, 2, 3, 4, 5);

            Assert.That(superSet, Is.SupersetOf(set).Using<int, string>((i, s) => i.ToString() == s));
        }
    }
}
