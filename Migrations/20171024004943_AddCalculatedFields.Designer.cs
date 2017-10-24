using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using netbelt.Contexts;

namespace netbelt.Migrations
{
    [DbContext(typeof(NetBeltContext))]
    [Migration("20171024004943_AddCalculatedFields")]
    partial class AddCalculatedFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.3");

            modelBuilder.Entity("netbelt.Models.Auction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("ProductName");

                    b.Property<bool>("Resolved");

                    b.Property<double>("StartingBid");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("netbelt.Models.Bid", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int>("AuctionID");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("Highest");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("AuctionID");

                    b.HasIndex("UserID");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("netbelt.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("FirstName");

                    b.Property<double>("HeldAmount")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("Username");

                    b.Property<double>("WalletBalance");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("netbelt.Models.Auction", b =>
                {
                    b.HasOne("netbelt.Models.User", "User")
                        .WithMany("Auctions")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("netbelt.Models.Bid", b =>
                {
                    b.HasOne("netbelt.Models.Auction", "Auction")
                        .WithMany("Bids")
                        .HasForeignKey("AuctionID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("netbelt.Models.User", "User")
                        .WithMany("Bids")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
