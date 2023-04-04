using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Samples
{
    public class ReceivingPageGetRequestModel : PageModel
    {
        [BindProperty]
        public string? shipmentid { get; set; }

        /*
         * the Get Request executes the OnGet
         * the parameter list in the OnGet is looking for a label in the Request
         *    the match the parameter name
         */ 
        public void OnGet(string? sid)
        {
           shipmentid = sid;
        }
    }
}
