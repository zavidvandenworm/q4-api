using Microsoft.EntityFrameworkCore;

namespace q4_api;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
}