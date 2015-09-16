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
using Rhino.Mocks.Exceptions;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks.Tests.FieldsProblem;
using Xunit;

namespace Rhino.Mocks.Tests
{
	public class BackToRecord
	{
		[Fact]
		public void CanMoveToRecordAndThenReplay()
		{
			IDemo demo = (IDemo)MockRepository.GenerateStrictMock(typeof(IDemo), null, null);
			demo.Expect(x => x.Prop).Return("ayende");
			demo.Replay();
			Assert.Equal("ayende", demo.Prop);
			demo.BackToRecord();
			demo.Expect(x => x.Prop).Return("rahien");
			demo.Replay();
			Assert.Equal("rahien", demo.Prop);
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void CanMoveToRecordFromVerified()
		{
			IDemo demo = (IDemo)MockRepository.GenerateStrictMock(typeof(IDemo), null, null);
			demo.Expect(x => x.Prop).Return("ayende");

			demo.Replay();
			Assert.Equal("ayende", demo.Prop);
			demo.VerifyAllExpectations();

			demo.BackToRecord();

			demo.Expect(x => x.Prop).Return("rahien");
			demo.Replay();
			Assert.Equal("rahien", demo.Prop);
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void CanSpecifyClearOnlyEvents()
		{
			IWithEvent withEvent = MockRepository.GenerateStrictMock<IWithEvent>();
			bool called = false;
			IEventRaiser raiser = withEvent.Expect(x => x.Load += delegate { called = true; }).GetEventRaiser();
			withEvent.BackToRecord(BackToRecordOptions.EventSubscribers);

			raiser.Raise(this, EventArgs.Empty);

			Assert.False(called);
		}

		[Fact]
		public void CanClearOnlyOriginalMethodCalls()
		{
			AbstractClass abstractClass = MockRepository.GenerateStrictMock<AbstractClass>();
			abstractClass.Expect(x => x.Add(0)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
			abstractClass.BackToRecord(BackToRecordOptions.OriginalMethodsToCall);
			abstractClass.Replay();

			var ex = Assert.Throws<ExpectationViolationException>(() => abstractClass.Add(5));
			Assert.Equal("AbstractClass.Add(5); Expected #0, Actual #1.", ex.Message);
		}

		[Fact]
		public void CanClearOnlyPropertyBehavior()
		{
			IDemo mock = MockRepository.GenerateStrictMock<IDemo>();
			mock.Expect(x => x.Prop).PropertyBehavior();

			mock.BackToRecord(BackToRecordOptions.PropertyBehavior);

			mock.Replay();

			var ex = Assert.Throws<ExpectationViolationException>(delegate { string prop = mock.Prop; });
			Assert.Equal("IDemo.get_Prop(); Expected #0, Actual #1.", ex.Message);
		}

		[Fact]
		public void CanMoveToRecordFromReplyWithoutClearingExpectations()
		{
			IDemo mock = MockRepository.GenerateStrictMock<IDemo>();
			mock.Expect(x => x.VoidNoArgs());
			mock.Replay();

			mock.BackToRecord(BackToRecordOptions.None);

			mock.Expect(x => x.VoidNoArgs());
			mock.Replay();

			mock.VoidNoArgs();

			var ex = Assert.Throws<ExpectationViolationException>(() => mock.VerifyAllExpectations());
			Assert.Equal("IDemo.VoidNoArgs(); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void CanMoveToRecordFromVerifiedWithoutClearingExpectations()
		{
			IDemo mock = MockRepository.GenerateStrictMock<IDemo>();

			mock.Expect(x => x.VoidNoArgs());
			mock.Replay();

			mock.VoidNoArgs();
			mock.VerifyAllExpectations();

			mock.BackToRecord(BackToRecordOptions.None);
			mock.Expect(x => x.VoidNoArgs());
			mock.Replay();

			mock.VoidNoArgs();
			mock.VerifyAllExpectations();
		}
	}
}
