// ***********************************************************************
// Copyright (c) 2013 Charlie Poole, Rob Prouse
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

using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Filters
{
    /// <summary>
    /// IdFilter selects tests based on their id
    /// </summary>
    internal sealed class IdFilter : ValueMatchFilter
    {
        internal const string XmlElementName = "id";

        /// <summary>
        /// Construct an IdFilter for a single value
        /// </summary>
        /// <param name="id">The id the filter will recognize.</param>
        public IdFilter(string id) : base(id) { }

        /// <summary>
        /// Match a test against a single value.
        /// </summary>
        public override bool Match(ITest test)
        {
            // We make a direct test here rather than calling ValueMatchFilter.Match
            // because regular expressions are not supported for ID.
            var testId = test.Id;

            // ids usually differ from the end as we have fixed prefix like 0-
            return testId.Length == ExpectedValue.Length
                   && testId[testId.Length - 1] == ExpectedValue[testId.Length - 1]
                   && testId == ExpectedValue;
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName => XmlElementName;
    }
}
