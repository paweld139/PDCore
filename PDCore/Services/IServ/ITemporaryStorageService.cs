namespace PDCore.Services.IServ
{
    public interface ITemporaryStorageService
    {
        void Deposit<T>(T o, string key);

        T Withdraw<T>(string key);
    }
}