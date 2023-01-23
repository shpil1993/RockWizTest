using System;

namespace RockWizTest.Helpers
{
    public class AbstractFactory<T> : IAbstractFactory<T>
    {
        #region Fields

        private readonly Func<T> _factory;

        #endregion

        public AbstractFactory(Func<T> factory)
        {
            _factory = factory;
        }

        #region Methods

        public T Create()
        {
            return _factory();
        }

        #endregion
    }
}
