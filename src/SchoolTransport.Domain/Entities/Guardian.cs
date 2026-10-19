namespace SchoolTransport.Domain.Entities
{
    public class Guardian : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        protected Guardian() { }
        public Guardian (string name, string phone, Guid userId)
        {
            Name = name;
            Phone = phone;
            UserId = userId;
        }
    }
}
