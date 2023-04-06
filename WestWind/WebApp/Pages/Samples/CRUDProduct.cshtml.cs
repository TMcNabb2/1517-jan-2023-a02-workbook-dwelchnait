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

        public Product productInfo { get; set; }
        public void OnGet()
        {

            if (productid.HasValue) //field is a nullale int
                if (productid > 0)  //primary keys start at 1 and increase
                {
                    //.Value is needed BECAUSE productid, though an int, is a nullable int
                    productInfo = _productServices.Product_GetById(productid.Value);
                    Feedback = $"ID {productInfo.ProductID} Name {productInfo.ProductName} Price {productInfo.UnitPrice}";
                }
        }
    }
}
