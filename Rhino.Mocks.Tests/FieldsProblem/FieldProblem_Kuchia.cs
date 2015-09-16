using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Kuchia : IDisposable
    {
        private IProblem _problem;
        private IDaoFactory _daoFactory;
        private IBLFactory _blFactory;

        [Fact]
        public void Method1_CallWithMocks_Returns10()
        {
            int result = Problem.Method1();
            _daoFactory.VerifyAllExpectations();
            _blFactory.VerifyAllExpectations();
            Assert.Equal(10, result);
        }

        public IDaoFactory DaoFactoryMock
        {
            get
            {
                _daoFactory = _daoFactory ?? MockRepository.GenerateStrictMock<IDaoFactory>();
                return _daoFactory;
            }
        }

        public IBLFactory BLFactoryMock
        {
            get
            {
                _blFactory = _blFactory ?? MockRepository.GenerateStrictMock<IBLFactory>();
                return _blFactory;
            }
        }


        public IProblem Problem
        {
            get
            {
                _problem = _problem ?? new Problem(BLFactoryMock, DaoFactoryMock);
                return _problem;
            }

        }

        public void Dispose()
        {
            _problem = null;
            _blFactory = null;
            _daoFactory = null;
        }
    }

    public interface IBLFactory
    {
    }

    public interface IDaoFactory
    {
    }

    public interface IProblem
    {
        int Method1();
    }

    public class Problem : BaseProblem, IProblem
    {
        public Problem(IBLFactory blFactory, IDaoFactory daoFactory)
            : base(blFactory, daoFactory)
        {
        }

        public int Method1()
        {
            return 10;
        }
    }

    public abstract class BaseProblem
    {
        private IBLFactory _blFactory;
        private IDaoFactory _daoFactory;

        public BaseProblem(IBLFactory blFactory, IDaoFactory daoFactory)
        {
            _blFactory = blFactory;
            _daoFactory = daoFactory;
        }
    }
}