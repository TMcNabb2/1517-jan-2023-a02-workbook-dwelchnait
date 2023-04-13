using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using WestWindSystem.BLL;
using WestWindSystem.Entities;
#endregion

namespace WebApp.Pages.Samples
{
    public class CRUDProductModel : PageModel
    {
        #region Private service fields & class constructor
        private readonly CategoryServices _categoryServices;
        private readonly ProductServices _productServices;
        private readonly SupplierServices _supplierServices;


        public CRUDProductModel(CategoryServices categoryservices,
                                        ProductServices productservices,
                                        SupplierServices supplierServices)
        {
            _categoryServices = categoryservices;
            _productServices = productservices;
            _supplierServices=supplierServices;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public int? productid { get; set; }

        public string Feedback { get; set; }
        public bool HasFeedback { get { return !string.IsNullOrWhiteSpace(Feedback); } }

        [BindProperty]
        public Product productInfo { get; set; }

        public List<Category> categoryList { get; set; }
        public List<Supplier> supplierList { get; set; }    

        public void OnGet()
        {

            if (productid.HasValue) //field is a nullable int
                if (productid > 0)  //primary keys start at 1 and increase
                {
                    //.Value is needed BECAUSE productid, though an int, is a nullable int
                    productInfo = _productServices.Product_GetById(productid.Value);
                    Feedback = $"ID {productInfo.ProductID} Name {productInfo.ProductName} Price {productInfo.UnitPrice}";
                }
            PopulateSupportLists();
        }

        public void PopulateSupportLists()
        {
            categoryList = _categoryServices.Category_List();
            supplierList = _supplierServices.Supplier_List();
        }

        public IActionResult OnPost()
        {
            PopulateSupportLists();
            return Page();
        }
        public IActionResult OnPostNew()
        {
            //setup a test to see if the state of the web page is valid
            //  BEFORE attempting any processing
            //The form fields have be associated with individual properties
            //   in the bound variable productInfo via the BindProperty attribute
            //As the Request Post was sent to the web server, the transfer of
            //  control form data was attempted to be placed in the receiving 
            //  variables.
            //this means any fully-implement properties that throw an exception
            //  will set the ModelState to invalid
            if (ModelState.IsValid)
            {
                //one will be interacting with multiple projects within this post event
                //the class library project CANNOT display errors directly to your screen
                //the class library project INSTEAD issues Exceptions
                //THEREFORE your post event handler MUST use user-friendly error handling
                //  techniques
                try
                {

                }
                catch(ArgumentNullException ex)
                {

                }
                catch(ArgumentException ex)
                {

                }
                catch(Exception ex)
                {

                }
            }
            else
            {
                //remember the internet and thus your web application is a stateless
                //   system
                //you must refresh your lists
                PopulateSupportLists();
            }
            return Page();
        }
    }
}
