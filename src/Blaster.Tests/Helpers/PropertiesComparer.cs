using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Blaster.Tests.Helpers
{
    public class PropertiesComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool>[] _comparers;

        public PropertiesComparer(params Expression<Func<T, object>>[] propertySelectors)
        {
            if (propertySelectors.Length == 0)
            {
                throw new Exception($"No property selectors specified in {this.GetType().Name}!");
            }

            _comparers = propertySelectors
                .Select(CreateComparer)
                .ToArray();
        }

        private static Func<T, T, bool> CreateComparer(Expression<Func<T, object>> picker)
        {
            return (left, right) =>
            {
                var valueExtractor = picker.Compile();

                var leftValue = valueExtractor(left);
                var rightValue = valueExtractor(right);

                return leftValue.Equals(rightValue);
            };
        }

        public bool Equals(T x, T y)
        {
            return _comparers
                .Select(comparer => comparer(x, y))
                .All(hmm => hmm);
        }

        public int GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}