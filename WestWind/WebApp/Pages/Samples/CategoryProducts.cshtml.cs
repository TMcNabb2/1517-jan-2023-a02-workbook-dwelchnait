using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WestWindSystem.Entities;
using WestWindSystem.BLL;

namespace WebApp.Pages.Samples
{
    public class CategoryProductsModel : PageModel
    {
        #region Private service fields & class constructor
        private readonly CategoryServices _categoryServices;
        private readonly ProductServices _productServices;
        private readonly SupplierServices _supplierServices;


        public CategoryProductsModel(CategoryServices categoryservices,
                                        ProductServices productservices,
                                        SupplierServices supplierServices)
        {
            _categoryServices = categoryservices;
            _productServices = productservices;
            _supplierServices=supplierServices;
        }
        #endregion
      
        [BindProperty]
        public int categoryid { get; set; }

        public List<Category> CategoryList { get; set; } 

        public List<Supplier> SupplierList { get; set; } 
        public List<Product> ProductList { get; set; } 

       
        public string Feedback { get; set; }

        public void OnGet()
        {
            //  your code here
            PopulateLists();
        }

        public void PopulateLists()
        {
            // your code here
            //obtain data from the data base via a service in the class library
            //receiving variable: CategoryList
            //the service instance: _xxxxxServices
            //the service query: .xxxxxxx()
            CategoryList = _categoryServices.Category_List();
            SupplierList = _supplierServices.Supplier_List();
        }

        public IActionResult OnPostSearch()
        {
            // your code here
            if (categoryid == 0)
            {
                Feedback ="Select a category to view it''s products";
            }
            else
            {
                //call the appropriate service method to obtain a list of
                //  products for this category
                ProductList = _productServices.Product_GetByCategory(categoryid);
            }
            PopulateLists();
            return Page();
        }

        public IActionResult OnPostClear()
        {
            Feedback = "";
            categoryid = 0;
            ModelState.Clear();
            /*
             * RedirectToPage() cause your system to do a second class to the
             * web page as if it was a new page
             * Therefore the OnGet() method will be executed.
             */ 
            return RedirectToPage();
        }

        public IActionResult OnPostNew()
        {
         
            //temporarly put in a call to reload the lists
            //this will be removed when the proper code for this
            //  method is inserted
          
            return RedirectToPage("./CRUDProduct");
        }

    }
}

