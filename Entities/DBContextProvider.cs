using Entities.Configurtion;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Entities
{
    public class DBContextProvider : IdentityDbContext<User>
    {
        public DBContextProvider(DbContextOptions options):base(options)
           { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Concert>  Concerts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<FloorSeat> FloorSeats { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Promoter> Promoters { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<DynParameters> DynParameters { get; set; }
        public DbSet<Attachments> Attachments {  get; set; }

        public DbSet<TempReservation> TempReservations { get; set; }

        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }


    }
}
