
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Data;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
public class AddToCartController : Controller
{
    private const string CartCookieKey = "Cart";
    private readonly ApplicationDbContext _context;
    private readonly IRepository<Catagory> _categoryRepository;

    private readonly IRepository<Cart> _cartRepository;
    private readonly IRepository<Checkout> _checkoutRepository;

    public AddToCartController(ApplicationDbContext context,
        IRepository<Catagory> categoryRepository, IRepository<Cart> cartRepository,
        IRepository<Checkout> checkoutRepository)
    {
        _context = context;
        _categoryRepository = categoryRepository;
        _cartRepository = cartRepository;
        _checkoutRepository = checkoutRepository;
    }
    public IActionResult DeleteCart(string id)
    {
        //string conn = "Data Source=(localdb)\\MSSQLLocalDB;" +
        //       "Initial Catalog=MyDB;Integrated Security=True";
        //IRepository<Cart> rep = new GenericRepository<Cart>(conn);
        _cartRepository.Delete(id);
        return RedirectToAction("ViewCart", "AddToCart");
    }
        public IActionResult AddSession() 
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cartJson = HttpContext.Session.GetString("CartItems");
        var cart = string.IsNullOrEmpty(cartJson) ? new List<CartViewModel>() : JsonSerializer.Deserialize<List<CartViewModel>>(cartJson);
        string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";

