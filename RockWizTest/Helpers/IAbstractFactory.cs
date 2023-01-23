namespace RockWizTest.Helpers
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}