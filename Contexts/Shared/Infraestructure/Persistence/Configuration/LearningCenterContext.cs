
using Microsoft.EntityFrameworkCore;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using learning_center_webapi.Contexts.IAM.Domain.Model.Aggregates;
using IAMValueObjects = learning_center_webapi.Contexts.IAM.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Profiles.Domain.Model.Aggregates;
using ProfilesValueObjects = learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;
using ProfileValueObjects = learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;
using ReminderValueObjects = learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;

public class LearningCenterContext(DbContextOptions options) : DbContext(options)
{
 
    private DbSet<Teleconsultation> Teleconsultations { get; set; }
    private DbSet<User> Users { get; set; }
    private DbSet<Profile> Profiles { get; set; }
    private DbSet<Reminder> Reminders { get; set; }

    private DbSet<Claim> Claims { get; set; }
    private DbSet<ClaimDocument> ClaimDocuments { get; set; }
    private DbSet<RegisteredObject> RegisteredObjects { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        

        builder.Entity<Teleconsultation>(entity =>
        {
            entity.ToTable("Teleconsultations");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedOnAdd();
            entity.Property(t => t.CreatedDate)
                .IsRequired()
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(t => t.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("datetime");
    
            entity.Property(t => t.Service).IsRequired().HasMaxLength(50);
            entity.Property(t => t.Date).IsRequired().HasMaxLength(10);
            entity.Property(t => t.Time).IsRequired().HasMaxLength(5);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(1000);
            entity.Property(t => t.UserId).IsRequired();  // ✅ Quitar HasMaxLength porque ahora es int
    
            entity.HasIndex(t => t.UserId);
            entity.HasIndex(t => t.Date);
        });
        
        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.CreatedDate)
                .IsRequired()
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(u => u.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("datetime");
    
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
    
            // Email - conversión del Value Object a string
            entity.Property(u => u.Email)
                .HasConversion(
                    email => email.Value,  // De Email a string (para guardar)
                    value => new IAMValueObjects.Email(value))  // De string a Email (para leer)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(255);
    
            // PasswordHash - conversión del Value Object a string
            entity.Property(u => u.PasswordHash)
                .HasConversion(
                    pwd => pwd.Value,  // De PasswordHash a string
                    value => IAMValueObjects.PasswordHash.FromHash(value))  // De string a PasswordHash
                .HasColumnName("PasswordHash")
                .IsRequired()
                .HasMaxLength(500);
    
            // Role - conversión del Value Object a string
            entity.Property(u => u.Role)
                .HasConversion(
                    role => role.Value,  // De UserRole a string
                    value => new IAMValueObjects.UserRole(value))  // De string a UserRole
                .HasColumnName("Rol")
                .IsRequired()
                .HasMaxLength(50);
    
            // Índice único en Email (usando el nombre de la columna)
            entity.HasIndex("Email").IsUnique();
        });
        
        builder.Entity<Reminder>(entity =>
        {
            entity.ToTable("Reminders");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).ValueGeneratedOnAdd();
            entity.Property(r => r.CreatedDate)
                .IsRequired()
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(r => r.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("datetime");
    
            entity.Property(r => r.UserId).IsRequired();
    
            // ReminderTitle Value Object
            entity.Property(r => r.Title)
                .HasConversion(
                    v => v.Value,
                    v => new ReminderValueObjects.ReminderTitle(v))
                .HasColumnName("Title")
                .IsRequired()
                .HasMaxLength(100);
    
            // ReminderType Value Object
            entity.Property(r => r.Type)
                .HasConversion(
                    v => v.Value,
                    v => new ReminderValueObjects.ReminderType(v))
                .HasColumnName("Type")
                .IsRequired()
                .HasMaxLength(50);
    
            // ReminderDate Value Object
            entity.Property(r => r.Date)
                .HasConversion(
                    v => v.Value,
                    v => new ReminderValueObjects.ReminderDate(v))
                .HasColumnName("Date")
                .IsRequired()
                .HasMaxLength(10);
    
            // ReminderTime Value Object
            entity.Property(r => r.Time)
                .HasConversion(
                    v => v.Value,
                    v => new ReminderValueObjects.ReminderTime(v))
                .HasColumnName("Time")
                .IsRequired()
                .HasMaxLength(5);
    
            entity.Property(r => r.Notes)
                .HasMaxLength(500);
    
            entity.HasIndex(r => r.UserId);
            entity.HasIndex(r => r.Date);
        });
        
