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
        public Int32 CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Picture { get; set; }
        public IEnumerable<Product> MyProducts { get; set; }

    }
    [DataServiceKey("SupplierID")]
    public class Supplier
    {
        public Int32 SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Homepage { get; set; }
        public IEnumerable<Product> MyProducts { get; set; }
    }
    [DataServiceKey("ProductID")]
    public class Product
    {
        public Int32 ProductID { get; set; }
        public string ProductName { get; set; }
        public Int32? SupplierID { get; set; }
        public Int32 CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public int? UnitPrice { get; set; }
        public Int16? UnitsInStock { get; set; }
        public Int16? UnitsOnOrder { get; set; }
        public Int16? ReOrderLevel { get; set; }
        public bool Discontinued { get; set; }
        public Supplier MySuppliers { get; set; }
        public Category MyCategories { get; set; }
    }
    public class MyDataSource
    {
        static string FOLDER =  @".\data\"; // HttpContext.Current.Server.MapPath("/data"); //@"C:\usertmp\"; //

        static MyDataSource()
        {
            Console.WriteLine($"... loading {FOLDER}\\XCategories.xml");
            _MyCategories =
                XElement.Load(FOLDER + @"\XCategories.xml")
                .Elements("Categories")
                .Select(x => new Category
                {
                    CategoryID = (Int32)x.Element("CategoryID"),
                    CategoryName = (string)x.Element("CategoryName"),
                    Description = (string)x.Element("Description"),
                    Picture = (int)x.Element("Picture"),

                }).ToArray();

            Console.WriteLine($"... loading {FOLDER}\\XSuppliers.xml");
            _MySuppliers =
               XElement.Load(FOLDER + @"\XSuppliers.xml")
               .Elements("MySuppliers")
               .Select(x => new Supplier
               {
                   SupplierID = (Int32)x.Element("SupplierID"),
                   CompanyName = (string)x.Element("CompanyName"),
                   ContactName = (string)x.Element("ContactName"),
                   ContactTitle = (string)x.Element("ContactTitle"),
                   Address = (string)x.Element("Address"),
                   City = (string)x.Element("City"),
                   Region = (string)x.Element("Region"),
                   PostalCode = (string)x.Element("PostalCode"),
                   Country = (string)x.Element("Country"),
                   Phone = (string)x.Element("Phone"),
                   Fax = (string)x.Element("Fax"),
                   Homepage = (string)x.Element("Homepage"),
                  
               }).ToArray();

            Console.WriteLine($"... loading {FOLDER}\\XProducts.xml");
            _MyProducts =
               XElement.Load(FOLDER + @"\XProducts.xml")
               .Elements("MyProducts")
               .Select(x => new Product
               {
                   ProductID = (Int32)x.Element("ProductID"),
                   ProductName = (string)x.Element("ProductName"),
                   SupplierID = (Int32?)x.Element("SupplierID"),
                   CategoryID = (Int32)x.Element("CategoryID"),
                   QuantityPerUnit = (string)x.Element("QuantityPerUnit"),
                   UnitPrice = (int?)x.Element("UnitPrice"),
                   UnitsInStock = (Int16?)x.Element("UnitsInStock"),
                   UnitsOnOrder = (Int16?)x.Element("UnitsOnOrder"),
                   ReOrderLevel = (Int16?)x.Element("ReOrderLevel"),
                   Discontinued = (bool)x.Element("Discontinued"),
               }).ToArray();

            Console.WriteLine($"... relating _Categories, _Products and _Suppliers");

            var _product_lookup = _MyProducts.ToLookup(o => o.CategoryID);
            var _categories_dict = _MyCategories.ToDictionary(c => c.CategoryID);

            foreach (var o in _MyProducts) o.MyCategories = _categories_dict[o.CategoryID];
            foreach (var c in _MyCategories) c.MyProducts = _product_lookup[c.CategoryID];

         
            Console.WriteLine($"... starting");
          
        }
        static IEnumerable<Category> _MyCategories;
        static IEnumerable<Product> _MyProducts;
        static IEnumerable<Supplier> _MySuppliers;

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
