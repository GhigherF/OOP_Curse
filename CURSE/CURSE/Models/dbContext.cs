using CURSE.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    public string Password { get; set; }
    public string NickName { get; set; }
}


public class dbContext : DbContext
{
    public dbContext() { } // Добавьте конструктор без параметров

    public dbContext(DbContextOptions<dbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Community> Community{ get; set; }
    //public DbSet<private_notes> Community { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Fluent API настройка при необходимости
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Конфигурация для Design-time (например, миграций)
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MyWpfAppDb;Trusted_Connection=True;");
        }
    }
}