        builder.Entity<Claim>(entity =>
        {
            entity.ToTable("Claims");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.CreatedDate)
                .IsRequired()
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(c => c.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("datetime");
            entity.Property(c => c.Number).IsRequired().HasMaxLength(50);
            entity.HasIndex(c => c.Number).IsUnique();
            entity.Property(c => c.Type).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Status).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Description).HasMaxLength(2000);
            entity.Property(c => c.RegisteredObjectId).IsRequired(false);
            entity.Property(c => c.UserId).IsRequired();
            entity.Property(c => c.Rating).IsRequired(false);
            entity.Property(c => c.IncidentDate)
                .IsRequired()
                .HasColumnType("datetime");
            entity.HasMany(c => c.Documents)
                .WithOne(d => d.Claim)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ClaimDocument>(entity =>
        {
            entity.ToTable("ClaimDocuments");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id).ValueGeneratedOnAdd();
            entity.Property(d => d.CreatedDate)
                .IsRequired()
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(d => d.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("datetime");
            entity.Property(d => d.Name).IsRequired().HasMaxLength(255);
            entity.Property(d => d.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Data).IsRequired().HasColumnType("LONGTEXT");
            entity.Property(d => d.Size).IsRequired();
            entity.Property(d => d.ClaimId).IsRequired();
        });

        builder.Entity<RegisteredObject>(entity =>
        {
            entity.ToTable("RegisteredObjects");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).ValueGeneratedOnAdd();
            entity.Property(o => o.CreatedDate)
                .IsRequired()
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(o => o.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("datetime");
            entity.Property(o => o.Tipo).IsRequired().HasMaxLength(100);
            entity.Property(o => o.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(o => o.DescripcionBreve).HasMaxLength(1000);
            entity.Property(o => o.NumeroSerie).HasMaxLength(200);
            entity.Property(o => o.Foto).HasColumnType("LONGTEXT");
            entity.Property(o => o.FechaRegistro)
                .IsRequired()
                .HasColumnType("datetime");
            entity.Property(o => o.Precio).HasColumnType("DECIMAL(18,2)");
            entity.Property(o => o.UserId).IsRequired();
            entity.HasIndex(o => o.UserId);
        });


        builder.Entity<Profile>(entity =>
{
    entity.ToTable("Profiles");
    entity.HasKey(p => p.Id);
    entity.Property(p => p.Id).ValueGeneratedOnAdd();
    entity.Property(p => p.CreatedDate)
        .IsRequired()
        .HasColumnType("TIMESTAMP")
        .HasDefaultValueSql("CURRENT_TIMESTAMP");
    entity.Property(p => p.UpdatedDate)
        .IsRequired(false)
        .HasColumnType("datetime");
    
    entity.Property(p => p.UserId).IsRequired();
    
    // PersonName Value Object
    entity.Property(p => p.Name)
        .HasConversion(
            v => v.Value,
            v => new ProfileValueObjects.PersonName(v))
        .HasColumnName("Nombre")
        .IsRequired()
        .HasMaxLength(100);
    
    entity.Property(p => p.Email)
        .HasConversion(
            v => v.Value,
            v => new ProfileValueObjects.Email(v))
        .HasColumnName("Correo")
        .IsRequired()
        .HasMaxLength(255);
    
    // Age Value Object
    entity.Property(p => p.Age)
        .HasConversion(
            v => v.Value,
            v => new ProfileValueObjects.Age(v))
        .HasColumnName("Edad")
        .IsRequired();
    
    // Address Value Object
    entity.Property(p => p.Residence)
        .HasConversion(
            v => v.Value,
            v => new ProfileValueObjects.Address(v))
        .HasColumnName("Residencia")
        .HasMaxLength(200);
    
    // PhoneNumber Value Object
    entity.Property(p => p.Phone)
        .HasConversion(
            v => v.Value,
            v => new ProfileValueObjects.PhoneNumber(v))
        .HasColumnName("Telefono")
        .HasMaxLength(20);
    
    // Gender Value Object
    entity.Property(p => p.Gender)
        .HasConversion(
            v => v.Value,
            v => new ProfileValueObjects.Gender(v))
        .HasColumnName("Genero")
        .IsRequired()
        .HasMaxLength(50);
    
    // EducationLevel Value Object
    entity.Property(p => p.EducationLevel)
        .HasConversion(
            v => v.Value,
            v => new ProfileValueObjects.EducationLevel(v))
        .HasColumnName("NivelInstruccion")
        .IsRequired()
        .HasMaxLength(50);
    
    entity.Property(p => p.PhotoDni)
        .HasColumnName("FotoDni")
        .HasMaxLength(500);
    
    entity.Property(p => p.PhotoCredential)
        .HasColumnName("FotoCredencial")
        .HasMaxLength(500);
    
    entity.Property(p => p.Bio)
        .HasMaxLength(1000);
    
    entity.HasIndex(p => p.UserId).IsUnique();
});
        
    }
}