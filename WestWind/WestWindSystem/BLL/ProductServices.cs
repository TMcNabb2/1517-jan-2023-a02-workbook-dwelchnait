using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class ProductServices
    {
        #region setup the context connection variable and class constructor
        //variable to hold an instance of context class
        private readonly WestWindContext _context;

        //constructor to create an instance of the registered context class
        internal ProductServices(WestWindContext regcontext)
        {
            _context = regcontext;
        }

        #endregion

        #region Service: Queries (R in CRUD)


        public List<Product> Product_GetByCategory(int categoryid)
        {
            return _context.Products
                            .Where(x => x.CategoryID == categoryid)
                            .ToList();

        }

        public Product Product_GetById(int productid)
        {
            return _context.Products
                            .Where(x => x.ProductID == productid)  //filter
                            .FirstOrDefault(); //if found return the first instance else null

        }
        #endregion

        #region Add, Update, Delete (C,U and D in CRUD)
        //Adding a record to your database MAY require you to 
        //      verify that the data does not already exist on the database
        //      verify that the incoming data is valid (do not trust front end)
        //this is referred to as Business Logic (Business Rules)
        //One way this can be done is using a Linq query and a given set
        //      of verificaction fields
        //Example: for this product insertion we will verify that there
        //              is no product record on the database with the
        //              same product name and quantity per unit from the
        //              same supplier. If so, throw an Exception

        //other key points
        // you MUST know whether the table has an identity pkey or not
        // if the table pkey is NOT an identity then you MUST ensure
        //      that the incoming instance of the enity HAS pkey value
        // if the table pkey is an indentity then the database WILL generate
        //      the pkey value and make it accessable to you ARTER the data
        //      has been committed on the database.

        //for our Add, the pkey is an identity field
        //We will retrieve and return the identity field value after committing
        //THIS IS OPTIONAL

        public int Product_AddProduct(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Product data is missing");
            }

            //Business Rule
            //this is an OPTIONAL sample of business rule validation
            //  of incoming data
            Product exists = _context.Products
                                .Where(x => x.ProductName.ToUpper().Equals(item.ProductName.ToUpper())
                                         && x.QuantityPerUnit.ToUpper().Equals(item.QuantityPerUnit.ToUpper())
                                         && x.SupplierID == item.SupplierID)
                                .FirstOrDefault();

            if (exists != null)
            {
                throw new Exception($"{item.ProductName} with a size of {item.QuantityPerUnit}" +
                    $" from the selected supplier is already on file.");
            }

            //at this point, after all business rule validation is complete,
            //      one can assume the data is valid

            //two steps in adding your data: Staging and Commit
            //Staging: staging is the act of placing your data  into local memory
            //          for eventual processing on the database
            // a) what is the Dbset
            // b) the action
            // c) indicate the data involved

            //IMPORTANT: the data is in LOCAL MEMORY
            //           the data has NOT!!! yet been sent to the database
            //THERFORE: at this time, there is NO!!!!!!!! primary key value
            //           on the instance (except for the default of that datatype)

            _context.Products.Add(item);

            //Commit the LOCAL Staged data to the database

            //IF there are validation annotation on your Entity definition
            //      they will be EXECUTED during the saving of your staged actions
            //      to the database
            //SO, if you violate a validation annotation, the your data DOES NOT
            //  go to the database; the system throws an Exception to the violation
            //  using the validation annonation message
            //IF a validation (constraints) on the database fails, then the
            //  database will throw an error and the attempted commit will automatically
            //  be rolledback.
            _context.SaveChanges();

            //AFTER the successful commit to the database, you new product
            //  will be placed in the database GENERATING you new identity pkey value
            return item.ProductID;
        }

        public int Product_UpdateProduct(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Product data is missing");
            }

            bool exists = _context.Products.Any(x => x.ProductID == item.ProductID);

            //instance not found has a null value for the results
            if (!exists)
            {
                throw new Exception($"{item.ProductName} is no longer on the system.");
            }

            //Staging
            EntityEntry<Product> updating = _context.Entry(item);
            updating.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

           //Commit
           //the returned value is the number of rows affected 
           //value of rows affected is sent here from sql
            return _context.SaveChanges();

            
        }

        public int Product_DeleteProduct(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Product data is missing");
            }


            //the .Any method returns a true or false as a result of the condition
            bool exists = _context.Products
                               .Any(x => x.ProductID == item.ProductID);



            //instance not 
            if (!exists)
            {
                throw new Exception($"{item.ProductName} is no longer on the system.");
            }
            //there are two types of deleting: physical and logically
            //whether you have a physical or logically delete is determind WHEN
            //  the database was designed.

            //physical delete
            //you physical remove a record from the database
            //REMEMBER: you CANNOT delete a parent record IF there are child records
            //              (pkey => fkey)
            //          if there are child records, sql will throw and error

            //Physical Delete
            //will do a physical removal of a record from the database
            //Staging
            //EntityEntry<Product> deleting = _context.Entry(item);
            //deleting.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

            //Logical Delete
            //for our Product we cannot do a physical delete
            //you logically flag a record on the database by setting a specific field
            //we have a discontinued flag
            //DO NOT rely on the user to set the flag
            //DO IT FOR the user within your method
            //for our Product the service method will set the field indicating
            //  the logical delete to its appropriate value
            //Your service method should not rely on the appropriate value
            //  being submitted. Set the value within the service method

            //for a logical delete your are ACTUALLY doing an update
            //Staging
            item.Discontinued =true;
            EntityEntry<Product> updating = _context.Entry(item);
            updating.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            //Commit
            //the returned value is the number of rows affected 
            //value of rows affected is sent here from sql
            return _context.SaveChanges();


        }

        #endregion
    }
}