        // List<CartViewModel> cartItems = new List<CartViewModel>();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string query = @"
            SELECT TOP 3 c.Id, c.Quantity, p.Name AS ProductName, p.Price
AS ProductPrice,c.TotalPrice
            FROM Cart c
            INNER JOIN Product p ON c.ProductId = p.ProductId
            WHERE c.UserId = @userId ORDER BY c.ProductId DESC"
;

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userId", userId);

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    CartViewModel item = new CartViewModel
                    {
                        ProductId = reader.GetInt32(0),
                        Quantity = reader.GetInt32(1),
                        ProductName = reader.GetString(2),
                        ProductPrice = reader.GetString(3),
                        ProductImagePath = reader.GetString(4),
                        // TotalPrice=reader.GetString(5),
                    };
                    cart.Add(item);

                }
            }

            // Retrieve product details from the database using the provided id
            // Add the new item to the cart
            // For demonstration purposes, let's assume you have retrieved product details and created a CartViewModel object called 'newCartItem'
            //  cart.Add(newCartItem);

            // Serialize the updated cart items
            var updatedCartJson = JsonSerializer.Serialize(cart);

            // Update the session with the updated cart items
            HttpContext.Session.SetString("CartItems", updatedCartJson);

            // Redirect to the view cart page or any other page as needed
            return RedirectToAction("ViewCart", "AddToCart");
        }
    }


    public IActionResult Index()
    {
        string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
        SqlConnection con = new SqlConnection(connString);
        con.Open();
        // Check if the product already exists in the cart for this user
        string checkCartQuery = "Delete from Cart";
        SqlCommand checkCartCmd = new SqlCommand(checkCartQuery, con);
        int del = checkCartCmd.ExecuteNonQuery();
        con.Close();
        // Delete specific cookies
        DeleteCookie(CartCookieKey);
        DeleteCookie("UserId");
        DeleteCookie("ProductId");

        // Return a view or redirect as needed
        return View("ViewCart");
    }
    private void DeleteCookie(string key)
    {
        HttpContext.Response.Cookies.Delete(key);
    }
    [ HttpPost]
    //    public async Task<IActionResult> Cart(string id)
    //    {
    //        var user = User;

    //        if (user.Identity.IsAuthenticated)
    //        {
    //            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //            int productId = int.Parse(id);

    //            // Set the database connection string
    //            string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
    //            SqlConnection con = new SqlConnection(connString);

    //            // Get the product price (NVARCHAR with dollar sign)
    //            string getProductPriceQuery = "SELECT Price FROM Product WHERE ProductId = @productId";
    //            SqlCommand getProductPriceCmd = new SqlCommand(getProductPriceQuery, con);
    //            getProductPriceCmd.Parameters.AddWithValue("@productId", productId);

    //            con.Open();
    //            var priceObj = getProductPriceCmd.ExecuteScalar();
    //            con.Close();

    //            if (priceObj == null)
    //            {
    //                return NotFound("Product not found.");
    //            }

    //            // Extract the numeric part of the price string and convert it to a decimal
    //            string priceString = priceObj.ToString();
    //            decimal productPrice;
    //            if (!TryExtractDecimal(priceString, out productPrice))
    //            {
    //                return BadRequest("Invalid price format.");
    //            }

    //            // Check if the product already exists in the cart for this user
    //            string checkCartQuery = "SELECT Quantity FROM Cart WHERE UserId = @userId AND " +
    //                "Id = @productId";
    //            SqlCommand checkCartCmd = new SqlCommand(checkCartQuery, con);
    //            checkCartCmd.Parameters.AddWithValue("@userId", userId);
    //            checkCartCmd.Parameters.AddWithValue("@productId", productId);

    //            con.Open();
    //            var quantityObj = checkCartCmd.ExecuteScalar();
    //            con.Close();

    //            if (quantityObj != null)
    //            {
    //                // If the product exists, update the quantity and total price
    //                int existingQuantity = Convert.ToInt32(quantityObj);
    //                int newQuantity = existingQuantity + 1;
    //                decimal totalPrice = newQuantity * productPrice;

    //                string updateCartQuery = "UPDATE Cart SET Quantity = @quantity, " +
    //                    "TotalPrice = @totalPrice WHERE UserId = @userId AND Id = @productId";
    //                SqlCommand updateCartCmd = new SqlCommand(updateCartQuery, con);
    //                updateCartCmd.Parameters.AddWithValue("@quantity", newQuantity);
    //                updateCartCmd.Parameters.AddWithValue("@totalPrice", FormatPrice(totalPrice));
    //                updateCartCmd.Parameters.AddWithValue("@userId", userId);
    //                updateCartCmd.Parameters.AddWithValue("@productId", productId);

    //                con.Open();
    //                updateCartCmd.ExecuteNonQuery();
    //                con.Close();
    //            }
    //            else
    //            {
    //                // If the product does not exist, add it to the cart
    //                int initialQuantity = 1;
    //                decimal totalPrice = initialQuantity * productPrice;

    //                Cart c = new Cart
    //                {
    //                    UserId = userId,
    //                    Id = productId,
    //                    Quantity = initialQuantity,
    //                    TotalPrice = FormatPrice(totalPrice)
    //                };
    //                string insertCartQuery = "INSERT INTO Cart" +
    //                    " (Id,UserId, Quantity, TotalPrice) VALUES (@productId,@userId, @quantity, @totalPrice)";
    //                SqlCommand insertCartCmd = new SqlCommand(insertCartQuery, con);
    //                insertCartCmd.Parameters.AddWithValue("@userId", c.UserId);
    //                insertCartCmd.Parameters.AddWithValue("@productId", c.Id);
    //                insertCartCmd.Parameters.AddWithValue("@quantity", c.Quantity);
    //                insertCartCmd.Parameters.AddWithValue("@totalPrice",c.TotalPrice);

    //                con.Open();
    //                insertCartCmd.ExecuteNonQuery();
    //                con.Close();

    //                //string conn = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
    //                //IRepository<Cart> rep = new GenericRepository<Cart>(conn);
    //                //rep.Add(c);

    //            }
    //            SetCookie("UserId", userId, 30);
    //            SetCookie("ProductId", id, 30);
    //            //string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial" +
    //            //              " Catalog=MyDB;Integrated Security=True;";
    //            //SqlConnection con = new SqlConnection(connString);
    //            con.Open();
    //            string query = "SELECT * FROM dbo.Product where ProductId=@id";
    //            SqlCommand cmd = new SqlCommand(query, con);

    //            cmd.Parameters.AddWithValue("@id", id);
    //            SqlDataReader dr = cmd.ExecuteReader();
    //            Product p = new Product();
    //            while (dr.Read())
    //            {
    //                p.Name = dr.GetString(1);
    //                p.Price = dr.GetString(2);
    //                p.ImagePath = dr.GetString(5);
    //            }
    //            con.Close();
    //            var cart = GetCart();

    //            var cartItem = cart.FirstOrDefault(ci => ci.ProductId == id);

    //            if (cartItem != null)
    //            {
    //                cartItem.Quantity += 1;
    //              //  c.Quantity = cartItem.Quantity;

    //            }
    //            else
    //            {
    //                cart.Add(new AddToCart
    //                {
    //                    ProductId = id,
    //                    Quantity = 1,
    //                    Price = p.Price,
    //                    Name = p.Name,
    //                    ImagePath = p.ImagePath
    //                });
    //              ////  c.Quantity = 1;
    //            }

    //          //  rep.Add(c);
    //            SaveCart(cart);


    //            return RedirectToAction("ViewCart", "AddToCart");

    //        }

    //        return RedirectToAction("ViewCart", "AddToCart");
    //    }

    //    sharp
    //Copy code
    [Authorize(Policy = "BusinessHoursOnly")]
    public async Task<IActionResult> Cart(string id)
    {
        var user = User;

        if (user.Identity.IsAuthenticated)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int productId = int.Parse(id);

            // Set the database connection string
            string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
            SqlConnection con = new SqlConnection(connString);

            // Get the product price (NVARCHAR with dollar sign)
            string getProductPriceQuery = "SELECT Price FROM Product" +
                " WHERE ProductId = @productId";
            SqlCommand getProductPriceCmd = new SqlCommand(getProductPriceQuery, con);
            getProductPriceCmd.Parameters.AddWithValue("@productId", productId);

            con.Open();
            var priceObj = getProductPriceCmd.ExecuteScalar();
            con.Close();

            if (priceObj == null)
            {
                return NotFound("Product not found.");
            }

            // Extract the numeric part of the price string and convert it to a decimal
            string priceString = priceObj.ToString();
            decimal productPrice;
            if (!TryExtractDecimal(priceString, out productPrice))
            {
                return BadRequest("Invalid price format.");
            }

            // Check if the product already exists in the cart for this user
            string checkCartQuery = "SELECT Quantity FROM Cart WHERE UserId = @userId AND Id = @productId";
            SqlCommand checkCartCmd = new SqlCommand(checkCartQuery, con);
            checkCartCmd.Parameters.AddWithValue("@userId", userId);
            checkCartCmd.Parameters.AddWithValue("@productId", productId);

            con.Open();
            var quantityObj = checkCartCmd.ExecuteScalar();
            con.Close();

            if (quantityObj != null)
            {
                // If the product exists, update the quantity and total price
                int existingQuantity = Convert.ToInt32(quantityObj);
                int newQuantity = existingQuantity + 1;
                decimal totalPrice = newQuantity * productPrice;

                string updateCartQuery = "UPDATE Cart SET Quantity = @quantity, TotalPrice = @totalPrice WHERE UserId = @userId AND Id = @productId";
                SqlCommand updateCartCmd = new SqlCommand(updateCartQuery, con);
                updateCartCmd.Parameters.AddWithValue("@quantity", newQuantity);
                updateCartCmd.Parameters.AddWithValue("@totalPrice", FormatPrice(totalPrice));
                updateCartCmd.Parameters.AddWithValue("@userId", userId);
                updateCartCmd.Parameters.AddWithValue("@productId", productId);

                con.Open();
                updateCartCmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                // If the product does not exist, add it to the cart
                int initialQuantity = 1;
                decimal totalPrice = initialQuantity * productPrice;

                string insertCartQuery = "INSERT INTO Cart (UserId, Id, Quantity, TotalPrice) VALUES (@userId, @productId, @quantity, @totalPrice)";
                SqlCommand insertCartCmd = new SqlCommand(insertCartQuery, con);
                insertCartCmd.Parameters.AddWithValue("@userId", userId);
                insertCartCmd.Parameters.AddWithValue("@productId", productId);
                insertCartCmd.Parameters.AddWithValue("@quantity", initialQuantity);
                insertCartCmd.Parameters.AddWithValue("@totalPrice", FormatPrice(totalPrice));

                con.Open();
                insertCartCmd.ExecuteNonQuery();
                con.Close();
            }

            SetCookie("UserId", userId, 30);
            SetCookie("ProductId", id, 30);

            con.Open();
            string query = "SELECT * FROM dbo.Product WHERE ProductId = @id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            Product p = new Product();
            while (dr.Read())
            {
                p.Name = dr.GetString(1);
                p.Price = dr.GetString(2);
                p.ImagePath = dr.GetString(5);
            }
            con.Close();

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(ci => ci.ProductId == id);

            if (cartItem != null)
            {
                cartItem.Quantity += 1;
            }
            else
            {
                cart.Add(new AddToCart
                {
                    ProductId = id,
                    Quantity = 1,
                    Price = p.Price,
                    Name = p.Name,
                    ImagePath = p.ImagePath
                });
            }

            SaveCart(cart);
            return RedirectToAction("ViewCart", "AddToCart");
        }

        return RedirectToAction("ViewCart", "AddToCart");
    }

    // Helper method to extract decimal from price string
    private bool TryExtractDecimal(string input, out decimal result)
    {
        result = 0;
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        string numericPart = new string(input.Where(char.IsDigit).ToArray());
        return decimal.TryParse(numericPart, out result);
    }

    // Helper method to format price with dollar sign and decimal places
    private string FormatPrice(decimal price)
    {
        return $"${price:F2}";
    }


   
    public IActionResult ViewCart()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";

        List<CartViewModel> cartItems = new List<CartViewModel>();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string query = @"
            SELECT c.Id, c.Quantity, p.Name AS ProductName, p.Price
