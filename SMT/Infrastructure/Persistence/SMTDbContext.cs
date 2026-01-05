using Microsoft.EntityFrameworkCore;
using SMT.Domain.Entities;

namespace SMT.Infrastructure.Persistence
{
    public class SMTDbContext : DbContext
    {
        public SMTDbContext(DbContextOptions<SMTDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Board> Boards => Set<Board>();
        public DbSet<Component> Components => Set<Component>();
        public DbSet<OrderBoard> OrderBoards => Set<OrderBoard>();
        public DbSet<BoardComponent> BoardComponents => Set<BoardComponent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // OrderBoard (Composite Key)
            modelBuilder.Entity<OrderBoard>()
                .HasKey(ob => new { ob.OrderId, ob.BoardId });

            modelBuilder.Entity<OrderBoard>()
                .HasOne(ob => ob.Order)
                .WithMany(o => o.OrderBoards)
                .HasForeignKey(ob => ob.OrderId);

            modelBuilder.Entity<OrderBoard>()
                .HasOne(ob => ob.Board)
                .WithMany(b => b.OrderBoards)
                .HasForeignKey(ob => ob.BoardId);

            // -----------------------------
            // BoardComponent (Composite Key)
            // -----------------------------
            modelBuilder.Entity<BoardComponent>()
                .HasKey(bc => new { bc.BoardId, bc.ComponentId });

            modelBuilder.Entity<BoardComponent>()
                .HasOne(bc => bc.Board)
                .WithMany(b => b.BoardComponents)
                .HasForeignKey(bc => bc.BoardId);

            modelBuilder.Entity<BoardComponent>()
                .HasOne(bc => bc.Component)
                .WithMany(c => c.BoardComponents)
                .HasForeignKey(bc => bc.ComponentId);

            //Example DATA
            // IDs
            var order1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var board1Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var board2Id = Guid.Parse("00000000-0000-0000-0000-000000000003");

            var resistorId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var capacitorId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var icId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // --------------------
            // Orders
            // --------------------
            modelBuilder.Entity<Order>().HasData(
                new
                {
                    Id = order1Id,
                    Name = "ASMPT-SMT-ORDER-001",
                    Description = "SMT production order for ASMPT demo line",
                    OrderDate = new DateTime(2025, 1, 10)
                }
            );

            // --------------------
            // Boards
            // --------------------
            modelBuilder.Entity<Board>().HasData(
                new
                {
                    Id = board1Id,
                    Name = "Control Board PCB",
                    Description = "Main control PCB for ASMPT placement system",
                    Length = 120.0,
                    Width = 80.0
                },
                new
                {
                    Id = board2Id,
                    Name = "Power Board PCB",
                    Description = "Power supply PCB for SMT machine",
                    Length = 150.0,
                    Width = 100.0
                }
            );

            // --------------------
            // Components
            // --------------------
            modelBuilder.Entity<Component>().HasData(
                new
                {
                    Id = resistorId,
                    Name = "Resistor 10kΩ",
                    Description = "Standard 10k Ohm SMD resistor"
                },
                new
                {
                    Id = capacitorId,
                    Name = "Capacitor 100nF",
                    Description = "Ceramic capacitor 100nF"
                },
                new
                {
                    Id = icId,
                    Name = "Microcontroller IC",
                    Description = "Main control IC for PCB logic"
                }
            );

            
            // Order ↔ Board (Many-to-Many)
            
            modelBuilder.Entity<OrderBoard>().HasData(
                new
                {
                    OrderId = order1Id,
                    BoardId = board1Id
                },
                new
                {
                    OrderId = order1Id,
                    BoardId = board2Id
                }
            );

            // --------------------
            // Board ↔ Component (Many-to-Many + Quantity)
            // --------------------
            modelBuilder.Entity<BoardComponent>().HasData(
                new
                {
                    BoardId = board1Id,
                    ComponentId = resistorId,
                    Quantity = 20
                },
                new
                {
                    BoardId = board1Id,
                    ComponentId = capacitorId,
                    Quantity = 10
                },
                new
                {
                    BoardId = board1Id,
                    ComponentId = icId,
                    Quantity = 1
                },
                new
                {
                    BoardId = board2Id,
                    ComponentId = resistorId,
                    Quantity = 15
                },
                new
                {
                    BoardId = board2Id,
                    ComponentId = capacitorId,
                    Quantity = 8
                }
            );
        }
    }
}
