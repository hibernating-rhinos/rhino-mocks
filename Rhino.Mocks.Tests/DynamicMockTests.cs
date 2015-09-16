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
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Tests
{
	public class DynamicMockTests : IDisposable
	{
		IDemo demo;
		private bool doNotVerifyOnTearDown ;

		public DynamicMockTests()
		{
			doNotVerifyOnTearDown = false;
			demo = (IDemo)MockRepository.GenerateMock(typeof(IDemo), null, null);
		}

		public void Dispose()
		{
			if(!doNotVerifyOnTearDown)
				demo.VerifyAllExpectations();
		}

		[Fact]
		public void CanCallUnexpectedMethodOnDynamicMock()
		{
			Assert.Equal(0,demo.ReturnIntNoArgs());
		}

		[Fact]
		public void CanSetupExpectations()
		{
			demo.Expect(x => x.ReturnIntNoArgs()).Return(30).Repeat.Once();
			Assert.Equal(30,demo.ReturnIntNoArgs());
			Assert.Equal(0,demo.ReturnIntNoArgs());
		}

		[Fact]
		public void ExpectationExceptionWithDynamicMock()
		{
			demo.Expect(x => x.ReturnIntNoArgs()).Return(30);
			Assert.Null(demo.ReturnStringNoArgs());
			doNotVerifyOnTearDown = true;
			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VerifyAllExpectations());
			Assert.Equal("IDemo.ReturnIntNoArgs(); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void SetupResultWorksWithDynamicMocks()
		{
			demo.Expect(x => x.StringArgString("Ayende")).Return("Rahien");
			for (int i = 0; i < 43; i++)
			{
				Assert.Equal("Rahien",demo.StringArgString("Ayende"));
				Assert.Null(demo.StringArgString("another"));
			}
		}

		[Fact]
		public void ExpectNeverForDyanmicMock()
		{
			demo.Expect(x => x.ReturnIntNoArgs()).Repeat.Never();
		}

		[Fact]
		public void ExpectNeverForDyanmicMockThrowsIfOccurs()
		{
			demo.Expect(x => x.ReturnIntNoArgs()).Repeat.Never();
			doNotVerifyOnTearDown = true;
			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.ReturnIntNoArgs());
			Assert.Equal("IDemo.ReturnIntNoArgs(); Expected #0, Actual #1.", ex.Message);
		}
	}
}
