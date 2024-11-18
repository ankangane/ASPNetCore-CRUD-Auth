using Microsoft.EntityFrameworkCore;
using Crud_Test.Model;

namespace Crud_Test.Data
{
    public class YourDbContext:DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext>options):base(options) { }
        
        public DbSet<Person> People { get; set; }
    }
}
