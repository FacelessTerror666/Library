using Library.Database.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Database.Entities
{
    public class User : IdentityUser<long>, IEntity
    {
    }
}
