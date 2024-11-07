using System.Text.Json.Serialization;

namespace HomeBudget.Api.Entities
{
    public class Address
    {
        public string Id { get; private set; }
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string ApartmentNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        #region UserRelation
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        [JsonIgnore]
        public User User { get; set; } = null!;
        #endregion

        public Address()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Address(User user) : this()
        {
            UserId = user.Id;
            User = user;
            User.Address = this;
        }

        public bool IsAddressValid()
        {
            if (string.IsNullOrWhiteSpace(Street))
                return false;

            if (string.IsNullOrWhiteSpace(HouseNumber))
                return false;

            if (string.IsNullOrWhiteSpace(City))
                return false;

            if (string.IsNullOrWhiteSpace(PostalCode))
                return false;

            if (string.IsNullOrWhiteSpace(Country))
                return false;

            if (string.IsNullOrWhiteSpace(Region))
                return false;

            return true;
        }
    }
}