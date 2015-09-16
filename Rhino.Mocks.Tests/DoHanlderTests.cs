﻿#region license
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
    public class DoHanlderTests
    {
        IDemo demo;

        public DoHanlderTests()
        {
            demo = (IDemo)MockRepository.GenerateStrictMock(typeof(IDemo), null, null);
        }

        [Fact]
        public void CanModifyReturnValue()
        {
            demo.Expect(x => x.EnumNoArgs()).Do(new GetDay(GetSunday));
            Assert.Equal(DayOfWeek.Sunday, demo.EnumNoArgs());
            demo.VerifyAllExpectations();
        }

        [Fact]
        public void SayHelloWorld()
        {
            INameSource nameSource = (INameSource)MockRepository.GenerateStrictMock(typeof(INameSource), null, null);
            nameSource.Expect(x => x.CreateName(null, null)).IgnoreArguments().Do(new NameSourceDelegate(Formal));
            string expected = "Hi, my name is Ayende Rahien";
            string actual = new Speaker("Ayende", "Rahien", nameSource).Introduce();
            Assert.Equal(expected, actual);
        }

        delegate string NameSourceDelegate(string first, string suranme);
        
        private string Formal(string first, string surname)
        {
            return first + " " +surname;
        }
        
        public class Speaker
        {
            private readonly string firstName;
            private readonly string surname;

            private INameSource nameSource ;

            public Speaker(string firstName, string surname, INameSource nameSource)
            {
                this.firstName = firstName;
                this.surname = surname;
                this.nameSource = nameSource;
            }
            
            public string Introduce()
            {
                string name = nameSource.CreateName(firstName, surname);
                return string.Format("Hi, my name is {0}", name);
            }
        }

        public interface INameSource
        {
            string CreateName(string firstName, string surname);
        }
        
        [Fact]
        public void CanThrow()
        {
            demo.Expect(x => x.EnumNoArgs()).Do(new GetDay(ThrowDay));
            try
            {
                demo.EnumNoArgs();
            }
            catch (ArgumentException e)
            {
                Assert.Equal("Not a day", e.Message);
            }
            demo.VerifyAllExpectations();
        }

        [Fact]
        public void InvalidReturnValueThrows()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => x.ReturnIntNoArgs()).Do(new GetDay(this.GetSunday)));
            Assert.Equal("The delegate return value should be assignable from System.Int32", ex.Message);
        }

    	[Fact]
        public void InvalidDelegateThrows()
    	{
            var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => x.ReturnIntNoArgs()).Do(new IntDelegate(this.IntMethod)));
    		Assert.Equal("Callback arguments didn't match the method arguments", ex.Message);
    	}

    	[Fact]
        public void CanOnlySpecifyOnce()
    	{
            var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => x.EnumNoArgs()).Do(new GetDay(this.ThrowDay)).Return(DayOfWeek.Saturday));
    		Assert.Equal("Can set only a single return value or exception to throw or delegate to execute on the same method call.", ex.Message);
    	}

    	public delegate DayOfWeek GetDay();

        private DayOfWeek GetSunday()
        {
            return DayOfWeek.Sunday;
        }

        private DayOfWeek ThrowDay()
        {
            throw new ArgumentException("Not a day");
        }

        public delegate int IntDelegate(int i);

        private int IntMethod(int i)
        {
            return i;
        }
    }
}
