namespace FluentData.Core
{
    public interface IParameterValue
    {
        TParameterType ParameterValue<TParameterType>(string outputParameterName);

        object? ParameterValue(string name, bool isFluentType = true);
    }
}