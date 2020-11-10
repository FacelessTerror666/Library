using Library.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Database.EntityConfigs
{
    public class LastParserIdConfig
    {
        public class OrderConfig : IEntityTypeConfiguration<LastParserId>
        {
            public void Configure(EntityTypeBuilder<LastParserId> builder)
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }

        }
    }
}
