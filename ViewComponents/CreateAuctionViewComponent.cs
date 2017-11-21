using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auctions.ViewModels;

namespace Auctions.ViewComponents {
    public class CreateAuctionViewComponent : ViewComponent {
        // using async call here because I had issues with rendering synchronous ViewComponent methods in Views
        public async Task<IViewComponentResult> InvokeAsync() {
            ViewBag.Now = DateTime.Now.ToString("yyyy-MM-dd");
            return View(new CreateAuctionViewModel());
        }
    }
}