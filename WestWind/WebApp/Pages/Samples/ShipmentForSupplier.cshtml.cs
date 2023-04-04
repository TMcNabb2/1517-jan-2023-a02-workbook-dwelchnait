using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WestWindSystem.BLL;
using WestWindSystem.Entities;

namespace WebApp.Pages.Samples
{
    public class ShipmentForSupplierModel : PageModel
    {
        #region Constructor Injection Depedency setup
        private readonly ShipperServices _shipperServices;
        private readonly ShipmentServices _shipmentServices;

        public ShipmentForSupplierModel(ShipperServices shipperServices,
                            ShipmentServices shipmentServices)
        {
            _shipperServices = shipperServices;
            _shipmentServices = shipmentServices;
        }
        #endregion

        //control / variable setup

        [BindProperty]
        public string? searcharg { get; set; } //incoming

        [BindProperty]
        public int shipperid { get; set; } //incoming

        public List<Shipment> ShipmentInfo { get; set; } //outgoing

        //by placing the =new() on the declaration, I ensure that an
        //   instance of the declaration exists. In this case a List<T>
        //HOWEVER: the list will be empty until something is placed within it
        public List<Shipper> ShipperList { get; set; } = new();

        public void OnGet()
        {
        }
        public IActionResult OnPostFetchShippers() 
        {
           //validation: was a search argument actually entered
           if(string.IsNullOrWhiteSpace(searcharg))
            {
                ModelState.AddModelError(nameof(searcharg), "Shipper search name is empty");
            }
           if(ModelState.IsValid)
            {
                ShipperList = _shipperServices.Shipper_GetByName(searcharg);
            }
            return Page();
        }
        public IActionResult OnPostFetchShipments()
        {
           //validation: was a shipper selected
           if (shipperid == 0)
            {
                ModelState.AddModelError(nameof(shipperid), "Shipper was not selected");

            }
            if (ModelState.IsValid)
            {
               ShipmentInfo = _shipmentServices.Shipment_GetByShipper(shipperid);
            }
            //refresh the shipper list if there was a shipper search arg
            if (!string.IsNullOrWhiteSpace(searcharg))
            {
                ShipperList = _shipperServices.Shipper_GetByName(searcharg);
            }
            return Page();
        }
        public IActionResult OnPostClear()
        {
            searcharg = "";
            shipperid = 0;
            ModelState.Clear();
            return RedirectToPage();
        }
    }
}
