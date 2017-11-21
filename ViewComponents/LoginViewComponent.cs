using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auctions.ViewModels;

namespace Auctions.ViewComponents {
    public class LoginViewComponent : ViewComponent {
        // using async call here because I had issues with rendering synchronous ViewComponent methods in Views
        public async Task<IViewComponentResult> InvokeAsync() {
            return View(new LoginViewModel());
        }
    }
}