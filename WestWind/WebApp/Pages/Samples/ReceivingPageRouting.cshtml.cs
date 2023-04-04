using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Samples
{
    public class ReceivingPageRoutingModel : PageModel
    {
        /*
         * the BindProperty for the property receiving the routing parameter
         *    needs to use the SupportsGet=true attribute
         * 
         * since a call to this page CAN be done without a routing parameter
         *    sent to it, the receiving local variable needs to be nullable
         *    
         */ 
        [BindProperty(SupportsGet =true)]
        public int? shipmentid { get; set; }
        public void OnGet()
        {
        }
    }
}
