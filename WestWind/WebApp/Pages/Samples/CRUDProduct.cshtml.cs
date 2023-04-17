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
                    //Feedback = $"ID {productInfo.ProductID} Name {productInfo.ProductName} Price {productInfo.UnitPrice}";
                }
            PopulateSupportLists();
        }

        public void PopulateSupportLists()
        {
            categoryList = _categoryServices.Category_List();
            supplierList = _supplierServices.Supplier_List();
        }

        public IActionResult OnPostClear()
        {
            ModelState.Clear();
            Feedback = "";
            productid = (int?)null;
            //redirect the page to itself and cause the OnGet to execute
            //the routing value needs to be also cleared
            return RedirectToPage(new { productid = productid});
        }
        public IActionResult OnPostSearch()
        {
            
            return RedirectToPage("./CategoryProducts");
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
            
            //do some on web page validation prior to sending request to BLL service method
            //this is optional
            //testing here should be for primitive validation
            // a) presence
            // b) datatype
            // c) range
            //in this example we will do a presence check that the category and supplier have
            //  been picked
            if (productInfo.CategoryID == 0)
            {
                ModelState.AddModelError(nameof(productInfo.CategoryID), "Select a category for the product");

            }
            if (productInfo.SupplierID == 0)
            {
                ModelState.AddModelError(nameof(productInfo.SupplierID), "Select a supplier for the product");

            }

            //At this point, if you had errors generate while the form data is placed into your
            //   entity instance (productInfo), thrown by fully implement properties
            // OR
            //   locally generate errors in your primitive validation
            //  the ModelState will be invalid

            if (ModelState.IsValid)
            {
                //one will be interacting with multiple projects within this post event
                //the class library project CANNOT display errors directly to your screen
                //the class library project INSTEAD issues Exceptions
                //THEREFORE your post event handler MUST use user-friendly error handling
                //  techniques
                try
                {
                    //the data is ALREADY in my instance of the Product
                    //call the appropriate service method
                    //capture any return data from the service method and do appropriate processing
                    int newproductid = _productServices.Product_AddProduct(productInfo);

                    Feedback = $"Product (id: {newproductid}) has been added to the system";
                   // PopulateSupportLists();
                }
                catch (ArgumentNullException ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName),GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
                catch(ArgumentException ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName), GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName), GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
            }
            //else
            //{
                //remember the internet and thus your web application is a stateless
                //   system
                //you must refresh your lists
                PopulateSupportLists();
            //}
            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (productInfo.CategoryID == 0)
            {
                ModelState.AddModelError(nameof(productInfo.CategoryID), "Select a category for the product");

            }
            if (productInfo.SupplierID == 0)
            {
                ModelState.AddModelError(nameof(productInfo.SupplierID), "Select a supplier for the product");

            }
            if (ModelState.IsValid)
            {
                try
                {
                    //during the update the result of your service call will be 1 of 3

                    // a) successfull  product was updated
                    // b) fails        an error was thrown by the class library during processing
                    //
                    // c) What is the other possibility?
                    // what if the update on the sql did not find the product record
                    //
                    // in sql your code will probably have a where class on the update statement
                    // the where clause will attempt to locate the record by the pkey value
                    // if the record is not found, sql DOES NOT Abort and DOES NOT update any thing

                    // One nice thing is that sql will return the number of records alterd
                    // That value can be tested in this code
                    // An appropriate Feedback message can be on against this value
                    int rowsaffected = _productServices.Product_UpdateProduct(productInfo);
                    if (rowsaffected > 0)
                    {
                        Feedback = $"Product (id: {productInfo.ProductID}) has been updated";
                    }
                    else
                    {
                        Feedback = $"Product (id: {productInfo.ProductID}) has been remove from the system. Return to the search page.";
                    }


                    // PopulateSupportLists();
                }
                catch (ArgumentNullException ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName), GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName), GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName), GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
            }

            PopulateSupportLists();
          
            return Page();
        }

        public IActionResult OnPostDelete()
        {
            //since our delete is a LOGICAL delete, these foreign key values are still important
            if (productInfo.CategoryID == 0)
            {
                ModelState.AddModelError(nameof(productInfo.CategoryID), "Select a category for the product");

            }
            if (productInfo.SupplierID == 0)
            {
                ModelState.AddModelError(nameof(productInfo.SupplierID), "Select a supplier for the product");

            }
            if (ModelState.IsValid)
            {
                try
                {
                    //during the update the result of your service call will be 1 of 3

                    // a) successfull  product was updated
                    // b) fails        an error was thrown by the class library during processing
                    //
                    // c) What is the other possibility?
                    // what if the update on the sql did not find the product record
                    //
                    // in sql your code will probably have a where class on the update statement
                    // the where clause will attempt to locate the record by the pkey value
                    // if the record is not found, sql DOES NOT Abort and DOES NOT update any thing

                    // One nice thing is that sql will return the number of records alterd
                    // That value can be tested in this code
                    // An appropriate Feedback message can be on against this value
                    int rowsaffected = _productServices.Product_DeleteProduct(productInfo);
                    if (rowsaffected > 0)
                    {
                        Feedback = $"Product (id: {productInfo.ProductID}) has been discontinued";
                    }
                    else
                    {
                        Feedback = $"Product (id: {productInfo.ProductID}) has been remove from the system. Return to the search page.";
                    }


                    // PopulateSupportLists();
                }
                catch (ArgumentNullException ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName), GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName), GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(productInfo.ProductName), GetInnerException(ex).Message);
                    //PopulateSupportLists();
                    //return Page();
                }
            }

            PopulateSupportLists();

            return Page();
        }


        //this method is used to drill down into error messages
        //at time you may receive an errorr that tells you the details are
        //  located in the InnerException
        //This method will drill down to the lowest InnerException and return the ex object
        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
    }
}
