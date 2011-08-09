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

namespace Rhino.Mocks.Tests
{
    
    public class WithTestFixture
    {
        [Fact]
        public void SetupMyOwnRepository()
        {
            MockRepository mocks = new MockRepository();
            IDemo demo = mocks.StrictMock<IDemo>();
            With.Mocks(mocks,delegate
            {
                Expect.Call(demo.ReturnStringNoArgs()).Return("Hi");
                Mocker.Current.ReplayAll();
                Assert.Equal("Hi", demo.ReturnStringNoArgs());
            });
        }
        [Fact]
        public void UsingTheWithMocksConstruct()
        {
            With.Mocks(new MockRepository(),delegate
            {
                IDemo demo = Mocker.Current.StrictMock<IDemo>();
                Expect.Call(demo.ReturnIntNoArgs()).Return(5);
                Mocker.Current.ReplayAll();
                Assert.Equal(5, demo.ReturnIntNoArgs());
            });
        }

    	[Fact]
		public void CannotUseMockerOutsideOfWithMocks()
    	{
    		var ex = Assert.Throws<InvalidOperationException>(() => GC.KeepAlive(Mocker.Current));
    		Assert.Equal("You cannot use Mocker.Current outside of a With.Mocks block", ex.Message);
    	}

    	[Fact]
        public void UsingTheWithMocksConstruct_ThrowsIfExpectationIsMissed()
    	{
    		var ex = Assert.Throws<ExpectationViolationException>(() =>

    		                                                      With.Mocks(delegate
    		                                                                 	{
    		                                                                 		IDemo demo = Mocker.Current.StrictMock<IDemo>();
    		                                                                 		Expect.Call(demo.ReturnIntNoArgs()).Return(5);
    		                                                                 		Mocker.Current.ReplayAll();
    		                                                                 	}));
    		Assert.Equal("IDemo.ReturnIntNoArgs(); Expected #1, Actual #0.", ex.Message);
    	}

    	[Fact]
        public void UsingTheWithMocksConstruct_ThrowsIfReplayAllNotCalled()
    	{
    		var ex = Assert.Throws<InvalidOperationException>(() =>
    		                                                  	{
    		                                                  		With.Mocks(delegate
    		                                                  		           	{
    		                                                  		           		IDemo demo = Mocker.Current.StrictMock<IDemo>();
    		                                                  		           		Expect.Call(demo.ReturnIntNoArgs()).Return(5);
    		                                                  		           	});
    		                                                  	});
    		Assert.Equal("This action is invalid when the mock object {Rhino.Mocks.Tests.IDemo} is in record state.", ex.Message);
    	}


    	[Fact]
        public void UsingTheWithMocksConstruct_GiveCorrectExceptionWhenMocking()
    	{
    		var ex = Assert.Throws<IndexOutOfRangeException>(() =>
    		                                                 	{
    		                                                 		With.Mocks(delegate
    		                                                 		           	{
    		                                                 		           		IDemo demo = Mocker.Current.StrictMock<IDemo>();
    		                                                 		           		Expect.Call(demo.ReturnIntNoArgs()).Return(5);
    		                                                 		           		Mocker.Current.ReplayAll();
    		                                                 		           		throw new IndexOutOfRangeException("foo");
    		                                                 		           	});
    		                                                 	});
    		Assert.Equal("foo", ex.Message);
    	}


    	[Fact]
        public void UsingTheWithMocksConstruct_GiveCorrectExceptionWhenMockingEvenIfReplayAllNotCalled()
    	{
    		var ex = Assert.Throws<IndexOutOfRangeException>(() =>
    		                                                 With.Mocks(delegate
    		                                                            	{
    		                                                            		IDemo demo = Mocker.Current.StrictMock<IDemo>();
    		                                                            		Expect.Call(demo.ReturnIntNoArgs()).Return(5);
    		                                                            		throw new IndexOutOfRangeException("foo");
    		                                                            	}));
    		Assert.Equal("foo", ex.Message);
    	}

    	[Fact]
        public void UsingTheWithMocksExceptingVerifyConstruct()
        {
            MockRepository mocks = new MockRepository();
            IDemo demo = mocks.StrictMock<IDemo>();

            With.Mocks(mocks)
            .Expecting(delegate
            {
                Expect.Call(demo.ReturnIntNoArgs()).Return(5);
            })
            .Verify(delegate
            {
                Assert.Equal(5, demo.ReturnIntNoArgs());
            });
        }

        [Fact]
        public void UsingTheWithMocksExceptingVerifyConstruct_ThrowsIfExpectationIsMissed()
        {
            MockRepository mocks = new MockRepository();
            IDemo demo = mocks.StrictMock<IDemo>();

        	var ex = Assert.Throws<ExpectationViolationException>(() =>
        	                                                      With.Mocks(mocks)
        	                                                      	.Expecting(delegate
        	                                                      	           	{
        	                                                      	           		Expect.Call(demo.ReturnIntNoArgs()).Return(5);
        	                                                      	           	})
        	                                                      	.Verify(delegate
        	                                                      	        	{
        	                                                      	        	}));
        	Assert.Equal("IDemo.ReturnIntNoArgs(); Expected #1, Actual #0.", ex.Message);
        }

        [Fact]
        public void UsingTheWithMocksExceptingVerifyConstruct_GiveCorrectExceptionWhenMocking()
        {
            MockRepository mocks = new MockRepository();
            IDemo demo = mocks.StrictMock<IDemo>();

        	var ex = Assert.Throws<IndexOutOfRangeException>(() =>
        	                                                 With.Mocks(mocks)
        	                                                 	.Expecting(delegate
        	                                                 	           	{
        	                                                 	           		Expect.Call(demo.ReturnIntNoArgs()).Return(5);
        	                                                 	           	})
        	                                                 	.Verify(delegate
        	                                                 	        	{
        	                                                 	        		throw new IndexOutOfRangeException("foo");
        	                                                 	        	}));
        	Assert.Equal("foo", ex.Message);
        }

        [Fact]
        public void UsingTheWithMocksExceptingInSameOrderVerifyConstruct_ShouldTakeCareOfOrder()
        {
            MockRepository mocks = new MockRepository();
            IDemo demo = mocks.StrictMock<IDemo>();
            bool verificationFailed; 

            try
            {
                With.Mocks(mocks).ExpectingInSameOrder(delegate
                {
                    Expect.Call(demo.ReturnIntNoArgs()).Return(1);
                    Expect.Call(demo.ReturnStringNoArgs()).Return("2");
                })
                .Verify(delegate
                {
                    demo.ReturnStringNoArgs();
                    demo.ReturnIntNoArgs();
                });
                verificationFailed = false;
            }
            catch (ExpectationViolationException)
            {
                verificationFailed = true;
            }

            Assert.True(verificationFailed,
                "Verification was supposed to fail, because the mocks are called in the wrong order");
        }
    }
}
