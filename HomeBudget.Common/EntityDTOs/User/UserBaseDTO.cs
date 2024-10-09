namespace HomeBudget.Common.EntityDTOs.User
{
    public class UserBaseDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsAccountConfirmed { get; set; } = false;
        public bool IsAccountSetup { get; set; } = false;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}