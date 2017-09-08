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
        public IEnumerable<Product> Products { get; set; }

    }
    [DataServiceKey("SupplierID")]
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Country { get; set; }
        public IEnumerable<Product> Products { get; set; }
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
        public Supplier Supplier { get; set; }
        public Category Category { get; set; }

    }

    public class MyDataSource
    {
        static string FOLDER =  @".\data\"; // HttpContext.Current.Server.MapPath("/data"); //@"C:\usertmp\"; //
        static IEnumerable<Category> _Categories;
        static IEnumerable<Product> _Products;
        static IEnumerable<Supplier> _Suppliers;

        static MyDataSource()
        {
            Console.WriteLine($"... loading {FOLDER}\\XCategories.xml");
            _Categories =
                XElement.Load(FOLDER + @"\XCategories.xml")
                .Elements("Category")
                .Select(x => new Category
                {
                    CategoryID = (int)x.Element("CategoryID"),
                    CategoryName = (string)x.Element("CategoryName"),
                    Description = (string)x.Element("Description"),

                }).ToArray();

            Console.WriteLine($"... loading {FOLDER}\\XSuppliers.xml");
            _Suppliers =
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
            _Products =
               XElement.Load(FOLDER + @"\XProducts.xml")
               .Elements("Product")
               .Select(x => new Product
               {
                   ProductID = (int)x.Element("ProductID"),
                   ProductName = (string)x.Element("ProductName"),
                   SupplierID = string.IsNullOrEmpty((string)x.Element("SupplierID")) ? (int?)null : (int?)x.Element("SupplierID"),
                   CategoryID = string.IsNullOrEmpty((string)x.Element("CategoryID")) ? (int?)null : (int?)x.Element("CategoryID"),
                   UnitPrice = string.IsNullOrEmpty((string)x.Element("UnitPrice")) ? (decimal?)null : (decimal)x.Element("UnitPrice"),
                   UnitsInStock = string.IsNullOrEmpty((string)x.Element("UnitsInStock")) ? (Int16?)null : (Int16)x.Element("UnitsInStock"),
                   UnitsOnOrder = string.IsNullOrEmpty((string)x.Element("UnitsOnOrder")) ? (Int16?)null : (Int16)x.Element("UnitsOnOrder"),

               }).ToArray();

            Console.WriteLine($"... relating _Categories, _Products and _Suppliers");


            
            var _product_supplier_lookup = _Products.Where(p => p.SupplierID.HasValue).ToLookup(o => o.SupplierID);
            var _suppliers_dict = _Suppliers.ToDictionary(c => c.SupplierID);

            foreach (var o in _Products) if (o.SupplierID.HasValue) o.Supplier = _suppliers_dict[(int)o.SupplierID];

            foreach (var c in _Suppliers)  c.Products = _product_supplier_lookup[c.SupplierID];

            var _product_lookup = _Products.Where(p => p.CategoryID.HasValue).ToLookup(o => o.CategoryID);
            var _categories_dict = _Categories.ToDictionary(c => c.CategoryID);

            foreach (var o in _Products) if (o.CategoryID.HasValue) o.Category = _categories_dict[(int)o.CategoryID]; //////

            foreach (var c in _Categories) c.Products = _product_lookup[c.CategoryID];

            Console.WriteLine($"... starting");

        }
        

        public IQueryable<Category> Categories
        {
            get
            {
                Console.WriteLine($"... returning MyCategories");
                return _Categories.AsQueryable();
            }
        }
        public IQueryable<Product> Products
        {
            get
            {
                Console.WriteLine($"... returning MyProducts");
                return _Products.AsQueryable();
            }
        }
        public IQueryable<Supplier> Suppliers
        {
            get
            {
                Console.WriteLine($"... returning MySuppliers");
                return _Suppliers.AsQueryable();
            }
        }
        
    }


}
