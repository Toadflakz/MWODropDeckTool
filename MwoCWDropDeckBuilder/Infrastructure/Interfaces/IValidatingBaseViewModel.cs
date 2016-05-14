using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MwoCWDropDeckBuilder.Infrastructure.Interfaces
{
    public interface IValidatingBaseViewModel : IDataErrorInfo
    {
        bool IsValid();
        bool IsValid<T>(Expression<Func<T>> propertyExpression);
    }
}