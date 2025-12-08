namespace Services
{
    public interface IPasswordService
    {
        int GetPasswordScore(string password);
    }
}