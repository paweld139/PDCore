namespace PDCore.Builders
{
    public class Builder<T> : IBuilder<T>
    {
        protected T _object;

        public static Builder<T> Default()
        {
            return new Builder<T>();
        }

        public T Build()
        {
            return _object;
        }
    }
}
