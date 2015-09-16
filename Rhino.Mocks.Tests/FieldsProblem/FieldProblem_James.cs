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
using System.Collections.Generic;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_James
	{
		[Fact]
		public void ShouldBeAbleToMockGenericMethod()
		{
			ILookupMapper<int> mapper = MockRepository.GenerateStrictMock<ILookupMapper<int>>();
			List<Foo<int>> retval = new List<Foo<int>>();
			retval.Add(new Foo<int>());
			mapper.Expect(x => x.FindAllFoo()).Return(retval);
			IList<Foo<int>> listOfFoo = mapper.FindAllFoo();
			mapper.VerifyAllExpectations();
		}

		[Fact]
		public void ShouldBeAbleToMockGenericMethod2()
		{
			ILookupMapper<int> mapper = MockRepository.GenerateStrictMock<ILookupMapper<int>>();
			Foo<int> retval = new Foo<int>();
			mapper.Expect(x => x.FindOneFoo()).Return(retval);
			Foo<int> oneFoo = mapper.FindOneFoo();
			mapper.VerifyAllExpectations();
		}

		[Fact]
		public void CanMockMethodsReturnIntPtr()
		{
			IFooWithIntPtr mock = MockRepository.GenerateStrictMock<IFooWithIntPtr>();
			mock.Expect(x => x.Buffer(15)).Return(IntPtr.Zero);

			IntPtr buffer = mock.Buffer(15);
			Assert.Equal(IntPtr.Zero, buffer);
			mock.VerifyAllExpectations();
		}

		[Fact]
		public void ShouldGetValidErrorWhenGenericTypeMismatchOccurs()
		{
			ILookupMapper<int> mapper = MockRepository.GenerateStrictMock<ILookupMapper<int>>();
			Foo<string> retval = new Foo<string>();
			var ex = Assert.Throws<InvalidOperationException>(() => mapper.Expect<ILookupMapper<int>, object>(x => x.FindOneFoo()).Return(retval));
			Assert.Equal("Type 'Rhino.Mocks.Tests.FieldsProblem.Foo`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]' doesn't match the return type 'Rhino.Mocks.Tests.FieldsProblem.Foo`1[System.Int32]' for method 'ILookupMapper`1.FindOneFoo();'", ex.Message);
		}
	}

	public interface ILookupMapper<T>
	{
		IList<Foo<T>> FindAllFoo();
		Foo<T> FindOneFoo();
	}

	public class Foo<T>
	{
		public T GetOne()
		{
			return default(T);
		}
	}

	public interface IFooWithIntPtr
	{
		IntPtr Buffer(UInt32 index);
	}
}