AS ProductPrice, p.ImagePath AS ProductImagePath,c.TotalPrice
            FROM Cart c
            INNER JOIN Product p ON c.Id = p.ProductId
            WHERE c.UserId = @userId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userId", userId);

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    CartViewModel item = new CartViewModel
                    {
                        ProductId = reader.GetInt32(0),
                        Quantity = reader.GetInt32(1),
                        ProductName = reader.GetString(2),
                        ProductPrice = reader.GetString(3),
                        ProductImagePath = reader.GetString(4),
                       // TotalPrice=reader.GetString(5),
                    };
                    cartItems.Add(item);
					
				}
            }
            con.Close();
        }

        //string conn = $"Data Source=(localdb)\\MSSQLLocalDB" +
        //        $";Initial Catalog=MyDB;Integrated Security=True";
        //IRepository<Catagory> rep1 = new GenericRepository<Catagory>
        //    (conn);
        List<Catagory> products = _categoryRepository.GetAll().ToList();
        ViewBag.MyList = products;

        return View(cartItems);
    }

    //public IActionResult ViewCart()
    //{
    //    string conn = $"Data Source=(localdb)\\MSSQLLocalDB;" +
    //               $"Initial Catalog=MyDB;Integrated Security=True";
    //    IRepository<Cart> rep = new GenericRepository<Cart>(conn);
    //    List<Cart> carts = rep.GetAll().ToList();

    //   // var cart = GetCart();
    //    return View(carts);
    //}

    private void SetCookie(string key, string value, int? expireTime)
    {
        CookieOptions option = new CookieOptions();

        if (expireTime.HasValue)
            option.Expires = DateTime.Now.AddDays(expireTime.Value);
        else
            option.Expires = DateTime.Now.AddMilliseconds(10);

        HttpContext.Response.Cookies.Append(key, value, option);
    }

    private List<AddToCart> GetCart()
    {
        var cartJson = HttpContext.Request.Cookies[CartCookieKey];
        if (string.IsNullOrEmpty(cartJson))
        {
            return new List<AddToCart>();
        }

        return JsonSerializer.Deserialize<List<AddToCart>>(cartJson);
    }
    private void SaveCart(List<AddToCart> cart)
    {
        var cartJson = JsonSerializer.Serialize(cart);
        SetCookie(CartCookieKey, cartJson, 30);
    }
    public IActionResult Checkout()
    {
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (string.IsNullOrEmpty(userId))
		{
			return RedirectToAction("Login", "Account");
		}

		string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";

		List<CartViewModel> cartItems = new List<CartViewModel>();

		using (SqlConnection con = new SqlConnection(connString))
		{
			string query = @"
            SELECT c.Id, c.Quantity, p.Name AS ProductName, p.Price
AS ProductPrice, p.ImagePath AS ProductImagePath,c.TotalPrice
            FROM Cart c
            INNER JOIN Product p ON c.Id = p.ProductId
            WHERE c.UserId = @userId";

			SqlCommand cmd = new SqlCommand(query, con);
			cmd.Parameters.AddWithValue("@userId", userId);

			con.Open();
			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					CartViewModel item = new CartViewModel
					{
						ProductId = reader.GetInt32(0),
						Quantity = reader.GetInt32(1),
						ProductName = reader.GetString(2),
						ProductPrice = reader.GetString(3),
						ProductImagePath = reader.GetString(4),
						TotalPrice = reader.GetString(5),
					};
					cartItems.Add(item);

				}
			}
            //string conn = $"Data Source=(localdb)\\MSSQLLocalDB" +
            //    $";Initial Catalog=MyDB;Integrated Security=True";
            //IRepository<Catagory> rep1 = new GenericRepository<Catagory>
            //    (conn);
            List<Catagory> products = _categoryRepository.GetAll().ToList();
            ViewBag.MyList = products;
            con.Close();
		}
		decimal totalAmount = cartItems.Sum(item =>
		{
			decimal.TryParse(item.TotalPrice.Trim('$'), out decimal price);
			return price;
		});

		// Pass the total amount to the view using ViewBag
		ViewBag.TotalAmount = totalAmount.ToString("C");


		return View(cartItems);
	}
	

    [HttpPost]
    public IActionResult Checkout(string Name, string Email, string Phone, string PostalCode, string Address, string City, string DeliveryOptions)
    {
        var user = User;

        if (!user.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Set the database connection string
        string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";

        // Retrieve cart items for the user
        List<(int ProductId, int Quantity)> cartItems = new List<(int ProductId, int Quantity)>();
        using (SqlConnection con = new SqlConnection(connString))
        {
            string getCartItemsQuery = "SELECT Id, Quantity FROM Cart WHERE UserId = @userId";
            SqlCommand getCartItemsCmd = new SqlCommand(getCartItemsQuery, con);
            getCartItemsCmd.Parameters.AddWithValue("@userId", userId);

            con.Open();
            using (SqlDataReader reader = getCartItemsCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cartItems.Add((reader.GetInt32(0), reader.GetInt32(1)));
                }
            }
            con.Close();
        }

        if (cartItems.Count == 0)
        {
            // Handle empty cart scenario
            return RedirectToAction("viewCart", "AddToCart");
        }

        // Create an Orders entry
        int orderId;
        using (SqlConnection con = new SqlConnection(connString))
        {
            string insertOrderQuery = "INSERT INTO Orders" +
                " (Name, Email, Phone, PostalCode, Address, City, OrderDate, DeliveryOptions) " +
                                      "OUTPUT INSERTED.Id " +
                                      "VALUES (@Name, @Email, @Phone, @PostalCode, @Address, @City, GETDATE(), @DeliveryOptions)";
            SqlCommand insertOrderCmd = new SqlCommand(insertOrderQuery, con);
            insertOrderCmd.Parameters.AddWithValue("@Name", Name);
            insertOrderCmd.Parameters.AddWithValue("@Email", Email);
            insertOrderCmd.Parameters.AddWithValue("@Phone", Phone);
            insertOrderCmd.Parameters.AddWithValue("@PostalCode", PostalCode);
            insertOrderCmd.Parameters.AddWithValue("@Address", Address);
            insertOrderCmd.Parameters.AddWithValue("@City", City);
            insertOrderCmd.Parameters.AddWithValue("@DeliveryOptions", DeliveryOptions);

            con.Open();
            orderId = (int)insertOrderCmd.ExecuteScalar();
            con.Close();
        }

        // Insert items into the OrderItems table
        using (SqlConnection con = new SqlConnection(connString))
        {
            con.Open();
            foreach (var item in cartItems)
            {
                string insertOrderItemQuery = "INSERT INTO OrderItems (OrderId, ProductId, Quantity) VALUES (@OrderId, @ProductId, @Quantity)";
                SqlCommand insertOrderItemCmd = new SqlCommand(insertOrderItemQuery, con);
                insertOrderItemCmd.Parameters.AddWithValue("@OrderId", orderId);
                insertOrderItemCmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                insertOrderItemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                insertOrderItemCmd.ExecuteNonQuery();
            }
            con.Close();
        }
        // Clear the user's cart
        using (SqlConnection con = new SqlConnection(connString))
        {
            string clearCartQuery = "DELETE FROM Cart WHERE UserId = @userId";
            SqlCommand clearCartCmd = new SqlCommand(clearCartQuery, con);
            clearCartCmd.Parameters.AddWithValue("@userId", userId);

            con.Open();
            clearCartCmd.ExecuteNonQuery();
            con.Close();
        }
        // Redirect to a success page or another action
        return RedirectToAction("Submit", "Order");
    }
}

