using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using WestWindSystem.DAL;
using WestWindSystem.Entities;
#endregion


namespace WestWindSystem.BLL
{
    public class ShipmentServices
    {
        #region setup the context connection variable and class constructor
        //variable to hold an instance of context class
        private readonly WestWindContext _context;

        //constructor to create an instance of the registered context class
        internal ShipmentServices(WestWindContext regcontext)
        {
            _context = regcontext;
        }

        #endregion

        #region Service: Queries

        public List<Shipment> Shipment_GetByShipper(int shipperid)
        {
            return _context.Shipments
                .Where(x => x.ShipVia == shipperid)
                .OrderBy(x => x.ShippedDate)
                .ToList();
        }

        #endregion

    }
}
