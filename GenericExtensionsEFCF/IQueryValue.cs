namespace GenericExtensionsEFCF
{
    public interface IQueryValue<out TValue, in TParameter>
    {
        TValue Get();
    }
}
