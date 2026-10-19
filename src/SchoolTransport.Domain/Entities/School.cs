namespace SchoolTransport.Domain.Entities
{
    public class School : BaseEntity
    {
        public string Name { get; private set;  } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public Guid DriverId { get; private set; }
        public User Driver { get; private set; } = null!;

        protected School() { }

        public School(string name, string address, Guid driverId)
        {
            Name = name;
            Address = address;
            DriverId = driverId;
        }
    }
}
