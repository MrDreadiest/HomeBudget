using HomeBudget.Common.EntityDTOs.User;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace HomeBudget.Api.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsAccountConfirmed { get; set; } = false;
        public bool IsAccountSetup { get; set; } = false;

        #region AddressRelation
        public Address? Address { get; set; } = null!;
        #endregion

        #region Budget
        public List<Budget> Budgets { get; private set; } = new List<Budget>();
        #endregion

        public User() : base() 
        {
            Address = new Address(this);
        }
    }
}