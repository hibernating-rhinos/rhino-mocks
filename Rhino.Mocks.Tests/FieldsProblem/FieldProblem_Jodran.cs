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

using Xunit;
using Rhino.Mocks.Exceptions;
using Xunit.Extensions;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Jodran
	{
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void CanUseExpectSyntax_OnStubWithOrderedExpectations(bool shouldSwitchToReplyImmediately)
		{
			var foo54 = MockRepository.GenerateStub<IFoo54>();
			if (shouldSwitchToReplyImmediately)
				foo54.Replay();

				foo54.Expect(x => x.DoSomething()).Return(0);
				foo54.Expect(x => x.DoSomethingElse());

			if (!shouldSwitchToReplyImmediately)
				foo54.Replay();

			foo54.DoSomething();
			foo54.DoSomethingElse();
		}

		[Theory, InlineData(true), InlineData(false)]
		public void CanUseExpectSyntax_OnMockWithOrderedExpectations(bool shouldSwitchToReplyImmediately)
		{
			var foo54 = MockRepository.GenerateStrictMock<IFoo54>();
			if (shouldSwitchToReplyImmediately)
				foo54.Replay();

				foo54.Expect(x => x.DoSomething()).Return(0);
				foo54.Expect(x => x.DoSomethingElse());

			if (!shouldSwitchToReplyImmediately)
				foo54.Replay();

			foo54.DoSomething();
			foo54.DoSomethingElse();

			foo54.VerifyAllExpectations();
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void CanUseExpectSyntax_OnMockWithOrderedExpectations2(bool shouldSwitchToReplyImmediately)
		{
			var foo54 = MockRepository.GenerateStrictMock<IFoo54>();
			if (shouldSwitchToReplyImmediately)
				foo54.Replay();

			foo54.Expect(x => x.DoSomething()).Return(0);
			foo54.Expect(x => x.DoSomethingElse());

			if (!shouldSwitchToReplyImmediately)
				foo54.Replay();

			foo54.DoSomethingElse();
			Assert.Throws<ExpectationViolationException>(() => foo54.AssertWasCalled(x => x.DoSomethingElse()).Before(foo54.AssertWasCalled(x => x.DoSomething())));
		}

		[Fact]
		public void ExtensionMethodsTransistionStateCorrectly()
		{
			var foo54 = MockRepository.GenerateStrictMock<IFoo54>();
			foo54.Expect(x => x.DoSomethingElse());
			foo54.DoSomethingElse(); // Satisfy first expectation

			foo54.Expect(x => x.DoSomethingElse());
			foo54.DoSomethingElse(); // Satisfy second expectation

			foo54.VerifyAllExpectations();
		}
	}
}
