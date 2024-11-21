namespace SmartHome.BusinessLogic.GenericRepositoryInterface;
public interface IUpdateMultipleElementsRepository<T>
{
    IList<T>? UpdateMultiplElements(List<T> updatedEntity);
}
