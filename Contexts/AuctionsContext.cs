using Microsoft.EntityFrameworkCore;
using Auctions.Models;

namespace Auctions.Contexts {
    public class AuctionsContext : DbContext {
        public AuctionsContext (DbContextOptions<AuctionsContext> options) : base(options) {}

        public DbSet<Auction> Auctions {get; set;}
        public DbSet<Bid> Bids {get; set;}
        public DbSet<User> Users {get; set;}
    }
}