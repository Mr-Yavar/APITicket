using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configurtion
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Id= "89968617-5474-4aba-bde5-29d4a1611374", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id= "89968617-5474-4aba-bde5-29d4a1611375",Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }
}
