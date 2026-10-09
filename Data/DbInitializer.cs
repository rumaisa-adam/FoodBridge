using FoodBridge.Models;

namespace FoodBridge.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FoodBridgeDbContext context)
        {
            context.Database.EnsureCreated();

            // Exit if database is already populated
            if (context.Categories.Any()) return;

            // Seed Categories 
            var catBakery = new Category { Name = "Bakery" };
            var catProduce = new Category { Name = "Produce" };
            var catCooked = new Category { Name = "Cooked Meals" };
            var catDairy = new Category { Name = "Dairy" };
            var catPantry = new Category { Name = "Pantry" };
            context.Categories.AddRange(catBakery, catProduce, catCooked, catDairy, catPantry);
            context.SaveChanges();

            // Seed Item Types 
            var types = new List<ItemType>
            {
                new ItemType { Name = "Bread", CategoryID = catBakery.CategoryID },
                new ItemType { Name = "Pastries", CategoryID = catBakery.CategoryID },
                new ItemType { Name = "Cakes", CategoryID = catBakery.CategoryID },
                new ItemType { Name = "Fruit", CategoryID = catProduce.CategoryID },
                new ItemType { Name = "Vegetables", CategoryID = catProduce.CategoryID },
                new ItemType { Name = "Herbs", CategoryID = catProduce.CategoryID },
                new ItemType { Name = "Curry", CategoryID = catCooked.CategoryID },
                new ItemType { Name = "Stew", CategoryID = catCooked.CategoryID },
                new ItemType { Name = "Rice & Pap", CategoryID = catCooked.CategoryID },
                new ItemType { Name = "Sandwiches", CategoryID = catCooked.CategoryID },
                new ItemType { Name = "Milk", CategoryID = catDairy.CategoryID },
                new ItemType { Name = "Yoghurt", CategoryID = catDairy.CategoryID },
                new ItemType { Name = "Cheese", CategoryID = catDairy.CategoryID },
                new ItemType { Name = "Tinned Goods", CategoryID = catPantry.CategoryID },
                new ItemType { Name = "Grains & Cereal", CategoryID = catPantry.CategoryID },
                new ItemType { Name = "Beverages", CategoryID = catPantry.CategoryID }
            };
            context.ItemTypes.AddRange(types);

            // Seed Suburbs 
            var s1 = new Suburb { Name = "Hatfield" };
            var s2 = new Suburb { Name = "Sunnyside" };
            var s3 = new Suburb { Name = "Arcadia" };
            var s4 = new Suburb { Name = "Brooklyn" };
            var s5 = new Suburb { Name = "Menlyn" };
            context.Suburbs.AddRange(s1, s2, s3, s4, s5);

            // Seed Storages
            var st1 = new Storage { Name = "Ambient" };
            var st2 = new Storage { Name = "Chilled" };
            var st3 = new Storage { Name = "Frozen" };
            context.Storages.AddRange(st1, st2, st3);

            // Seed Users 
            var u1 = new User { FirstName = "Farah", LastName = "Omar", PhoneNumber = "0821234567" };
            var u2 = new User { FirstName = "Rumaisa", LastName = "Adam", PhoneNumber = "0719876543" };
            var u3 = new User { FirstName = "Fardeen", LastName = "Cassim", PhoneNumber = "0835551234" };
            var u4 = new User { FirstName = "Zidaan", LastName = "Spector", PhoneNumber = "0612345678" };
            context.Users.AddRange(u1, u2, u3, u4);
            context.SaveChanges();

            var items = new List<FoodItem>
            {
                new FoodItem
                {
                    ItemName = "Sourdough Loaves",
                    CategoryId = catBakery.CategoryID,
                    ItemTypeId = types.First(t => t.Name == "Bread").ItemTypeID,
                    SuburbId = s1.SuburbID,
                    StorageId = st1.StorageID,
                    Quantity = 12,
                    BestBeforeDate = DateTime.Now.AddDays(2),
                    ImageData = "https://images.unsplash.com/photo-1509440159596-0249088772ff?w=600&auto=format&fit=crop&q=80",
                    ItemNotes = "Freshly baked morning sourdough loaves.",
                    Status = "Claimed",
                    PostedByUserId = u1.UserID,
                    ClaimedByUserId = u4.UserID,
                    ClaimDate = DateTime.Now.AddHours(-8)
                },
                new FoodItem
                {
                    ItemName = "Chicken Curry (Cooked)",
                    CategoryId = catCooked.CategoryID,
                    ItemTypeId = types.First(t => t.Name == "Curry").ItemTypeID,
                    SuburbId = s2.SuburbID,
                    StorageId = st2.StorageID,
                    Quantity = 5,
                    BestBeforeDate = DateTime.Now.AddDays(1),
                    ImageData = "https://images.unsplash.com/photo-1764304733301-3a9f335f0c67?q=80&w=1025&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    ItemNotes = "Mild spice Chicken curry.",
                    Status = "Claimed",
                    PostedByUserId = u2.UserID,
                    ClaimedByUserId = u1.UserID,
                    ClaimDate = DateTime.Now.AddHours(-4)
                },
                new FoodItem
                {
                    ItemName = "Organic Potatos",
                    CategoryId = catProduce.CategoryID,
                    ItemTypeId = types.First(t => t.Name == "Vegetables").ItemTypeID,
                    SuburbId = s3.SuburbID,
                    StorageId = st1.StorageID,
                    Quantity = 15,
                    BestBeforeDate = DateTime.Now.AddDays(10),
                    ImageData = "https://images.unsplash.com/photo-1744659751904-3b2e5c095323?q=80&w=1331&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    ItemNotes = "Harvested Organic Potatos.",
                    Status ="Available",
                    PostedByUserId = u3.UserID
                },
                new FoodItem
                {
                    ItemName = "French Baguettes",
                    CategoryId = catBakery.CategoryID,
                    ItemTypeId = types.First(t => t.Name == "Bread").ItemTypeID,
                    SuburbId = s4.SuburbID,
                    StorageId = st1.StorageID,
                    Quantity = 8,
                    BestBeforeDate = DateTime.Now.AddDays(1),
                    ImageData = "https://images.unsplash.com/photo-1568471173242-461f0a730452?q=80&w=1147&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    ItemNotes = "Crispy baguettes.",
                    Status = "Available",
                    PostedByUserId = u1.UserID
                },
                new FoodItem
                {
                    ItemName = "Fresh Mango",
                    CategoryId = catProduce.CategoryID,
                    ItemTypeId = types.First(t => t.Name == "Fruit").ItemTypeID,
                    SuburbId = s5.SuburbID,
                    StorageId = st1.StorageID,
                    Quantity = 25,
                    BestBeforeDate = DateTime.Now.AddDays(6),
                    ImageData = "https://images.unsplash.com/photo-1601493700631-2b16ec4b4716?q=80&w=1935&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    ItemNotes = "Crate of Chaunsa Mangos.",
                    Status = "Available",
                    PostedByUserId = u4.UserID
                },
                new FoodItem
                {
                    ItemName = "Beef Stew Batches",
                    CategoryId = catCooked.CategoryID,
                    ItemTypeId = types.First(t => t.Name == "Stew").ItemTypeID,
                    SuburbId = s1.SuburbID,
                    StorageId = st3.StorageID,
                    Quantity = 6,
                    BestBeforeDate = DateTime.Now.AddDays(14),
                    ImageData = "https://images.unsplash.com/photo-1664741662725-bd131742b7b7?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    ItemNotes = "Frozen beef stew containers.",
                    Status = "Available",
                    PostedByUserId = u2.UserID
                }
            };
            context.FoodItems.AddRange(items);

            // 7. Seed Initial Donation
            context.Donations.Add(new Donations { UserID = u4.UserID, Amount = 12500m, DonationDate = DateTime.Now.AddDays(-1) });

            context.SaveChanges();
        }
    }
}