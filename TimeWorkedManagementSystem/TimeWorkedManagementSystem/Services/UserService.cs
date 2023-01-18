using TimeWorkedManagementSystem.Interfaces;

namespace TimeWorkedManagementSystem.Services
{
    public class UserService : IUserService
    {
        public Guid UserId { get; private set; }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
    }
}
