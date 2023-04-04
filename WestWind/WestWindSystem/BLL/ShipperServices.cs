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
    public class ShipperServices
    {
        #region setup the context connection variable and class constructor
        //variable to hold an instance of context class
        private readonly WestWindContext _context;

        //constructor to create an instance of the registered context class
        internal ShipperServices(WestWindContext regcontext)
        {
            _context = regcontext;
        }

        #endregion

        #region Service: Queries

        public List<Shipper> Shipper_GetByName(string partialname)
        {
            return _context.Shippers
                .Where(x => x.CompanyName.Contains(partialname))
                .OrderBy(x => x.CompanyName)
                .ToList();
        }

        #endregion

    }
}
