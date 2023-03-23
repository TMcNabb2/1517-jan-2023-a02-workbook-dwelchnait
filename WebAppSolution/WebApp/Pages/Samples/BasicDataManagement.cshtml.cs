using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Samples
{
    public class BasicDataManagementModel : PageModel
    {
        //properties to use for local data processing
        //a property can be tied DIRECTLY to a specific control
        //    on the web page
        //use the annotation [BindProperty] to directly tie
        //    a property in the PageModel (code-behind) to
        //    a control on your web page
        //Data Transfer
        //  a) a property/field in your code-behind that does
        //      NOT use the annotation [BindProperty] is a ONE
        //      way data transfer: out (display)
        //  b) a property in your code-behind that does
        //      use the annotation [BindProperty] is a TWO-Way
        //      data transfer: out (display) and in (a user input)

        [BindProperty]
        public int Num { get; set; }

        [BindProperty]
        public string MassText { get; set; }

        //this is for the select control that has a value attribute
        //      on the <option> tag
        //the datatype of the property MUST MATCh the value attribute 
        //      on the <option> tag
        [BindProperty]
        public int FavouriteCourse { get; set; }

        //this is for the select control that has NO value attribute
        //      on the <option> tag
        //the datatype of the property MUST BE A STRING
        [BindProperty]
        public string FavouriteCourseNoValueOnOption { get; set; }

        public string FeedBack { get; set; }
        public void OnGet()
        {
        }
        public void OnPostControlProcessing()
        {
            //do your form validation and record your error message under ModelState
            if (Num == 0)
            {
                ModelState.AddModelError(nameof(Num),"Value for number is zero");
            }
            if (MassText == null || MassText.Length == 0)
            {
                ModelState.AddModelError(nameof(MassText), "You did not enter a comment");
            }
            if (FavouriteCourse == 0)
            {
                ModelState.AddModelError(nameof(FavouriteCourse), "You did not indicate your favourite course. Please choose.");
            }
            //is the ModelState in good order
            if (ModelState.IsValid)
            {
                FeedBack = $"You entered the value {Num}" +
                    $" your comment is {MassText}" +
                    $" your favourite course number is: {FavouriteCourse}" +
                    $" for {FavouriteCourseNoValueOnOption}";
            }
        }
    }
}
