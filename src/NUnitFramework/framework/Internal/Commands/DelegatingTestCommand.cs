// ***********************************************************************
// Copyright (c) 2010 Charlie Poole, Rob Prouse
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
using System.Threading;

namespace NUnit.Framework.Internal.Commands
{
    /// <summary>
    /// DelegatingTestCommand wraps an inner TestCommand.
    /// Derived classes may do what they like before or
    /// after running the inner command.
    /// </summary>
    public abstract class DelegatingTestCommand : TestCommand
    {
        /// <summary>TODO: Documentation needed for field</summary>
#pragma warning disable IDE1006
        // ReSharper disable once InconsistentNaming
        // Disregarding naming convention for back-compat
        protected TestCommand innerCommand;
#pragma warning restore IDE1006

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatingTestCommand"/> class.
        /// </summary>
        /// <param name="innerCommand">The inner command.</param>
        protected DelegatingTestCommand(TestCommand innerCommand)
            : base(innerCommand.Test)
        {
            this.innerCommand = innerCommand;
        }

        /// <summary>
        /// Runs the test with exception handling.
        /// </summary>
        protected static void RunTestMethodInThreadAbortSafeZone(TestExecutionContext context, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
#if THREAD_ABORT
                if (ex is ThreadAbortException)
                    Thread.ResetAbort();
#endif
                context.CurrentResult.RecordException(ex);
            }
        }

        internal override void OnBeforeTest(TestExecutionContext context)
        {
            base.OnBeforeTest(context);
            this.innerCommand?.OnBeforeTest(context);
        }

        internal override void OnAfterTest(TestExecutionContext context)
        {
            base.OnAfterTest(context);
            this.innerCommand?.OnAfterTest(context);
        }
    }
}
