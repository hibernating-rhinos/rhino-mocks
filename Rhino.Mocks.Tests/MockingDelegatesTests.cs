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
using System.Reflection;
using Xunit;
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Tests
{
    public delegate object ObjectDelegateWithNoParams();

    public class MockingDelegatesTests
    {
        private delegate object ObjectDelegateWithNoParams();
        private delegate void VoidDelegateWithParams(string a);
        private delegate string StringDelegateWithParams(int a, string b);
        private delegate int IntDelegateWithRefAndOutParams(ref int a, out string b);

        [Fact]
        public void CallingMockedDelegatesWithoutOn()
        {
            ObjectDelegateWithNoParams d1 = (ObjectDelegateWithNoParams)MockRepository.GenerateStrictMock(typeof(ObjectDelegateWithNoParams), null, null);
            d1.Expect(x => x()).Return(1);
            Assert.Equal(1, d1());
        }

        [Fact]
        public void MockTwoDelegatesWithTheSameName()
        {
            ObjectDelegateWithNoParams d1 = (ObjectDelegateWithNoParams)MockRepository.GenerateStrictMock(typeof(ObjectDelegateWithNoParams), null, null);
            Tests.ObjectDelegateWithNoParams d2 = (Tests.ObjectDelegateWithNoParams)MockRepository.GenerateStrictMock(typeof(Tests.ObjectDelegateWithNoParams), null, null);

            d1.Expect(x => x()).Return(1);
            d2.Expect(x => x()).Return(2);

            Assert.Equal(1, d1());
            Assert.Equal(2, d2());

            d1.VerifyAllExpectations();
            d2.VerifyAllExpectations();
        }

        [Fact]
        public void MockObjectDelegateWithNoParams()
        {
            ObjectDelegateWithNoParams d = (ObjectDelegateWithNoParams)MockRepository.GenerateStrictMock(typeof(ObjectDelegateWithNoParams), null, null);

            d.Expect(x => x()).Return("abc");
            d.Expect(x => x()).Return("def");

            Assert.Equal("abc", d());
            Assert.Equal("def", d());

            try
            {
                d();
                Assert.False(true, "Expected an expectation violation to occur.");
            }
            catch (ExpectationViolationException)
            {
                // Expected.
            }
        }

        [Fact]
        public void MockVoidDelegateWithNoParams()
        {
            VoidDelegateWithParams d = (VoidDelegateWithParams)MockRepository.GenerateStrictMock(typeof(VoidDelegateWithParams), null, null);
            d.Expect(x => x("abc"));
            d.Expect(x => x("efg"));

            d("abc");
            d("efg");

            try
            {
                d("hij");
                Assert.False(true, "Expected an expectation violation to occur.");
            }
            catch (ExpectationViolationException)
            {
                // Expected.
            }
        }

        [Fact]
        public void MockStringDelegateWithParams()
        {
            StringDelegateWithParams d = (StringDelegateWithParams)MockRepository.GenerateStrictMock(typeof(StringDelegateWithParams), null, null);

            d.Expect(x => x(1, "111")).Return("abc");
            d.Expect(x => x(2, "222")).Return("def");

            Assert.Equal("abc", d(1, "111"));
            Assert.Equal("def", d(2, "222"));

            try
            {
                d(3, "333");
                Assert.False(true, "Expected an expectation violation to occur.");
            }
            catch (ExpectationViolationException)
            {
                // Expected.
            }
        }

        [Fact]
        public void MockIntDelegateWithRefAndOutParams()
        {
            IntDelegateWithRefAndOutParams d = (IntDelegateWithRefAndOutParams)MockRepository.GenerateStrictMock(typeof(IntDelegateWithRefAndOutParams), null, null);

            int a = 3;
            string b = null;
            d.Expect(x => x(ref a, out b)).Do(new IntDelegateWithRefAndOutParams(Return1_Plus2_A));

            Assert.Equal(1, d(ref a, out b));
            Assert.Equal(5, a);
            Assert.Equal("A", b);

            try
            {
                d(ref a, out b);
                Assert.False(true, "Expected an expectation violation to occur.");
            }
            catch (ExpectationViolationException)
            {
                // Expected.
            }

        }

        [Fact]
        public void InterceptsDynamicInvokeAlso()
        {
            IntDelegateWithRefAndOutParams d = (IntDelegateWithRefAndOutParams)MockRepository.GenerateStrictMock(typeof(IntDelegateWithRefAndOutParams), null, null);

            int a = 3;
            string b = null;
            d.Expect(x => x(ref a, out b)).Do(new IntDelegateWithRefAndOutParams(Return1_Plus2_A));

            object[] args = new object[] { 3, null };
            Assert.Equal(1, d.DynamicInvoke(args));
            Assert.Equal(5, args[0]);
            Assert.Equal("A", args[1]);

            try
            {
                d.DynamicInvoke(args);
                Assert.False(true, "Expected an expectation violation to occur.");
            }
            catch (TargetInvocationException ex)
            {
                // Expected.
                Assert.True(ex.InnerException is ExpectationViolationException);
            }
        }

        [Fact]
        public void DelegateBaseTypeCannotBeMocked()
        {
        	var ex = Assert.Throws<InvalidOperationException>(() => MockRepository.GenerateStrictMock(typeof(Delegate), null, null));
        	Assert.Equal("Cannot mock the Delegate base type.", ex.Message);
        }

    	private int Return1_Plus2_A(ref int a, out string b)
        {
            a += 2;
            b = "A";
            return 1;
        }

        [Fact]
        public void GenericDelegate()
        {
            Action<int> action = MockRepository.GenerateStrictMock<Action<int>>();
            for (int i = 0; i < 10; i++)
            {
                action.Expect(x => x(i));
            }

            ForEachFromZeroToNine(action);
            action.VerifyAllExpectations();
        }

        private void ForEachFromZeroToNine(Action<int> act)
        {
            for (int i = 0; i < 10; i++)
            {
                act(i);
            }
        }
    }
}
