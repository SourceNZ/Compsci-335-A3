using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Xml.Linq;

namespace WebApplication1
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class WcfDataService1 : DataService<MyDataSource>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.UseVerboseErrors = true;
        }
    }

    [DataServiceKey("CategoryID")]
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public IEnumerable<Product> MyProducts { get; set; }

    }
    [DataServiceKey("SupplierID")]
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Country { get; set; }
        public IEnumerable<Product> MyProducts { get; set; }
    }
    [DataServiceKey("ProductID")]
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public decimal? UnitPrice { get; set; }
        public Int16? UnitsInStock { get; set; }
        public Int16? UnitsOnOrder { get; set; }
        public Supplier MySupplier { get; set; }
        public Category MyCategory { get; set; }

    }
    //CategoryID = null or SupplierID = null or UnitPrice = null or UnitsInStock = null or UnitsOnOrder = null
    // <ProductID>1</ProductID>
    // <ProductName>Chai!</ProductName>
    // <SupplierID>1</SupplierID>
    //<CategoryID>1</CategoryID>
    // <UnitPrice>18.0000</UnitPrice>
    // <UnitsInStock>39</UnitsInStock>
    // <UnitsOnOrder>0</UnitsOnOrder>

    public class MyDataSource
    {
        static string FOLDER =  @".\data\"; // HttpContext.Current.Server.MapPath("/data"); //@"C:\usertmp\"; //
        static IEnumerable<Category> _MyCategories;
        static IEnumerable<Product> _MyProducts;
        static IEnumerable<Supplier> _MySuppliers;

        static MyDataSource()
        {
            Console.WriteLine($"... loading {FOLDER}\\XCategories.xml");
            _MyCategories =
                XElement.Load(FOLDER + @"\XCategories.xml")
                .Elements("Category")
                .Select(x => new Category
                {
                    CategoryID = (int)x.Element("CategoryID"),
                    CategoryName = (string)x.Element("CategoryName"),
                    Description = (string)x.Element("Description"),

                }).ToArray();

            Console.WriteLine($"... loading {FOLDER}\\XSuppliers.xml");
            _MySuppliers =
               XElement.Load(FOLDER + @"\XSuppliers.xml")
               .Elements("Supplier")
               .Select(x => new Supplier
               {
                   SupplierID = (int)x.Element("SupplierID"),
                   CompanyName = (string)x.Element("CompanyName"),
                   ContactName = (string)x.Element("ContactName"),
                   Country = (string)x.Element("Country"),

               }).ToArray();

            Console.WriteLine($"... loading {FOLDER}\\XProducts.xml");
            _MyProducts =
               XElement.Load(FOLDER + @"\XProducts.xml")
               .Elements("Product")//this should be Product ( if i get server error 500 then change this
               .Select(x => new Product
               {
                   ProductID = (int)x.Element("ProductID"),
                   ProductName = (string)x.Element("ProductName"),
                   SupplierID = string.IsNullOrEmpty((string)x.Element("SupplierID")) ? null : (int?)x.Element("SupplierID"),
                   CategoryID = string.IsNullOrEmpty((string)x.Element("CategoryID")) ? null : (int?)x.Element("CategoryID"),
                   UnitPrice = string.IsNullOrEmpty((string)x.Element("UnitPrice")) ? null : (decimal?)x.Element("UnitPrice"),
                   UnitsInStock = string.IsNullOrEmpty((string)x.Element("UnitsInStock")) ? null : (Int16?)x.Element("UnitsInStock"),
                   UnitsOnOrder = string.IsNullOrEmpty((string)x.Element("UnitsOnOrder")) ? null : (Int16?)x.Element("UnitsOnOrder"),

               }).ToArray();

            Console.WriteLine($"... relating _Categories, _Products and _Suppliers");

            //need to link products: supplier ID to suppliers supplierID
            // public Supplier MySupplier { get; set; }
            // public Category MyCategory { get; set; }
            // Supplier:
            // public IEnumerable<Product> MyProducts { get; set; }

            var _product_supplier_lookup = _MyProducts.ToLookup(o => o.SupplierID);
            var _suppliers_dict = _MySuppliers.ToDictionary(c => (int?)c.SupplierID);

            foreach (var o in _MyProducts) o.MySupplier = _suppliers_dict[o.SupplierID];
            foreach (var c in _MySuppliers) c.MyProducts = _product_supplier_lookup[(int?)c.SupplierID];

            //Linking products categoryID to categories CategoryID
            var _product_lookup = _MyProducts.ToLookup(o => o.CategoryID);
            var _categories_dict = _MyCategories.ToDictionary(c => (int?)c.CategoryID);

            foreach (var o in _MyProducts) o.MyCategory = _categories_dict[o.CategoryID];
            foreach (var c in _MyCategories) c.MyProducts = _product_lookup[(int?)c.CategoryID];



            Console.WriteLine($"... starting");

        }
        

        public IQueryable<Category> Categories
        {
            get
            {
                Console.WriteLine($"... returning MyCategories");
                return _MyCategories.AsQueryable();
            }
        }
        public IQueryable<Product> Products
        {
            get
            {
                Console.WriteLine($"... returning MyProducts");
                return _MyProducts.AsQueryable();
            }
        }
        public IQueryable<Supplier> Suppliers
        {
            get
            {
                Console.WriteLine($"... returning MySuppliers");
                return _MySuppliers.AsQueryable();
            }
        }
        
    }


}
