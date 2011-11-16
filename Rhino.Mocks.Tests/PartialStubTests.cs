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

namespace Rhino.Mocks.Tests
{
    public class PartialStubTests
    {
        MockRepository mocks;
        AbstractClass abs;

        public PartialStubTests()
        {
            mocks = new MockRepository();
            abs = (AbstractClass)mocks.PartialStub(typeof(AbstractClass));
        }

        [Fact]
        public void AutomaticallCallBaseMethodIfNoExpectationWasSet()
        {
            mocks.ReplayAll();
            Assert.Equal(1, abs.Increment());
            Assert.Equal(6, abs.Add(5));
            Assert.Equal(6, abs.Count);
        }

        [Fact]
        public void CanStubVirtualMethods()
        {
            Expect.Call(abs.Increment()).Return(5);
            Expect.Call(abs.Add(2)).Return(3);
            mocks.ReplayAll();
            Assert.Equal(5, abs.Increment());
            Assert.Equal(3, abs.Add(2));
            Assert.Equal(0, abs.Count);
        }

        [Fact]
        public void CanStubAbstractMethods()
        {
            Expect.Call(abs.Decrement()).Return(5);
            mocks.ReplayAll();
            Assert.Equal(5, abs.Decrement());
            Assert.Equal(0, abs.Count);
        }

        [Fact]
        public void CantCreatePartialStubFromInterfaces()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => new MockRepository().PartialStub(typeof(IDemo)));
            Assert.Equal("Can't create a partial stub from an interface", ex.Message);
        }

        [Fact]
        public void CallAnAbstractMethodWithoutSettingExpectation()
        {
            mocks.ReplayAll();
            Assert.Equal(0, this.abs.Decrement());
        }

        [Fact]
        public void CanStubWithCtorParams()
        {
            WithParameters withParameters = mocks.PartialStub<WithParameters>(1);
            withParameters.Int = 4;
            mocks.ReplayAll();
            Assert.Equal(4, withParameters.Int);
        }
    }
}
