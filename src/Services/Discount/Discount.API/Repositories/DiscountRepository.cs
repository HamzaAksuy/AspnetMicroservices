using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affacted = await connection.ExecuteAsync("insert into Coupon (ProductName,Description,Amount) Values(@ProductName,@Description,@Amount)", new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            if (affacted == null) return false;
            return true;
        }

        public async Task<bool?> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var updated = await connection.ExecuteAsync("delete from Coupon  where ProductName = @ProductName", new { ProductName = productName });
            if (updated == null) return false;
            return true;
        }

        public async Task<Coupon> GetCoupon(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("select * from Coupon where ProductName = @ProductName",new { ProductName =productName});
            if (coupon == null) return new Coupon { ProductName = "No Product",Amount=0,Description="No Discount Desc" };
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var updated = await connection.ExecuteAsync("update Coupon set ProductName=@ProductName,Description=@Description,Amount=@Amount where Id=@Id", new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount,Id=coupon.ID });
            if (updated == null) return false;
            return true;
        }
    }
}
