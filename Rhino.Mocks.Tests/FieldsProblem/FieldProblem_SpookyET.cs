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

using System.IO;
using System.Net;
using System.Text;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
		public class FieldProblem_SpookyET
		{
				[Fact]
				public void MockHttpRequesteRsponse()
				{
						byte[] responseData = Encoding.UTF8.GetBytes("200 OK");
						Stream stream = new MemoryStream(responseData);
						WebRequest request = (WebRequest)MockRepository.GenerateStrictMock(typeof(WebRequest), null, null);
						WebResponse response = (WebResponse)MockRepository.GenerateStrictMock(typeof(WebResponse), null, null);
						request.Expect(x => x.GetResponse()).Return(response);
						response.Expect(x => x.GetResponseStream()).Return(stream);

						Stream returnedStream = GetResponseStream(request);

						Assert.Same(stream, returnedStream);
						string returnedString = new StreamReader(returnedStream).ReadToEnd();
						Assert.Equal("200 OK", returnedString);

						request.VerifyAllExpectations();
						response.VerifyAllExpectations();
				}

				/// <summary>
				/// Notice the ordering: First we've a Return and then IgnoreArguments, that
				/// broke because I didn't copy the returnValueSet in the expectation swapping.
				/// </summary>
				[Fact]
				public void UsingReturnAndThenIgnoreArgs()
				{
						IDemo demo = (IDemo)MockRepository.GenerateStrictMock(typeof(IDemo), null, null);
						demo.Expect(x => x.StringArgString(null)).Return("ayende").IgnoreArguments();
						Assert.Equal("ayende", demo.StringArgString("rahien"));
						demo.VerifyAllExpectations();
				}

				[Fact]
				public void WebRequestWhenDisposing()
				{
						var webRequestMock = (WebRequest)MockRepository.GenerateStrictMock(typeof(WebRequest), null, null);
						var webResponseMock = (WebResponse)MockRepository.GenerateStrictMock(typeof(WebResponse), null, null);

						webRequestMock.Expect(x => x.GetResponse()).Return(webResponseMock);
						webResponseMock.Expect(x => x.GetResponseStream()).Return(new MemoryStream());

						webResponseMock.Expect(x => x.Close());

						WebResponse response = webRequestMock.GetResponse();
						response.GetResponseStream();
						webResponseMock.Close();

						webRequestMock.AssertWasCalled(x => x.GetResponse()).Before(webResponseMock.AssertWasCalled(x => x.GetResponseStream()));

						webRequestMock.VerifyAllExpectations();
						webResponseMock.VerifyAllExpectations();
				}

				private Stream GetResponseStream(WebRequest request)
				{
						return request.GetResponse().GetResponseStream();
				}
		}
}