#region license
// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion


using System;
using Xunit;
using Rhino.Mocks.Constraints;
using Rhino.Mocks.Exceptions;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks.Tests
{
	public class LastCallTests : IDisposable
	{
		private IDemo demo;
		private bool delegateWasCalled;

		[Fact]
		public void LastCallOnNonMockObjectThrows()
		{
				var ex = Assert.Throws<InvalidOperationException>(() => (new object()).Expect(x => x.ToString()));
				Assert.Equal("The object 'System.Object' is not a mocked object.", ex.Message);
		}

		[Fact]
		public void LastCallConstraints()
		{
			var demo = (IDemo)MockRepository.GenerateStrictMock(typeof (IDemo), null, null);
			demo.Expect(x => x.StringArgString("")).Constraints(Is.Null()).Return("aaa").Repeat.Twice();
			Assert.Equal("aaa", demo.StringArgString(null));

			try
			{
				demo.StringArgString("");
				Assert.False(true, "Exception expected");
			}
			catch(Exception e)
			{
				Assert.Equal("IDemo.StringArgString(\"\"); Expected #0, Actual #1.\r\nIDemo.StringArgString(equal to null); Expected #2, Actual #1.", e.Message);
			}
		}

        [Fact]
        public void LastCallCallOriginalMethod()
        {
            CallOriginalMethodFodder comf1 = (CallOriginalMethodFodder)MockRepository.GenerateMock(typeof(CallOriginalMethodFodder), null, null);
            CallOriginalMethodFodder comf2 = (CallOriginalMethodFodder)MockRepository.GenerateMock(typeof(CallOriginalMethodFodder), null, null);
            comf2.Expect(x => x.TheMethod()).CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            comf1.TheMethod();
            Assert.Equal(false, comf1.OriginalMethodCalled);

            comf2.TheMethod();
            Assert.Equal(true, comf2.OriginalMethodCalled);
        }

		[Fact]
		public void LastCallOriginalMethod_WithExpectation()
		{
			CallOriginalMethodFodder comf1 = (CallOriginalMethodFodder)MockRepository.GenerateMock(typeof(CallOriginalMethodFodder), null, null);
            CallOriginalMethodFodder comf2 = (CallOriginalMethodFodder)MockRepository.GenerateMock(typeof(CallOriginalMethodFodder), null, null);
            comf2.Expect(x => x.TheMethod()).CallOriginalMethod(OriginalCallOptions.CreateExpectation).Repeat.Twice();

            comf1.TheMethod();
            Assert.Equal(false, comf1.OriginalMethodCalled);

            comf2.TheMethod();
            Assert.Equal(true, comf2.OriginalMethodCalled);

			var ex = Assert.Throws<ExpectationViolationException>(() => comf2.VerifyAllExpectations());
			Assert.Equal("CallOriginalMethodFodder.TheMethod(); Expected #2, Actual #1.", ex.Message);
		}

        public class CallOriginalMethodFodder
        {
            private bool mOriginalMethodCalled;

	        public bool OriginalMethodCalled
	        {
		        get { return mOriginalMethodCalled;}
	        }

            public virtual void TheMethod()
            {
                mOriginalMethodCalled = true;
            }
        }

		[Fact]
		public void LastCallCallback()
		{
			demo.Expect(x => x.VoidNoArgs()).Callback(delegateCalled);
			delegateWasCalled = false;

			demo.VoidNoArgs();
			Assert.True(delegateWasCalled);
		}

		private bool delegateCalled()
		{
			delegateWasCalled = true;
			return true;
		}

		public LastCallTests()
		{
			demo = (IDemo)MockRepository.GenerateStrictMock(typeof (IDemo), null, null);
		}

		public void Dispose()
		{
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void LastCallReturn()
		{
			demo.Expect(x => x.ReturnIntNoArgs()).Return(5);
			Assert.Equal(5, demo.ReturnIntNoArgs());
		}

		[Fact]
		public void NoLastCall()
		{
			try
			{
				var ex = Assert.Throws<InvalidOperationException>(() => demo.Expect(x => x).Return(null));
				Assert.Equal("Invalid call, the last call has been used or no call has been made (make sure that you are calling a virtual (C#) / Overridable (VB) method).", ex.Message);
			}
			finally
			{
				demo.Replay(); //for the tear down
			}
		}

		[Fact]
		public void LastCallThrow()
		{
			demo.Expect(x => x.VoidNoArgs()).Throw(new Exception("Bla!"));
			var ex = Assert.Throws<Exception>(() => this.demo.VoidNoArgs());
			Assert.Equal("Bla!", ex.Message);
		}

		[Fact]
		public void LastCallRepeat()
		{
			demo.Expect(x => x.VoidNoArgs()).Repeat.Twice();
			demo.VoidNoArgs();
			demo.VoidNoArgs();
		}

		[Fact]
		public void LastCallIgnoreArguments()
		{
			demo.Expect(x => x.VoidStringArg("hello")).IgnoreArguments();
			demo.VoidStringArg("bye");
		}
	}
}
