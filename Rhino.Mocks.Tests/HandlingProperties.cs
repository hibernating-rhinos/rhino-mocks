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
    public class HandlingProperties
    {
        IDemo demo;

		public HandlingProperties()
        {
            demo = (IDemo)MockRepository.GenerateStrictMock(typeof(IDemo), null, null);
        }

        [Fact]
        public void PropertyBehaviorForSingleProperty()
        {
            demo.Expect(x => x.Prop).PropertyBehavior();
            for (int i = 0; i < 49; i++)
            {
                demo.Prop = "ayende" + i;
                Assert.Equal("ayende" + i, demo.Prop);
            }
            demo.VerifyAllExpectations();
        }

        [Fact]
        public void ExceptionIfLastMethodCallIsNotProperty()
        {
        	var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => x.EnumNoArgs()).PropertyBehavior());
        	Assert.Equal("Last method call was not made on a setter or a getter", ex.Message);
        }

    	[Fact]
        public void ExceptionIfPropHasOnlyGetter()
    	{
    		var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => x.ReadOnly).PropertyBehavior());
    		Assert.Equal("Property must be read/write", ex.Message);
    	}

    	[Fact]
        public void ExceptionIfPropHasOnlySetter()
    	{
    		var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => x.WriteOnly).PropertyBehavior());
    		Assert.Equal("Property must be read/write", ex.Message);
    	}

    	[Fact]
        public void IndexedPropertiesSupported()
        {
            IWithIndexers x = (IWithIndexers)MockRepository.GenerateStrictMock(typeof(IWithIndexers), null, null);
            x.Expect(y => y[1]).PropertyBehavior();
            x.Expect(y => y["",1]).PropertyBehavior();

            x[1] = 10;
            x[10] = 100;
            Assert.Equal(10, x[1]);
            Assert.Equal(100, x[10]);

            x["1", 2] = "3";
            x["2", 3] = "5";
            Assert.Equal("3", x["1", 2]);
            Assert.Equal("5", x["2", 3]);

            x.VerifyAllExpectations();
        }

        [Fact]
        public void IndexPropertyWhenValueTypeAndNotFoundThrows()
        {
            IWithIndexers x = (IWithIndexers)MockRepository.GenerateStrictMock(typeof(IWithIndexers), null, null);
            x.Expect(y => y[1]).PropertyBehavior();
        	var ex = Assert.Throws<InvalidOperationException>(() => GC.KeepAlive(x[1]));
        	Assert.Equal("Can't return a value for property Item because no value was set and the Property return a value type.", ex.Message);
        }

        [Fact]
        public void IndexPropertyWhenRefTypeAndNotFoundReturnNull()
        {
            IWithIndexers x = (IWithIndexers)MockRepository.GenerateStrictMock(typeof(IWithIndexers), null, null);
            x.Expect(y => y["",3]).PropertyBehavior();
            Assert.Null(x["", 2]);
        }

        public interface IWithIndexers
        {
            int this[int x] { get; set; }

            string this[string n, int y] { get; set; } 
        }
    }
}
