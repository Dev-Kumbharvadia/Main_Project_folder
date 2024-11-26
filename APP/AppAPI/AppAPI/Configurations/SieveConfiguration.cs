using AppAPI.Models.Domain;
using Sieve.Services;

namespace AppAPI.Configurations
{
    public class SieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Product>(p => p.ProductName)
                  .CanFilter()
                  .CanSort();

            // Other properties
            mapper.Property<Product>(p => p.Description)
                  .CanFilter()
                  .CanSort();
            mapper.Property<Product>(p => p.Price)
                  .CanFilter()
                  .CanSort();
            mapper.Property<Product>(p => p.CreatedAt)
                  .CanFilter()
                  .CanSort();
        }
    }
}
