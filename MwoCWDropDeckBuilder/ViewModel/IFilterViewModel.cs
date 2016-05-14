using System;
using System.Collections.Generic;
using MwoCWDropDeckBuilder.Infrastructure;
using MwoCWDropDeckBuilder.Infrastructure.Interfaces;
using MwoCWDropDeckBuilder.Model;

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