using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using netbelt.ViewModels;
using netbelt.Contexts;
using netbelt.Models;

namespace netbelt.ViewComponents {

    public class ListAuctionsViewComponent : ViewComponent {

        private readonly NetBeltContext _context;

        public ListAuctionsViewComponent(NetBeltContext context) {
            _context = context;
        }

        // using async call here because I had issues with rendering synchronous ViewComponent methods in Views
        public async Task<IViewComponentResult> InvokeAsync() {
            Task<List<Auction>> auctions = _context.Auctions.Include(a => a.User).Include(a => a.Bids).Where(a => a.EndDate > DateTime.UtcNow).OrderBy(a => a.EndDate).ToListAsync();
            ViewBag.Now = DateTime.UtcNow;
            return View(await auctions); // whynot.jpg
        }
    }
}