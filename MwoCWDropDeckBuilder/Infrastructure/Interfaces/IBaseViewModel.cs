namespace MwoCWDropDeckBuilder.Infrastructure.Interfaces
{
    public interface IBaseViewModel<TModelType>
    {
        TModelType Model { get; }
    }
}