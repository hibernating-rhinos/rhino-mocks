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

namespace Rhino.Mocks.Tests
{
	using System;
	using Xunit;

	public class ExpectTests : IDisposable
	{
		private IDemo demo;

		public ExpectTests()
		{
			demo = MockRepository.GenerateStrictMock(typeof(IDemo), null, null) as IDemo;
		}

		public void Dispose()
		{
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void CanExpect()
		{
			demo.Expect(x => x.Prop).Return("Ayende");
			Assert.Equal("Ayende", demo.Prop);
		}

		[Fact]
		public void PassNonMock()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => new object().Expect(x => x.ToString()));
			Assert.Equal("The object 'System.Object' is not a mocked object.", ex.Message);
		}

		[Fact]
		public void CallVoidMethods()
		{
			demo.Expect(x => x.VoidNoArgs()).Throw(new ArgumentNullException());
			Assert.Throws<ArgumentNullException>(() => demo.VoidNoArgs());
		}

		[Fact]
		public void ExpectCallNormal()
		{
			demo.Expect(x => x.Prop).Return("ayende");
			Assert.Equal("ayende", demo.Prop);
		}

		[Fact]
		public void ExpectWhenNoCallMade()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => { }));
			Assert.Equal("Invalid call, the last call has been used or no call has been made (make sure that you are calling a virtual (C#) / Overridable (VB) method).", ex.Message);
			this.demo.Replay();
		}

		[Fact]
		public void ExpectOnReplay()
		{
			demo.Expect(x => x.Prop).Return("ayende");
			Assert.Equal("ayende", demo.Prop);
			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => { }));
			Assert.Equal("Invalid call, the last call has been used or no call has been made (make sure that you are calling a virtual (C#) / Overridable (VB) method).", ex.Message);
			this.demo.Replay();
		}
	}
}