﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApi.Helpers;

namespace WebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WebApi.Controllers.ViewModels.ClientsPlans", b =>
                {
                    b.Property<string>("ClientId")
                        .HasColumnType("text");

                    b.Property<string>("PlanId")
                        .HasColumnType("text");

                    b.HasKey("ClientId", "PlanId");

                    b.HasIndex("PlanId");

                    b.ToTable("ClientsPlans");
                });

            modelBuilder.Entity("WebApi.Controllers.ViewModels.ClientsTrainers", b =>
                {
                    b.Property<string>("ClientId")
                        .HasColumnType("text");

                    b.Property<string>("TrainerId")
                        .HasColumnType("text");

                    b.HasKey("ClientId", "TrainerId");

                    b.HasIndex("TrainerId");

                    b.ToTable("ClientsTrainers");
                });

            modelBuilder.Entity("WebApi.Entities.Category", b =>
                {
                    b.Property<string>("CategoryId")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = "1",
                            Title = "Amatorskie"
                        },
                        new
                        {
                            CategoryId = "2",
                            Title = "Średnio-Zaawansowane"
                        },
                        new
                        {
                            CategoryId = "3",
                            Title = "Profesjonalistyczne"
                        });
                });

            modelBuilder.Entity("WebApi.Entities.Exercise", b =>
                {
                    b.Property<string>("ExerciseId")
                        .HasColumnType("text");

                    b.Property<string>("CategoryId")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<List<byte[]>>("Files")
                        .HasColumnType("bytea[]");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PlanId")
                        .HasColumnType("text");

                    b.Property<int>("Series")
                        .HasColumnType("integer");

                    b.Property<int>("Times")
                        .HasColumnType("integer");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("ExerciseId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PlanId");

                    b.ToTable("Exercises");

                    b.HasData(
                        new
                        {
                            ExerciseId = "a",
                            CategoryId = "1",
                            Description = "W podciąganiu na drążku podchwytem, sam chwyt nie różni się od tego w innych ćwiczeniach wielostawowych z obciążeniem. Podchwyt to oczywiście ustawienie rąk w supinacji, czyli wewnętrzną częścią dłoni w naszą stronę. Drążek chwytamy jak najmocniej i oplatając go kciukiem.",
                            Name = "Podciąganie nad chwyt",
                            Series = 7,
                            Times = 4,
                            Weight = 0
                        },
                        new
                        {
                            ExerciseId = "b",
                            CategoryId = "1",
                            Description = "Nasze mięśnie czworogłowe dają z siebie wszystko już na samym dole przysiadu, jako że przy siadach high bar ciężar jest mniejszy, kolana mogą wysunąć się trochę bardziej do przodu, bo moment siły potrzebny do wyprostowania kolana jest taki sam, jak przy siadzie low bar z cięższą sztangą.",
                            Name = "Przysiady ze sztangą (high bar)",
                            Series = 7,
                            Times = 4,
                            Weight = 45
                        },
                        new
                        {
                            ExerciseId = "c",
                            CategoryId = "1",
                            Description = "Hip thrust, czyli wypychanie bioder w podporze grzbietem o ławeczkę oraz glute bridge, czyli unoszenie bioder w pozycji leżącej to aktualnie jedne z najskuteczniejszych ćwiczeń na mięśnie pośladkowe!",
                            Name = "Glut bridge jednorożec",
                            Series = 9,
                            Times = 3,
                            Weight = 15
                        },
                        new
                        {
                            ExerciseId = "d",
                            CategoryId = "1",
                            Description = "Utrzymuj prawidłową pozycję wyjściową, napinaj mocno mięśnie nóg, pośladki oraz brzuch, utrzymaj pozycję przez wyznaczony czas, wykonaj izometryczny skurcz mięśni oraz oddychaj głęboko.",
                            Name = "Deska bokiem",
                            Series = 27,
                            Times = 2,
                            Weight = 0
                        },
                        new
                        {
                            ExerciseId = "e",
                            CategoryId = "1",
                            Description = "Dziękuję bardzo za odpowiedź! czy mogę wykonywać wznosy bokiem hantlami bo chce zacząć chodzić na siłownie,mialem przerwę i chce znowu zacząć chodzić. Czy jakoś te wznosy mogą przyhamowac wzrost czy coś i czy mogę je wykonywać?",
                            Name = "Spiętki",
                            Series = 7,
                            Times = 4,
                            Weight = 0
                        },
                        new
                        {
                            ExerciseId = "f",
                            CategoryId = "2",
                            Description = "Spacer farmera (ang. Farmer's Walk) – konkurencja zawodów siłaczy. Zadaniem zawodnika jest podniesienie z podłoża dwóch ciężarów (tzw. „walizek”) – po jednym w każdej z dłoni – i pokonaniu z obydwoma dystansu.",
                            Name = "Spacer farmera",
                            Series = 0,
                            Times = 0,
                            Weight = 25
                        },
                        new
                        {
                            ExerciseId = "g",
                            CategoryId = "2",
                            Description = "",
                            Name = "Martwy ciąg sumo",
                            Series = 0,
                            Times = 0,
                            Weight = 35
                        },
                        new
                        {
                            ExerciseId = "h",
                            CategoryId = "2",
                            Description = "Najprościej można powiedzieć, że martwy ciąg klasyczny wykonujemy rozstawiając nogi na szerokość bioder, a martwy ciąg sumo robimy na nogach rozstawionych szeroko, pilnując, aby ręce znajdowały się wewnątrz ich nawisu.",
                            Name = "Martwy Ciąg",
                            Series = 0,
                            Times = 0,
                            Weight = 43
                        },
                        new
                        {
                            ExerciseId = "i",
                            CategoryId = "2",
                            Description = "W pozycji górnej ćwiczenia napnij łydki.Powoli opuść się z powrotem do pozycji wyjściowej, abyś czuł pełne rozciąganie w łydkach.Nie uginaj kolan, by wytworzyć pęd podczas unoszenia się na palcach stóp.",
                            Name = "Uginanie na łydki stojąc",
                            Series = 27,
                            Times = 2,
                            Weight = 35
                        },
                        new
                        {
                            ExerciseId = "j",
                            CategoryId = "3",
                            Description = "1) Połóż się na ławce płaskiej. 2) Stopy ustaw w lekkim rozkroku i mocno zaprzyj o podłoże. 3) Chwyć sztangę nachwytem (palce wskazują przód, kciuki skierowane do środka) na taką szerokość, aby w połowie wykonywania ruchu kąt między ramieniem a przedramieniem wynosił 90 stopni.",
                            Name = "Wyciskanie na płaskiej",
                            Series = 2,
                            Times = 5,
                            Weight = 60
                        },
                        new
                        {
                            ExerciseId = "k",
                            CategoryId = "3",
                            Description = "Wznosy bokiem, wznosy sztangielek bokiem, lub odwodzenie ramion w bok ze sztangielkami (ang. Shoulder Fly, dumbbell deltoid raise) - ćwiczenie fizyczne polegające na podnoszeniu ramionami ciężaru (najczęściej hantli) stosowane podczas treningu kulturystycznego.",
                            Name = "Wznosy bokiem",
                            Series = 3,
                            Times = 5,
                            Weight = 25
                        },
                        new
                        {
                            ExerciseId = "l",
                            CategoryId = "3",
                            Description = "Najprościej można powiedzieć, że martwy ciąg klasyczny wykonujemy rozstawiając nogi na szerokość bioder, a martwy ciąg sumo robimy na nogach rozstawionych szeroko, pilnując, aby ręce znajdowały się wewnątrz ich nawisu.",
                            Name = "Martwy ciąg sumo",
                            Series = 0,
                            Times = 0,
                            Weight = 35
                        },
                        new
                        {
                            ExerciseId = "m",
                            CategoryId = "3",
                            Description = "1) Zajmij miejsce na maszynie, dostosowując ją do swojego wzrostu.Kończyny dolne wyprostowane, wałek maszyny znajduje się kilka centymetrów poniżej łydek.Chwyć za uchwyty znajdujące się po bokach siedziska.",
                            Name = "Uginanie na dwójki na maszynie",
                            Series = 0,
                            Times = 0,
                            Weight = 43
                        },
                        new
                        {
                            ExerciseId = "n",
                            CategoryId = "3",
                            Description = " Z pozycji, w której stopa jest mocno zadarta do góry, pięta skrajnie obniżona, palce wskazują sufit, a łydka jest mocno rozciągnięta, odpychaj się od podwyższenia poprzez mocne wspięcie na palce i napięcie łydek.",
                            Name = "Uginanie na łydki stojąc",
                            Series = 27,
                            Times = 2,
                            Weight = 35
                        },
                        new
                        {
                            ExerciseId = "o",
                            CategoryId = "3",
                            Description = "musculus triceps brachii) - mięsień zajmujący całą powierzchnię tylną ramienia i należący do tylnej grupy mięśni ramienia, rozpięty między łopatką i kością",
                            Name = "Triceps",
                            Series = 5,
                            Times = 1,
                            Weight = 7
                        });
                });

            modelBuilder.Entity("WebApi.Entities.Organization", b =>
                {
                    b.Property<string>("OrganizationId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("OrganizationId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("WebApi.Entities.Plan", b =>
                {
                    b.Property<string>("PlanId")
                        .HasColumnType("text");

                    b.Property<string>("ClientUserId")
                        .HasColumnType("text");

                    b.Property<string>("CreatorId")
                        .HasColumnType("text");

                    b.Property<string>("CreatorName")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("TrainerUserId")
                        .HasColumnType("text");

                    b.HasKey("PlanId");

                    b.HasIndex("ClientUserId");

                    b.HasIndex("TrainerUserId");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("WebApi.Entities.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("bytea");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("OrganizationId")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<int>("PhoneNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("WebApi.Entities.Client", b =>
                {
                    b.HasBaseType("WebApi.Entities.User");

                    b.Property<string>("ClientId")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Client");

                    b.HasData(
                        new
                        {
                            UserId = "u1",
                            Email = "tgianelli0@eventbrite.com",
                            FirstName = "Teodoor",
                            LastName = "Gianelli",
                            Password = "Teodor",
                            PhoneNumber = 555555555,
                            Role = "User",
                            Token = "t-user",
                            ClientId = "u1"
                        },
                        new
                        {
                            UserId = "u2",
                            Email = "jcasson3@prlog.org",
                            FirstName = "Jillana",
                            LastName = "Casson",
                            Password = "Jillana",
                            PhoneNumber = 666666666,
                            Role = "User",
                            Token = "t-trainer",
                            ClientId = "u2"
                        },
                        new
                        {
                            UserId = "u3",
                            Email = "Teloinic@gmail.com",
                            FirstName = "Camille",
                            LastName = "Teloinic",
                            Password = "Teodor",
                            PhoneNumber = 555555555,
                            Role = "User",
                            Token = "t-user",
                            ClientId = "u3"
                        },
                        new
                        {
                            UserId = "u4",
                            Email = "kburgne2@hp.com",
                            FirstName = "Kiel",
                            LastName = "Burgne",
                            Password = "Kiel",
                            PhoneNumber = 777777777,
                            Role = "User",
                            Token = "t-trainer",
                            ClientId = "u4"
                        },
                        new
                        {
                            UserId = "u5",
                            Email = "awharinu@tmall.com",
                            FirstName = "Augustus",
                            LastName = "Wharin",
                            Password = "Augustus",
                            PhoneNumber = 555555555,
                            Role = "User",
                            Token = "t-user",
                            ClientId = "u5"
                        },
                        new
                        {
                            UserId = "u6",
                            Email = "bcaullieres@auda.org.au",
                            FirstName = "Bondy",
                            LastName = "Caulliere",
                            Password = "Bondy",
                            PhoneNumber = 666666666,
                            Role = "User",
                            Token = "t-trainer",
                            ClientId = "u6"
                        });
                });

            modelBuilder.Entity("WebApi.Entities.Trainer", b =>
                {
                    b.HasBaseType("WebApi.Entities.User");

                    b.Property<string>("TrainerId")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Trainer");

                    b.HasData(
                        new
                        {
                            UserId = "t1",
                            Email = "vmaccathay17@house.gov",
                            FirstName = "Valentia",
                            LastName = "MacCathay",
                            Password = "Valentia",
                            PhoneNumber = 777777777,
                            Role = "Trainer",
                            Token = "t-organization",
                            TrainerId = "t1"
                        },
                        new
                        {
                            UserId = "t2",
                            Email = "efearey1f@mlb.com",
                            FirstName = "Eadith",
                            LastName = "Fearey",
                            Password = "Eadith",
                            PhoneNumber = 777777777,
                            Role = "Trainer",
                            Token = "t-organization",
                            TrainerId = "t2"
                        });
                });

            modelBuilder.Entity("WebApi.Controllers.ViewModels.ClientsPlans", b =>
                {
                    b.HasOne("WebApi.Entities.Client", "Client")
                        .WithMany("ClientsPlans")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.Plan", "Plan")
                        .WithMany("ClientsPlans")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Controllers.ViewModels.ClientsTrainers", b =>
                {
                    b.HasOne("WebApi.Entities.Client", "Client")
                        .WithMany("ClientsTrainers")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.Trainer", "Trainer")
                        .WithMany("ClientsTrainers")
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Entities.Exercise", b =>
                {
                    b.HasOne("WebApi.Entities.Category", null)
                        .WithMany("Exercises")
                        .HasForeignKey("CategoryId");

                    b.HasOne("WebApi.Entities.Plan", null)
                        .WithMany("Exercises")
                        .HasForeignKey("PlanId");
                });

            modelBuilder.Entity("WebApi.Entities.Plan", b =>
                {
                    b.HasOne("WebApi.Entities.Client", null)
                        .WithMany("Plans")
                        .HasForeignKey("ClientUserId");

                    b.HasOne("WebApi.Entities.Trainer", null)
                        .WithMany("Plans")
                        .HasForeignKey("TrainerUserId");
                });

            modelBuilder.Entity("WebApi.Entities.User", b =>
                {
                    b.HasOne("WebApi.Entities.Organization", null)
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId");
                });
#pragma warning restore 612, 618
        }
    }
}
