namespace SprintCrowdBackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Entity configuration for FirebaseMessagingToken table
    /// </summary>
    public class FirebaseMessagingTokenConfig : IEntityTypeConfiguration<FirebaseMessagingToken>
    {
        /// <summary>
        /// Configure table FirebaseMessagingToken
        /// </summary>
        /// <param name="builder">entity builder instance</param>
        public void Configure(EntityTypeBuilder<FirebaseMessagingToken> builder)
        {
            builder.HasOne(token => token.User);
            builder.Property<DateTime>("LastUpdated");
        }
    }
}