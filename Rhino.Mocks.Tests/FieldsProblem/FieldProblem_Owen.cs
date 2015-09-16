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

using System.Data;
using Xunit;
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Tests
{
	public class Metric : IMetric
	{
		IApplicationSession m_objIApplicationSession;
		Metric(IApplicationSession pv_objIApplicationSession, DataSet pv_objDataSet)
		{
			m_objIApplicationSession = pv_objIApplicationSession;
		}

		public IMetric Numerator
		{
			get
			{
				return Metric.GetByName(m_objIApplicationSession, "Numerator");
			}
		}

		public static IMetric GetByName(IApplicationSession pv_objIApplicationSession, string pv_strMetricName)
		{
			return new Metric(pv_objIApplicationSession, pv_objIApplicationSession.IMetricBroker.FetchMetric(pv_strMetricName));
		}

		public static IMetric GetByID(IApplicationSession pv_objIApplicationSession, long pv_lMetricID)
		{
			return new Metric(pv_objIApplicationSession, pv_objIApplicationSession.IMetricBroker.FetchMetric(pv_lMetricID));
		}
	}

	public interface IMetric
	{
		IMetric Numerator { get;}
	}

	public interface IApplicationSession
	{
		IMetricBroker IMetricBroker { get;}
	}

	public interface IMetricBroker
	{
		DataSet FetchMetric(string MetricName);
		DataSet FetchMetric(long MetricID);
	}

	public class FieldProblem_Owen
	{
		IApplicationSession m_objIApplication;
		IMetricBroker m_objIMetricBroker;

		DataSet m_objDSRatioMetric;

		IMetric m_objIMetric;

		public FieldProblem_Owen()
		{
			m_objIApplication = (IApplicationSession)MockRepository.GenerateStrictMock(typeof(IApplicationSession), null, null);
			m_objIMetricBroker = (IMetricBroker)MockRepository.GenerateStrictMock(typeof(IMetricBroker), null, null);

			m_objDSRatioMetric = new DataSet();

			m_objIApplication.Expect(x => x.IMetricBroker).Return(m_objIMetricBroker);
			m_objIMetricBroker.Expect(x => x.FetchMetric("Risk")).Return(m_objDSRatioMetric);

			m_objIMetric = Metric.GetByName(m_objIApplication, "Risk");

			m_objIMetricBroker.AssertWasCalled(x => x.FetchMetric("Risk")).After(m_objIApplication.AssertWasCalled(x => x.IMetricBroker));

			m_objIApplication.VerifyAllExpectations();
			m_objIMetricBroker.VerifyAllExpectations();
		}
		[Fact]
		public void T001_SavingShouldNotInvalidateOtherCachedSingleObjects()
		{
			m_objIApplication.BackToRecord();
			m_objIMetricBroker.BackToRecord();
			long lOtherID = 200;
			DataSet objDSOtherMetric = new DataSet();

			m_objIApplication.Expect(x => x.IMetricBroker).Return(m_objIMetricBroker);
			m_objIMetricBroker.Expect(x => x.FetchMetric(lOtherID)).Return(objDSOtherMetric);

			m_objIApplication.Replay();
			m_objIMetricBroker.Replay();

			IMetric objOtherMetric = Metric.GetByID(m_objIApplication, lOtherID);
			m_objIMetricBroker.AssertWasCalled(x => x.FetchMetric(lOtherID)).After(m_objIApplication.AssertWasCalled(x => x.IMetricBroker));

			m_objIApplication.VerifyAllExpectations();
			m_objIMetricBroker.VerifyAllExpectations();

			m_objIApplication.BackToRecord();
			m_objIMetricBroker.BackToRecord();

			//missing expectations here. 
			m_objIApplication.Replay();
			m_objIMetricBroker.Replay();

			//cause a stack overflow error
			var ex = Assert.Throws<ExpectationViolationException>(() => { IMetric objMetric = this.m_objIMetric.Numerator; });
			Assert.Equal("IApplicationSession.get_IMetricBroker(); Expected #0, Actual #1.", ex.Message);
		}
	}
}