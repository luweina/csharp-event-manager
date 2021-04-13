using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EventManager.Data
{
    public class Member
    {
        [Key]
        [Display(Name = "Member ID")]
        public int Id { get; set; }

        [Display(Name = "user Name")]
        [Required]
        public string userName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string lastName { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string firstName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string email { get; set; }

        public virtual ICollection<UserEvent> UserEvent { get; set; }
    }

    public class Event
    {
        [Key]
        [Display(Name = "Event ID")]
        public int ID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        
        public string Time { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<UserEvent> UserEvent { get; set; }

    }

    public class UserEvent
    {
        [Key, Column(Order = 0)]
        [Required]
        public int MemberId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public int EventID { get; set; }

        public virtual Member Member { get; set; }
        public virtual Event Event { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEvent>()
                .HasKey(x => new { x.MemberId, x.EventID });

            builder.Entity<UserEvent>()
               .HasOne(x => x.Member)
               .WithMany(x => x.UserEvent)
               .HasForeignKey(fk => new { fk.MemberId })
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserEvent>()
               .HasOne(x => x.Event)
               .WithMany(x => x.UserEvent)
               .HasForeignKey(fk => new { fk.EventID })
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
