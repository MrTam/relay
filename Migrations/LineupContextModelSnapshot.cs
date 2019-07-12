﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Relay.Models;

namespace Relay.Migrations
{
    [DbContext(typeof(LineupContext))]
    partial class LineupContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Relay.Models.LineupEntry", b =>
                {
                    b.Property<int>("LineupEntryId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Favorite");

                    b.Property<int>("HD");

                    b.Property<string>("Name");

                    b.Property<uint>("Number");

                    b.Property<int>("Provider");

                    b.Property<string>("Url");

                    b.HasKey("LineupEntryId");

                    b.ToTable("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
