using SchoolTransport.Domain.Enums;

namespace SchoolTransport.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set;  } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public USerRole Role {  get; private set; }
        public bool IsActive { get; private set; }
        public Guid? DriverId { get; private set; }
        public User? Driver { get; private set; }

        protected User() { }

        public User(string name, string email, string passwordHash, USerRole role, bool isActive, Guid? driverId, User? driver)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = isActive;
            DriverId = driverId;
            Driver = driver;
        }

        public void Enable()
        {
            IsActive = true;
        }
        public void Disable() 
        { 
            IsActive = false; 
        }
    }
}
