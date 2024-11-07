namespace HomeBudget.App.Models
{
    public class Address
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public string Street { get; private set; } = string.Empty;
        public string HouseNumber { get; private set; } = string.Empty;
        public string ApartmentNumber { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Region { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;

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

            return true;
        }
    }
}