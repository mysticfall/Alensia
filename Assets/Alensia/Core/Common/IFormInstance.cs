namespace Alensia.Core.Common
{
    public interface IFormInstance<out T> where T : IForm
    {
        T Form { get; }
    }
}