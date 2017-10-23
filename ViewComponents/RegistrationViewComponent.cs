using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using netbelt.ViewModels;

namespace netbelt.ViewComponents {
    public class RegistrationViewComponent : ViewComponent {
        // using async call here because I had issues with rendering synchronous ViewComponent methods in Views
        public async Task<IViewComponentResult> InvokeAsync() {
            return View(new RegistrationViewModel());
        }
    }
}