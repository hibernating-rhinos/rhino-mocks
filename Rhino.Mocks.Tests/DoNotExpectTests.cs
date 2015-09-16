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

using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Tests
{
    using Xunit;

    public class DoNotExpectTests
    {
        private IDemo demo;

		public DoNotExpectTests()
        {
            demo = MockRepository.GenerateMock<IDemo>();
        }

        [Fact]
        public void ShouldNotExpect()
        {
            demo.Expect(x => x.StringArgString("Ayende")).Repeat.Never();
        	var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.StringArgString("Ayende"));
        	Assert.Equal("IDemo.StringArgString(\"Ayende\"); Expected #0, Actual #1.", ex.Message);
        }

        [Fact]
        public void CanUseAnonymousDelegatesToCallVoidMethods()
        {
            demo.Expect(x => x.VoidNoArgs()).Repeat.Never();
          var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VoidNoArgs());
        	Assert.Equal("IDemo.VoidNoArgs(); Expected #0, Actual #1.", ex.Message);
        }

        [Fact]
        public void CanUseAnonymousDelegatesToCallVoidMethods_WithStringArg()
        {
            demo.Expect(x => x.VoidStringArg("Ayende")).Repeat.Never();
          var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VoidStringArg("Ayende"));
        	Assert.Equal("IDemo.VoidStringArg(\"Ayende\"); Expected #0, Actual #1.", ex.Message);
        }

        [Fact]
        public void DoNotExpectCallRespectsArguments()
        {
            demo.Expect(x => x.StringArgString("Ayende")).Repeat.Never();
            demo.StringArgString("Sneal");
        	var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.StringArgString("Ayende"));
        	Assert.Equal("IDemo.StringArgString(\"Ayende\"); Expected #0, Actual #1.", ex.Message);
        }

        [Fact]
        public void CanUseDoNotCallOnPropertySet()
        {
            demo.Expect(x => x.Prop = "Ayende").Repeat.Never();
        	var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.Prop = "Ayende");
        	Assert.Equal("IDemo.set_Prop(\"Ayende\"); Expected #0, Actual #1.", ex.Message);
        }

        [Fact]
        public void CanUseDoNotCallOnPropertyGet()
        {
            demo.Expect(x => { var y = x.Prop; }).Repeat.Never();
        	var ex = Assert.Throws<ExpectationViolationException>(() => { string soItCompiles = this.demo.Prop; });
        	Assert.Equal("IDemo.get_Prop(); Expected #0, Actual #1.", ex.Message);
        }
    }
}
