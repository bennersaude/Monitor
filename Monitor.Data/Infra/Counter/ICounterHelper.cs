namespace Monitor.Data.Infra.Counter
{
    public interface ICounterHelper
    {
        long GetNextValue(string sequenceName);
    }
}