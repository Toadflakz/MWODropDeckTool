using System.Collections.Generic;
using MwoCWDropDeckBuilder.Infrastructure.Interfaces;

namespace MwoCWDropDeckBuilder.ViewModel
{
    public interface IFilterViewModel<T> : IFilterViewModel
    {
        IEnumerable<T> ApplyFilter(IEnumerable<T> collection);
        bool PassFilterConditions(T item);
    }

    public interface IFilterViewModel : IValidatingBaseViewModel
    {
        bool IsPreFilter { get; }
        bool IsEnabled { get; }
    }
}