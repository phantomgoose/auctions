using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using netbelt.ViewModels;

namespace netbelt.ViewComponents {
    public class CreateBidViewComponent : ViewComponent {
        // using async call here because I had issues with rendering synchronous ViewComponent methods in Views
        public async Task<IViewComponentResult> InvokeAsync(int AuctionID) {
            ViewBag.AuctionID = AuctionID;
            return View(new BidViewModel());
        }
    }
}