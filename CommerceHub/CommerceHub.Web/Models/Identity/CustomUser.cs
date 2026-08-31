using Microsoft.AspNetCore.Identity;

namespace CommerceHub.Web.Models.Identity
{
    public class CustomUser : IdentityUser
    {
        public string? FullName { get; set; }
        public int? CustomerId { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }

    }


}
