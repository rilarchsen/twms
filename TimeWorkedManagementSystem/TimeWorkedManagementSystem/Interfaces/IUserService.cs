namespace TimeWorkedManagementSystem.Interfaces
{
    public interface IUserService
    {
        public Guid UserId { get; }
        public void SetUserId(Guid userId);
    }
}
