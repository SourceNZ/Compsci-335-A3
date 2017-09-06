<Query Kind="Program" />

void Main() {
    var prefix = "odata_xml_";
    
    var urls = new [] {
    /*0*/    "http://localhost:8181/WcfDataService1.svc/?$format=json",
    /*1*/    "http://localhost:8181/WcfDataService1.svc/Categories()?$format=json&$orderby=CategoryID&$top=3&$select=CategoryID,CategoryName,Description",
    /*2*/    "http://localhost:8181/WcfDataService1.svc/Suppliers()?$format=json&$orderby=SupplierID&$top=3&$select=SupplierID,CompanyName,ContactName,Country",        
    /*3*/    "http://localhost:8181/WcfDataService1.svc/Products()?$format=json&$orderby=ProductID desc&$top=3&$select=ProductID,ProductName,CategoryID,SupplierID,UnitPrice,UnitsInStock,UnitsOnOrder",
    /*4*/    "http://localhost:8181/WcfDataService1.svc/Products()?$format=json&$orderby=ProductID&$filter=CategoryID eq null or SupplierID eq null or UnitPrice eq null or UnitsInStock eq null or UnitsOnOrder eq null&$select=ProductID,ProductName,CategoryID,SupplierID,UnitPrice,UnitsInStock,UnitsOnOrder",
    /*5*/    "http://localhost:8181/WcfDataService1.svc/Categories()?$format=json&$filter=CategoryID eq 9&$expand=Products&$select=CategoryID,CategoryName,Description,Products/ProductID,Products/ProductName,Products/UnitPrice,Products/UnitsInStock,Products/UnitsOnOrder",
    /*6*/    "http://localhost:8181/WcfDataService1.svc/Suppliers(24)?$format=json&$expand=Products&$select=SupplierID,CompanyName,ContactName,Country,Products/ProductID,Products/ProductName,Products/UnitPrice,Products/UnitsInStock,Products/UnitsOnOrder",
    /*7*/    "http://localhost:8181/WcfDataService1.svc/Products()?$format=json&$orderby=ProductID&$top=3&$expand=Category&$select=ProductID,ProductName,CategoryID,SupplierID,Category/CategoryID,Category/CategoryName",
    /*8*/    "http://localhost:8181/WcfDataService1.svc/Products()?$format=json&$orderby=ProductID&$top=3&$expand=Supplier&$select=ProductID,ProductName,CategoryID,SupplierID,Supplier/CompanyName,Supplier/ContactName",
    /*9*/    "http://localhost:8181/WcfDataService1.svc/Products()?$format=json&$orderby=ProductID&$top=3&$expand=Category,Supplier&$select=ProductID,ProductName,CategoryID,SupplierID,Category/CategoryName,Supplier/CompanyName,Supplier/ContactName",
    };
    
    for (int i=0; i<urls.Length; i++) {
        var res = GetRes(urls[i]);
        res.Dump(prefix+i);
        //Console.ReadLine();
        //System.IO.File.WriteAllText(prefix+i+".txt", res);
    }
}

string GetRes(string url) {
    var uri = new System.Uri(url);
    var webClient = new System.Net.WebClient();
    var res = webClient.DownloadString(uri);
    return res;
}