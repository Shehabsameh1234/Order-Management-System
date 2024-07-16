using OrderSys.Core.Entities;
using OrderSys.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderSys.Repository.DataSeeding
{
    public static class DataSeed
    {
        public async static Task SeedAsync(OrderManagementDbContext OrderManagementDbContext)

        {
            if (OrderManagementDbContext.Products.Count() == 0)
            {
                
                var productsData = File.ReadAllText("../OrederSys.Core/DataSeeding/Files/products.json");
                //2-Deserialize from json to list
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                //3-add to data base
                if (products?.Count() > 0)
                {

                    foreach (var product in products)
                    {
                        OrderManagementDbContext.Products.Add(product);
                    }
                    await OrderManagementDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
