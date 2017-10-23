using Microsoft.EntityFrameworkCore;
using netbelt.Models;

namespace netbelt.Contexts {
    public class NetBeltContext : DbContext {
        public NetBeltContext (DbContextOptions<NetBeltContext> options) : base(options) {}

        public DbSet<Auction> Auctions {get; set;}
        public DbSet<Bid> Bids {get; set;}
        public DbSet<User> Users {get; set;}
    }
}