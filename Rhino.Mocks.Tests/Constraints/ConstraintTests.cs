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
using System.Data;
using Rhino.Mocks.Constraints;
using Rhino.Mocks.Exceptions;
using Xunit;

namespace Rhino.Mocks.Tests.Constraints
{
	public class ConstraintTests
	{
		private IDemo demo;

		public ConstraintTests()
		{
			demo = (IDemo) MockRepository.GenerateStrictMock(typeof (IDemo), null, null);
		}

		[Fact]
		public void UsingPredicate()
		{
			demo.Expect(x => x.VoidStringArg(null)).Constraints(Is.Matching<string>(s => s.Length == 2) && Is.Matching<string>(s => s.EndsWith("b")));
			
			demo.VoidStringArg("ab");

			demo.VerifyAllExpectations();
		}

		[Fact]
		public void UsingPredicateConstraintWhenTypesNotMatching()
		{
			demo.Expect(x => x.VoidStringArg(null)).Constraints(Is.Matching<DataSet>(s => false));

			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.VoidStringArg("ab"));
			Assert.Equal("Predicate accept System.Data.DataSet but parameter is System.String which is not compatible", ex.Message);
		}

		[Fact]
		public void UsingPredicateConstraintWithSubtype()
		{
			demo.Expect(x => x.VoidStringArg(null)).Constraints(Is.Matching<object>(o => o.Equals("ab")));

			demo.VoidStringArg("ab");

			demo.VerifyAllExpectations();
		}

		[Fact]
		public void UsingPredicateWhenExpectationViolated()
		{
			demo.Expect(x => x.VoidStringArg(null)).Constraints(Is.Matching<string>(JustPredicate));

			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VoidStringArg("cc"));
			Assert.Equal("IDemo.VoidStringArg(\"cc\"); Expected #0, Actual #1.\r\nIDemo.VoidStringArg(Predicate (ConstraintTests.JustPredicate(obj);)); Expected #1, Actual #0.", ex.Message);
		}
		
		public bool JustPredicate(string s)
		{
			return false;
		}

		[Fact]
		public void AndSeveralConstraings()
		{
			AbstractConstraint all = Is.NotEqual("bar") & Is.TypeOf(typeof(string)) & Is.NotNull();
			Assert.True(all.Eval("foo"));
			Assert.Equal("not equal to bar and type of {System.String} and not equal to null", all.Message);
		}

		[Fact]
		public void AndSeveralConstraings_WithGenerics()
		{
			AbstractConstraint all = Is.NotEqual("bar") && Is.TypeOf<string>() && Is.NotNull();
			Assert.True(all.Eval("foo"));
			Assert.Equal("not equal to bar and type of {System.String} and not equal to null", all.Message);
		}

		[Fact]
		public void AndConstraints()
		{
			AbstractConstraint start = Text.StartsWith("Ayende"), end = Text.EndsWith("Rahien");
			AbstractConstraint combine = start & end;
			Assert.True(combine.Eval("Ayende Rahien"));
			Assert.Equal("starts with \"Ayende\" and ends with \"Rahien\"", combine.Message);
		}

		[Fact]
		public void NotConstraint()
		{
			AbstractConstraint start = Text.StartsWith("Ayende");
			AbstractConstraint negate = !start;
			Assert.True(negate.Eval("Rahien"));
			Assert.Equal("not starts with \"Ayende\"", negate.Message);
		}

		[Fact]
		public void OrConstraints()
		{
			AbstractConstraint start = Text.StartsWith("Ayende"), end = Text.EndsWith("Rahien");
			AbstractConstraint combine = start | end;
			Assert.True(combine.Eval("Ayende"));
			Assert.True(combine.Eval("Rahien"));
			Assert.Equal("starts with \"Ayende\" or ends with \"Rahien\"", combine.Message);
		}

		[Fact]
		public void SettingConstraintOnAMock()
		{
			demo.Expect(x => x.VoidStringArg("Ayende")).Constraints(Text.Contains("World"));
			demo.VoidStringArg("Hello, World");
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void ConstraintFailingThrows()
		{
			demo.Expect(x => x.VoidStringArg("Ayende")).Constraints(Text.Contains("World"));
			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VoidStringArg("Hello, world"));
			Assert.Equal("IDemo.VoidStringArg(\"Hello, world\"); Expected #0, Actual #1.\r\nIDemo.VoidStringArg(contains \"World\"); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void ConstraintWithTooMuchForArguments()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => demo.Expect(x => x.VoidStringArg("Ayende")).Constraints(Text.Contains("World"), Is.Equal("Rahien")));
			Assert.Equal("The number of constraints is not the same as the number of the method's parameters!", ex.Message);
		}

		[Fact]
		public void ConstraintWithTooFewForArguments()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => demo.Expect(x => x.VoidThreeArgs(1, "Ayende", 3.14f)).Constraints(Text.Contains("World"), Is.Equal("Rahien")));
			Assert.Equal("The number of constraints is not the same as the number of the method's parameters!", ex.Message);
		}

		[Fact]
		public void ConstraintsThatWerentCallCauseVerifyFailure()
		{
			this.demo.Expect(x => x.VoidStringArg("Ayende")).Constraints(Text.Contains("World"));
			var ex = Assert.Throws<ExpectationViolationException>(() => this.demo.VerifyAllExpectations());
			Assert.Equal("IDemo.VoidStringArg(contains \"World\"); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void AddConstraintAndThenTryToIgnoreArgs()
		{
			var ex = Assert.Throws<InvalidOperationException>(() => this.demo.Expect(x => x.VoidStringArg("Ayende")).Constraints(Text.Contains("World")).Callback<string>("".StartsWith));
			Assert.Equal("This method has already been set to ConstraintsExpectation.", ex.Message);
		}
	}
}
