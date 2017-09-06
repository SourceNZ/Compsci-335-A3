<Query Kind="Program" />

void Main()
{
    var mds = new MyDataSource ();
    // mds.Categories.Dump("Products");
	// UDO
}

public class MyDataSource
{
    static string FOLDER = @".\data\"; // HttpContext.Current.Server.MapPath("/data"); // @"C:\usertmp\";

    static MyDataSource()
    {
        Console.WriteLine($"... loading {FOLDER}\\XCategories.xml");
        // UDO

        Console.WriteLine($"... loading {FOLDER}\\XSuppliers.xml");
        // UDO

        Console.WriteLine($"... loading {FOLDER}\\XProducts.xml");
        // UDO

        Console.WriteLine($"... relating _Categories, _Products and _Suppliers");
		// UDO
		
        Console.WriteLine($"... starting");
    }

    // Enumerables UDO
	
	// Queryables UDO

}

// UDO
public class Category
{
    // UDO
}

// UDO
public class Supplier
{
    // UDO
}

// UDO
public class Product
{
	// UDO
}