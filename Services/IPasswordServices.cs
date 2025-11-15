namespace Services
{
    public interface IPasswordServices
    {
        int GetPasswordScore(string password);
    }
}