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
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks.Tests
{

	public class MockRepositoryTests
	{
		private IDemo demo;

		public MockRepositoryTests()
		{
			demo = MockRepository.GenerateStrictMock(typeof(IDemo), null, null) as IDemo;
		}

		[Fact]
		public void CreatesNewMockObject()
		{
			Assert.NotNull(demo);
		}

		[Fact]
		public void CallMethodOnMockObject()
		{
			demo.Expect(x => x.ReturnStringNoArgs());
		}

		[Fact]
		public void RecordWithBadReplayCauseException()
		{
			demo.Expect(x => x.ReturnStringNoArgs()).Return(null);
			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VerifyAllExpectations());
			Assert.Equal("IDemo.ReturnStringNoArgs(); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void RecordTwoMethodsButReplayOneCauseException()
		{
			demo.Expect(x => x.ReturnStringNoArgs()).Return(null).Repeat.Twice();
			demo.ReturnStringNoArgs();
			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VerifyAllExpectations());
			Assert.Equal("IDemo.ReturnStringNoArgs(); Expected #2, Actual #1.", ex.Message);
		}

		[Fact]
		public void CallingReplayOnNonMockThrows()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => (new object()).Replay());
			Assert.Equal("The object 'System.Object' is not a mocked object.", ex.Message);
		}

		[Fact]
		public void CallingVerifyOnNonMockThrows()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => (new object()).VerifyAllExpectations());
			Assert.Equal("The object 'System.Object' is not a mocked object.", ex.Message);
		}

		[Fact]
		public void TryingToReplayMockMoreThanOnceDoesNotThrow()
		{
			this.demo.Replay();
			this.demo.Replay();
		}

		[Fact]
		public void CallingVerifyWithoutReplayFirstCauseException()
		{
			this.demo.BackToRecord(BackToRecordOptions.All);
			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.VerifyAllExpectations());
			Assert.Equal("This action is invalid when the mock object {Rhino.Mocks.Tests.IDemo} is in record state.", ex.Message);
		}

		[Fact]
		public void UsingVerifiedObjectThrows()
		{
			demo.Replay();
			demo.VerifyAllExpectations();
			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.ReturnIntNoArgs());
			Assert.Equal("This action is invalid when the mock object is in verified state.", ex.Message);
		}

		[Fact]
		public void NotClosingMethodBeforeReplaying()
		{
			demo.Expect(x => x.StringArgString(""));
			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.StringArgString(""));
			Assert.Equal("Method 'IDemo.StringArgString(\"\");' requires a return value or an exception to throw.", ex.Message);
		}

		[Fact]
		public void GetmocksFromProxy()
		{
			IMockedObject mockedObj = demo as IMockedObject;
			Assert.NotNull(mockedObj);
			MockRepository MockRepository = mockedObj.Repository;
			Assert.NotNull(MockRepository);
		}

		[Fact]
		public void CallingLastCallWithoutHavingLastCallThrows()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => x));
			Assert.Equal("Invalid call, the last call has been used or no call has been made (make sure that you are calling a virtual (C#) / Overridable (VB) method).", ex.Message);
		}

		[Fact]
		public void SetReturnValue()
		{
			string retVal = "Ayende";
			demo.Expect(x => x.ReturnStringNoArgs()).Return(retVal);
			Assert.Equal(retVal, demo.ReturnStringNoArgs());
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void SetReturnValueAndNumberOfRepeats()
		{
			string retVal = "Ayende";
			demo.Expect(x => x.ReturnStringNoArgs()).Return(retVal).Repeat.Twice();
			Assert.Equal(retVal, demo.ReturnStringNoArgs());
			Assert.Equal(retVal, demo.ReturnStringNoArgs());
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void SetMethodToThrow()
		{
			demo.Expect(x => x.VoidStringArg("test")).Throw(new ArgumentException("Reserved value, must be zero"));
			var ex = Assert.Throws<ArgumentException>(() => this.demo.VoidStringArg("test"));
			Assert.Equal("Reserved value, must be zero", ex.Message);
		}

		[Fact]
		public void SettingMethodToThrowTwice()
		{
			string exceptionMessage = "Reserved value, must be zero";
			demo.Expect(x => x.VoidStringArg("test")).Throw(new ArgumentException(exceptionMessage)).Repeat.Twice();

			for (int i = 0; i < 2; i++)
			{
				try
				{
					demo.VoidStringArg("test");
					Assert.False(true, "Expected exception");
				}
				catch (ArgumentException e)
				{
					Assert.Equal(exceptionMessage, e.Message);
				}
			}
		}

		[Fact]
		public void ReturnningValueType()
		{
			demo.Expect(x => x.ReturnIntNoArgs()).Return(2);
			Assert.Equal(2, demo.ReturnIntNoArgs());
		}

		[Fact]
		public void CallingSecondMethodWithoutSetupRequiredInfoOnFirstOne()
		{
			demo.Expect(x => x.ReturnIntNoArgs());
			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.ReturnIntNoArgs());
			Assert.Equal("Method 'IDemo.ReturnIntNoArgs();' requires a return value or an exception to throw.", ex.Message);
		}

		[Fact]
		public void ReturnDerivedType()
		{
			demo.Expect(x => x.EnumNoArgs()).Return(DemoEnum.Demo);
		}

		[Fact]
		public void SetExceptionAndThenSetReturn()
		{
			demo.Expect(x => x.EnumNoArgs()).Throw(new Exception());
			demo.Expect(x => x.EnumNoArgs()).Return(DemoEnum.Demo);

			try
			{
				demo.EnumNoArgs();
				Assert.False(true, "Expected exception");
			}
			catch (Exception)
			{
			}
			DemoEnum d = (DemoEnum)this.demo.EnumNoArgs();
			Assert.Equal(d, DemoEnum.Demo);
		}

		[Fact]
		public void SetReturnValueAndExceptionThrows()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => demo.Expect(x => x.EnumNoArgs()).Throw(new Exception()).Return(DemoEnum.Demo));
			Assert.Equal("Can set only a single return value or exception to throw or delegate to execute on the same method call.", ex.Message);
		}

		[Fact]
		public void SetExceptionAndThenThrows()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => demo.Expect(x => x.EnumNoArgs()).Throw(new Exception()).Return(DemoEnum.Demo));
			Assert.Equal("Can set only a single return value or exception to throw or delegate to execute on the same method call.", ex.Message);
		}

		[Fact]
		public void SetTwoReturnValues()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => demo.Expect(x => x.EnumNoArgs()).Return(DemoEnum.Demo).Return(DemoEnum.Demo));
			Assert.Equal("Can set only a single return value or exception to throw or delegate to execute on the same method call.", ex.Message);
		}

		[Fact]
		public void SetTwoExceptions()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => demo.Expect(x => x.EnumNoArgs()).Throw(new Exception()).Throw(new Exception()));
			Assert.Equal("Can set only a single return value or exception to throw or delegate to execute on the same method call.", ex.Message);
		}

		[Fact]
		public void ExpectMethodOnce()
		{
			demo.Expect(x => x.EnumNoArgs()).Return(DemoEnum.NonDemo).Repeat.Once();
			DemoEnum d = (DemoEnum)demo.EnumNoArgs();
			Assert.Equal(d, DemoEnum.NonDemo);
			try
			{
				demo.EnumNoArgs();
				Assert.False(true, "Expected exception");
			}
			catch (ExpectationViolationException e)
			{
				Assert.Equal("IDemo.EnumNoArgs(); Expected #1, Actual #2.", e.Message);
			}
		}

		[Fact]
		public void ExpectMethodAlways()
		{
			demo.Expect(x => x.EnumNoArgs()).Return(DemoEnum.NonDemo).Repeat.Any();
			demo.EnumNoArgs();
			demo.EnumNoArgs();
			demo.EnumNoArgs();
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void DifferentArgumentsCauseException()
		{
			demo.Expect(x => x.VoidStringArg("Hello"));
			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VoidStringArg("World"));
			Assert.Equal("IDemo.VoidStringArg(\"World\"); Expected #0, Actual #1.\r\nIDemo.VoidStringArg(\"Hello\"); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void VerifyingArguments()
		{
			demo.Expect(x => x.VoidStringArg("Hello"));
			demo.VoidStringArg("Hello");
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void IgnoreArgument()
		{
			demo.Expect(x => x.VoidStringArg("Hello")).IgnoreArguments();
			demo.VoidStringArg("World");
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void IgnoreArgsAndReturnValue()
		{
			string objToReturn = "World";
			demo.Expect(x => x.StringArgString("Hello")).IgnoreArguments().Repeat.Twice().Return(objToReturn);
			Assert.Equal(objToReturn, demo.StringArgString("foo"));
			Assert.Equal(objToReturn, demo.StringArgString("bar"));
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void RepeatThreeTimes()
		{
			string objToReturn = "World";
			demo.Expect(x => x.StringArgString("Hello")).IgnoreArguments().Repeat.Times(3).Return(objToReturn);
			Assert.Equal(objToReturn, demo.StringArgString("foo"));
			Assert.Equal(objToReturn, demo.StringArgString("bar"));
			Assert.Equal(objToReturn, demo.StringArgString("bar"));
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void RepeatOneToThreeTimes()
		{
			string objToReturn = "World";
			demo.Expect(x => x.StringArgString("Hello")).IgnoreArguments().Repeat.Times(1, 3).Return(objToReturn);
			Assert.Equal(objToReturn, demo.StringArgString("foo"));
			Assert.Equal(objToReturn, demo.StringArgString("bar"));
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void ThrowingExceptions()
		{
			demo.Expect(x => x.StringArgString("Ayende")).Throw(new Exception("Ugh! It's alive!")).IgnoreArguments();
			var ex = Assert.Throws<Exception>(() => this.demo.StringArgString(null));
			Assert.Equal("Ugh! It's alive!", ex.Message);
		}

		[Fact]
		public void ExpectationExceptionWhileUsingDisposableThrowTheCorrectExpectation()
		{
			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VoidNoArgs());
			Assert.Equal("IDemo.VoidNoArgs(); Expected #0, Actual #1.", ex.Message);
		}

		[Fact]
		public void MockObjectThrowsForUnexpectedCall()
		{
			IDemo demo = (IDemo)MockRepository.GenerateStrictMock(typeof(IDemo), null, null);
			var ex = Assert.Throws<ExpectationViolationException>(() => demo.VoidNoArgs());
			Assert.Equal("IDemo.VoidNoArgs(); Expected #0, Actual #1.", ex.Message);
		}



		[Fact]
		public void MockObjectThrowsForUnexpectedCall_WhenVerified_IfFirstExceptionWasCaught()
		{
			IDemo demo = (IDemo)MockRepository.GenerateStrictMock(typeof(IDemo), null, null);
			try
			{
				demo.VoidNoArgs();
			}
			catch (Exception) { }
			var ex = Assert.Throws<ExpectationViolationException>(() => demo.VerifyAllExpectations());
			Assert.Equal("IDemo.VoidNoArgs(); Expected #0, Actual #1.", ex.Message);
		}

		[Fact]
		public void DyamicMockAcceptUnexpectedCall()
		{
			IDemo demo = (IDemo)MockRepository.GenerateMock(typeof(IDemo), null, null);
			demo.VoidNoArgs();
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void RepositoryThrowsWithConstructorArgsForMockInterface()
		{
			Assert.Throws<ArgumentException>(() => MockRepository.GenerateStrictMock(typeof(IDemo), null, "Foo"));
		}

		[Fact]
		public void RepositoryThrowsWithConstructorArgsForMockDelegate()
		{
			Assert.Throws<ArgumentException>(() => MockRepository.GenerateStrictMock(typeof(EventHandler), null, "Foo"));
		}

		[Fact]
		public void RepositoryThrowsWithWrongConstructorArgsForMockClass()
		{
			// There is no constructor on object that takes a string
			// parameter, so this should fail.
			try
			{
				object o = MockRepository.GenerateStrictMock(typeof(object), null, "Foo");

				Assert.False(true, "The above call should have failed");
			}
			catch (ArgumentException argEx)
			{
				Assert.Contains("Can not instantiate proxy of class: System.Object.", argEx.Message);
			}
		}

		[Fact]
		public void GenerateMockForClassWithNoDefaultConstructor()
		{
			Assert.NotNull(MockRepository.GenerateMock<ClassWithNonDefaultConstructor>(null, 0));
		}

		[Fact]
		public void GenerateMockForClassWithDefaultConstructor()
		{
			Assert.NotNull(MockRepository.GenerateMock<ClassWithDefaultConstructor>());
		}

		[Fact]
		public void GenerateMockForInterface()
		{
			Assert.NotNull(MockRepository.GenerateMock<IDemo>());
		}

		[Fact]
		public void GenerateStrictMockWithRemoting()
		{
			IDemo mock = MockRepository.GenerateStrictMockWithRemoting<IDemo>();
			Assert.NotNull(mock);
			Assert.True(mock.GetMockRepository().IsInReplayMode(mock));
		}

		[Fact]
		public void GenerateDynamicMockWithRemoting()
		{
			IDemo mock = MockRepository.GenerateDynamicMockWithRemoting<IDemo>();
			Assert.NotNull(mock);
			Assert.True(mock.GetMockRepository().IsInReplayMode(mock));
		}

		public class ClassWithNonDefaultConstructor
		{
			public ClassWithNonDefaultConstructor(string someString, int someInt) { }
		}
		public class ClassWithDefaultConstructor { }

		#region Implementation

		private enum DemoEnum
		{
			Demo,
			NonDemo
		}

		#endregion
	}
}
