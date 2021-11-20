﻿using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using PlanfiApi.Data.Entities;
using PlanfiApi.Data.Entities.Users;
using PlanfiApi.Data.ViewModels;
using PlanfiApi.Models;
using PlanfiApi.Models.ViewModels;

namespace PlanfiApi.Helpers
{
    public class DataContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqlConnectionString = Configuration.GetConnectionString("WebApiDatabase");
            optionsBuilder.UseNpgsql(sqlConnectionString);
        }
        
        public DbSet<Organization> organizations { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<Plan> plans { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<ChatRoom> chatrooms { get; set; }
        public DbSet<UsersPlans> usersplans { get; set; }
        public DbSet<UsersTrainers> userstrainers { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Exercise> exercises { get; set; }
        
        public DbSet<Serie> series { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Organization>()
                .HasMany(b => b.Users)
                .WithOne();
            
            modelBuilder.Entity<Organization>()
                .HasMany(b => b.Plans)
                .WithOne();
            
            // Plans <-> Users relationship
            modelBuilder.Entity<UsersPlans>()
                .HasKey(bc => new { bc.UserId, bc.PlanId });  
            
            modelBuilder.Entity<UsersPlans>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.Plans)
                .HasForeignKey(bc => bc.UserId);  
            
            modelBuilder.Entity<UsersPlans>()
                .HasOne(bc => bc.Plan)
                .WithMany(c => c.Users)
                .HasForeignKey(bc => bc.PlanId);
            
            modelBuilder.Entity<UsersTrainers>()
                .HasKey(bc => new { bc.ClientId, bc.TrainerId });  
            
            modelBuilder.Entity<UsersTrainers>()
                .HasOne(bc => bc.Client)
                .WithMany(b => b.UsersTrainers) 
                .HasForeignKey(bc => bc.ClientId);
            
            modelBuilder.Entity<Plan>()
                .HasMany(b => b.Exercises)
                .WithOne();
            
            modelBuilder.Entity<Category>()
                .HasMany(b => b.Exercises)
                .WithOne();
            
            modelBuilder.Entity<Exercise>()
                .HasMany(b => b.Series)
                .WithOne();
            

            modelBuilder.Entity<Organization>().HasData(
                    new Organization
                    {
                        OrganizationId = "O1",
                        Name = "Apple",
                    },
                    new Organization
                    {
                        OrganizationId = "O2",
                        Name = "Google",
                    },
                    new Organization
                    {
                        OrganizationId = "O3",
                        Name = "Microsoft",
                    }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = "1",
                    Title = "Amatorskie",
                    OrganizationId = "O1",
                },
                new Category
                {
                    CategoryId = "2",
                    Title = "Średnio-Zaawansowane",
                    OrganizationId = "O1",
                },
                new Category
                {
                    CategoryId = "3",
                    Title = "Profesjonalistyczne",
                    OrganizationId = "O1",
                }
            );


            modelBuilder.Entity<Exercise>().HasData(
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Podciąganie nad chwyt",
                    Description = "W podciąganiu na drążku podchwytem, sam chwyt nie różni się od tego w innych ćwiczeniach wielostawowych z obciążeniem. Podchwyt to oczywiście ustawienie rąk w supinacji, czyli wewnętrzną częścią dłoni w naszą stronę. Drążek chwytamy jak najmocniej i oplatając go kciukiem.",
                    Series = null,
                    Files = null,
                    CategoryId = "1",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Przysiady ze sztangą (high bar)",
                    Description = "Nasze mięśnie czworogłowe dają z siebie wszystko już na samym dole przysiadu, jako że przy siadach high bar ciężar jest mniejszy, kolana mogą wysunąć się trochę bardziej do przodu, bo moment siły potrzebny do wyprostowania kolana jest taki sam, jak przy siadzie low bar z cięższą sztangą.",
                    Series = null,
                    Files = null,
                    CategoryId = "1",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Glut bridge jednorożec",
                    Description = "Hip thrust, czyli wypychanie bioder w podporze grzbietem o ławeczkę oraz glute bridge, czyli unoszenie bioder w pozycji leżącej to aktualnie jedne z najskuteczniejszych ćwiczeń na mięśnie pośladkowe!",
                    Series = null,
                    Files = null,
                    CategoryId = "1",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Deska bokiem",
                    Description = "Utrzymuj prawidłową pozycję wyjściową, napinaj mocno mięśnie nóg, pośladki oraz brzuch, utrzymaj pozycję przez wyznaczony czas, wykonaj izometryczny skurcz mięśni oraz oddychaj głęboko.",
                    Series = null,
                    Files = null,
                    CategoryId = "1",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Spiętki",
                    Description = "Dziękuję bardzo za odpowiedź! czy mogę wykonywać wznosy bokiem hantlami bo chce zacząć chodzić na siłownie,mialem przerwę i chce znowu zacząć chodzić. Czy jakoś te wznosy mogą przyhamowac wzrost czy coś i czy mogę je wykonywać?",
                    Series = null,
                    Files = null,
                    CategoryId = "1",
                },

                new Exercise
                 {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Spacer farmera",
                    Description = "Spacer farmera (ang. Farmer's Walk) – konkurencja zawodów siłaczy. Zadaniem zawodnika jest podniesienie z podłoża dwóch ciężarów (tzw. „walizek”) – po jednym w każdej z dłoni – i pokonaniu z obydwoma dystansu.",
                    Series = null,
                    Files = null,
                    CategoryId = "2",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Martwy ciąg sumo",
                    Description = "",
                    Series = null,
                    Files = null,
                    CategoryId = "2",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Martwy Ciąg",
                    Description = "Najprościej można powiedzieć, że martwy ciąg klasyczny wykonujemy rozstawiając nogi na szerokość bioder, a martwy ciąg sumo robimy na nogach rozstawionych szeroko, pilnując, aby ręce znajdowały się wewnątrz ich nawisu.",
                    Series = null,
                    Files = null,
                    CategoryId = "2",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Uginanie na łydki stojąc",
                    Description = "W pozycji górnej ćwiczenia napnij łydki.Powoli opuść się z powrotem do pozycji wyjściowej, abyś czuł pełne rozciąganie w łydkach.Nie uginaj kolan, by wytworzyć pęd podczas unoszenia się na palcach stóp.",
                    Series = null,
                    Files = null,
                    CategoryId = "2",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Wyciskanie na płaskiej",
                    Description = "1) Połóż się na ławce płaskiej. 2) Stopy ustaw w lekkim rozkroku i mocno zaprzyj o podłoże. 3) Chwyć sztangę nachwytem (palce wskazują przód, kciuki skierowane do środka) na taką szerokość, aby w połowie wykonywania ruchu kąt między ramieniem a przedramieniem wynosił 90 stopni.",
                    Series = null,
                    Files = null,
                    CategoryId = "3",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Wznosy bokiem",
                    Description = "Wznosy bokiem, wznosy sztangielek bokiem, lub odwodzenie ramion w bok ze sztangielkami (ang. Shoulder Fly, dumbbell deltoid raise) - ćwiczenie fizyczne polegające na podnoszeniu ramionami ciężaru (najczęściej hantli) stosowane podczas treningu kulturystycznego.",
                    Series = null,
                    Files = null,
                    CategoryId = "3",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Martwy ciąg sumo",
                    Description = "Najprościej można powiedzieć, że martwy ciąg klasyczny wykonujemy rozstawiając nogi na szerokość bioder, a martwy ciąg sumo robimy na nogach rozstawionych szeroko, pilnując, aby ręce znajdowały się wewnątrz ich nawisu.",
                    Series = null,
                    Files = null,
                    CategoryId = "3",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Uginanie na dwójki na maszynie",
                    Description = "1) Zajmij miejsce na maszynie, dostosowując ją do swojego wzrostu.Kończyny dolne wyprostowane, wałek maszyny znajduje się kilka centymetrów poniżej łydek.Chwyć za uchwyty znajdujące się po bokach siedziska.",
                    Series = null,
                    Files = null,
                    CategoryId = "3",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Uginanie na łydki stojąc",
                    Description = " Z pozycji, w której stopa jest mocno zadarta do góry, pięta skrajnie obniżona, palce wskazują sufit, a łydka jest mocno rozciągnięta, odpychaj się od podwyższenia poprzez mocne wspięcie na palce i napięcie łydek.",
                    Series = null,
                    Files = null,
                    CategoryId = "3",
                },
                new Exercise
                {
                    ExerciseId = Guid.NewGuid().ToString(),
                    Name = "Triceps",
                    Description = "musculus triceps brachii) - mięsień zajmujący całą powierzchnię tylną ramienia i należący do tylnej grupy mięśni ramienia, rozpięty między łopatką i kością",
                    Series = null,
                    Files = null,
                    CategoryId = "3",
                }
            );
            
                modelBuilder.Entity<Role>().HasData(
                    new Role
                    {
                        Id = "1",
                        Name = "Trainer"
                    },
                    new Role
                    {
                        Id = "2",
                        Name = "User",
                    },
                    new Role
                    {
                        Id = "3",
                        Name = "Owner",
                    }
                );
                
            // O1
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    OrganizationId = "O1",
                    UserId = "u1",
                    Avatar = null,
                    FirstName = "Teodoor",
                    LastName = "Gianelli",
                    //Role = new Role() { Id = "1", Name = "Trainer" },
                    RoleId = "1",
                    Email = "tgianelli0@eventbrite.com",
                    PhoneNumber = "555555555",
                    Password = "Teodor",
                    PasswordHash = new byte[2],
                    PasswordSalt = new byte[5],
                    Token = "t-user",
                    IsActivated = true,
                },
                new User
                {
                    OrganizationId = "O2",
                    UserId = "o2u1",
                    Avatar = null,
                    FirstName = "Jacklyn",
                    LastName = "Meachem",
                    //Role = new Role() { Id = "1", Name = "Trainer" },
                    RoleId = "1",
                    Email = "jmeachem0@eventbrite.com",
                    PhoneNumber = "555555555",
                    Password = "Jacklyn",
                    PasswordHash = new byte[2],
                    PasswordSalt = new byte[5],
                    Token = "t-user",
                    IsActivated = true,
                },
                new User
                {
                    OrganizationId = "O3",
                    UserId = "o3u1",
                    Avatar = null,
                    FirstName = "Titus",
                    LastName = "Hilldrup",
                    //Role = new Role() { Id = "1", Name = "Trainer" },
                    RoleId = "1",
                    Email = "thilldrupe@berkeley.edu",
                    PhoneNumber = "555555555",
                    Password = "Titus",
                    PasswordHash = new byte[2],
                    PasswordSalt = new byte[5],
                    Token = "t-user",
                    IsActivated = true,
                }
            );
            
            
            modelBuilder.NamesToSnakeCase();
        }
    }

    public static class ModelBuilderExtensions
    {

        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
        
        public static void NamesToSnakeCase(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                var tableName = entity.GetTableName();
                entity.SetTableName(ToSnakeCase(tableName));
                
                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    var propertyName = property.GetColumnName(StoreObjectIdentifier.Table(tableName, null));
                    property.SetColumnName(ToSnakeCase(propertyName));
                }
                
            }
        }
    }
}
