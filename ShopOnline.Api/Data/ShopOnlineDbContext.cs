using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Data
{
    public class ShopOnlineDbContext:DbContext
    {
        public ShopOnlineDbContext(DbContextOptions<ShopOnlineDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
			//Products
			//Beauty Category
			modelBuilder.Entity<Product>().HasData(new Product
			{
				Id = 1,
				Name = "Glazmozaiek 1",
				Description = "Een beeldig stukje glazmozaiek van Ineke Breur",
				ImageURL = "/Images/Glasmozaiek/bp 011.jpg",
				Price = 100,
				Qty = 100,
				CategoryId = 1

			});
			

			//Add users
			modelBuilder.Entity<User>().HasData(new User
			{
				Id = 1,
				UserName = "Bob"

			});
			modelBuilder.Entity<User>().HasData(new User
			{
				Id = 2,
				UserName = "Sarah"

			});

			//Create Shopping Cart for Users
			modelBuilder.Entity<Cart>().HasData(new Cart
			{
				Id = 1,
				UserId = 1

			});
			modelBuilder.Entity<Cart>().HasData(new Cart
			{
				Id = 2,
				UserId = 2

			});
			//Add Product Categories
			modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
			{
				Id = 1,
				Name = "Glasmozaiek",
				IconCSS = "fas fa-spa"
			});
			modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
			{
				Id = 2,
				Name = "Dienbladen",
				IconCSS ="fas fa-couch"
			});
			modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
			{
				Id = 3,
				Name = "Lampjes",
				IconCSS = "fas fa-headphones"
			});
			modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
			{
				Id = 4,
				Name = "Mozaiektegels",
				IconCSS = "fas fa-shoe-prints"
			});


		}

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
