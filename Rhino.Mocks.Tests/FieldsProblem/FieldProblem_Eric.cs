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
namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using Xunit;
	using Rhino.Mocks.Constraints;

	public class ClassWithFinalizer
	{
		~ClassWithFinalizer()
		{
			Console.WriteLine(5);
		}
	}

	public class FieldProblem_Eric
	{
		[Fact]
		public void MockAClassWithFinalizer()
		{
			ClassWithFinalizer withFinalizer = (ClassWithFinalizer) MockRepository.GenerateStrictMock(typeof(ClassWithFinalizer), null, null);
			withFinalizer.VerifyAllExpectations(); //move it to verify state
			withFinalizer = null; // abandon the variable, will make it avialable for GC.
			GC.WaitForPendingFinalizers();
		}

		public class Class1Test
		{
			[Fact]
			public void ThisWorks()
			{
				IFoo mockFoo = MockRepository.GenerateStrictMock<IFoo>();
				int junk = 3;
				mockFoo.Expect(x => x.foo(ref junk)).
					IgnoreArguments().
					Constraints(Is.Anything()).
					OutRef(3).
					Repeat.Once().
					Return(true);
				ClassUnderTest cut = new ClassUnderTest();
				Assert.Equal(3, cut.doit(mockFoo));
				mockFoo.VerifyAllExpectations();
			}

			[Fact]
			public void ThisDoesnt()
			{
				IFoo mockFoo = MockRepository.GenerateStrictMock<IFoo>();
				int junk = 3;
				mockFoo.Expect(x => x.foo(ref junk)).
					IgnoreArguments().
					OutRef(3).
					Constraints(Is.Anything()).
					Repeat.Once().
					Return(true);
				ClassUnderTest cut = new ClassUnderTest();
				Assert.Equal(3, cut.doit(mockFoo));
				mockFoo.VerifyAllExpectations();
			}
		}

		#region Nested type: ClassUnderTest

		public class ClassUnderTest
		{
			public int doit(IFoo fooer)
			{
				int results = 0;
				if (fooer.foo(ref results))
					return results;
				else
					return -1;
			}
		}

		#endregion

		#region Nested type: IFoo

		public interface IFoo
		{
			bool foo(ref int fooSquared);
		}

		#endregion
	}
}