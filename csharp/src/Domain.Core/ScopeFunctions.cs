namespace Domain.Core
{
    public static class ScopeFunctions
    {
        public static T Let<T>(this T obj, Action<T> action)
        {
            if (!EqualityComparer<T>.Default.Equals(obj, default(T))) action(obj);
            return obj;
        }
    }
}