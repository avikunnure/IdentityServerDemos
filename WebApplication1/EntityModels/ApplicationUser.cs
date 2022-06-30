using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.EntityModels
{
    public class ApplicationUser : IdentityUser
    {
        [Column("FirstName")]
        public string FirstName { get; set; }= string.Empty;

        [Column("LastName")]
        public string LastName { get; set; }=string.Empty;

    }
}