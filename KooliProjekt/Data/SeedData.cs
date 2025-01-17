namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void GenerateClients(ApplicationDbContext context)
        {
            if (context.Clients.Any()) // Asenda TableName oma tegeliku tabeli nimega
            {
                Console.WriteLine("Kliendid on juba olemas");
                return; // Kui on andmed, siis ei tee midagi
            }

            var clients = new List<Client>
    {
        new Client { Name = "Jordan Smith", Phonenumber = "5027010", Email = "Jordan.Smith@gmail.com" },
        new Client { Name = "John Doe", Phonenumber = "5027011", Email = "John.Doe@gmail.com" },
        new Client { Name = "Alice Johnson", Phonenumber = "5027012", Email = "Alice.Johnson@gmail.com" },
        new Client { Name = "Michael Brown", Phonenumber = "5027013", Email = "Michael.Brown@gmail.com" },
        new Client { Name = "Emily Davis", Phonenumber = "5027014", Email = "Emily.Davis@gmail.com" },
        new Client { Name = "David Miller", Phonenumber = "5027015", Email = "David.Miller@gmail.com" },
        new Client { Name = "Sophia Wilson", Phonenumber = "5027016", Email = "Sophia.Wilson@gmail.com" },
        new Client { Name = "James Taylor", Phonenumber = "5027017", Email = "James.Taylor@gmail.com" },
        new Client { Name = "Olivia Anderson", Phonenumber = "5027018", Email = "Olivia.Anderson@gmail.com" },
        new Client { Name = "William Thomas", Phonenumber = "5027019", Email = "William.Thomas@gmail.com" }
    };

            context.Clients.AddRange(clients);

            context.SaveChanges();
        }


        public static void GenerateEvents(ApplicationDbContext context)
        {
            if (context.Events.Any()) // Asenda TableName oma tegeliku tabeli nimega
            {
                Console.WriteLine("Üritused on juba olemas");
                return; // Kui on andmed, siis ei tee midagi
            }

            var events = new List<Event>
 {
        new Event { Name = "WinterFest", Date = new DateTime(2024, 11, 25, 9, 0, 0), Description = "Winter Stuff", Seats = "1000", Price = "20m", Summary = "WIHWWRF", Organizer = "SM Co", },
        new Event { Name = "New Year Festival", Date = new DateTime(2024, 12, 5, 9, 0, 0), Description = "Fireworks and stuff", Seats = "5000", Price = "50m", Summary = "WIHWWRF", Organizer = "WT Co.", },
        new Event { Name = "Indepentence drinking festival", Date = new DateTime(2025, 2, 15, 9, 0, 0), Description = "Drinking to celebrate indepentence", Seats = "500", Price = "10m", Summary = "WIHWWRF", Organizer = "WHO Co.", },
        new Event { Name = "Early summer Festival", Date = new DateTime(2025, 4, 17, 9, 0, 0), Description = "Drinking and stuff", Seats = "250", Price = "5m", Summary = "WIHWWRF", Organizer = "NF Co.", },
        new Event { Name = "Spring Carnival", Date = new DateTime(2025, 3, 21, 10, 0, 0), Description = "Carnival rides, games, and fun for all ages", Seats = "3000", Price = "15m", Summary = "WIHWWRF", Organizer = "ABC Co.", },
        new Event { Name = "Halloween Bash", Date = new DateTime(2024, 10, 31, 18, 0, 0), Description = "Costume party with music, dancing, and tricks", Seats = "2000", Price = "30m", Summary = "WIHWWRF", Organizer = "GH Co.", },
        new Event { Name = "Summer Solstice Celebration", Date = new DateTime(2025, 6, 21, 12, 0, 0), Description = "Celebrating the longest day of the year", Seats = "1500", Price = "25m", Summary = "WIHWWRF", Organizer = "TLM Co.", },
        new Event { Name = "Autumn Harvest Festival", Date = new DateTime(2024, 9, 25, 9, 0, 0), Description = "Harvest-themed event with food, music, and dancing", Seats = "1000", Price = "12m", Summary = "WIHWWRF", Organizer = "HGP Co.", },
        new Event { Name = "Valentine's Day Gala", Date = new DateTime(2025, 2, 14, 18, 0, 0), Description = "A romantic evening with dinner, dancing, and music", Seats = "600", Price = "35m", Summary = "WIHWWRF", Organizer = "VCO Co.", },
        new Event { Name = "Music and Arts Festival", Date = new DateTime(2025, 7, 10, 11, 0, 0), Description = "Live music, art displays, and creative workshops", Seats = "4000", Price = "45m", Summary = "WIHWWRF", Organizer = "CREO Co.", }

    };

            context.Events.AddRange(events);

            context.SaveChanges();
        }
        public static void GenerateOrganizers(ApplicationDbContext context)
        {
            if (context.Organizers.Any()) // Asenda TableName oma tegeliku tabeli nimega
            {
                Console.WriteLine("Planeerijad on juba olemas");
                return; // Kui on andmed, siis ei tee midagi
            }

            var organizers = new List<Organizer>
 {
        new Organizer { Name = "SM Co", Description = "Specializes in winter festivals and seasonal events. Known for large-scale outdoor gatherings and winter-themed activities.", },
        new Organizer { Name = "WT Co", Description = "Experts in fireworks displays and New Year's Eve celebrations. They also organize large public festivals and citywide events.", },
        new Organizer { Name = "NF Co", Description = "Known for organizing summer festivals and family-friendly outdoor events. They focus on community participation and entertainment.", },
        new Organizer { Name = "WHO Co", Description = "Specializes in heritage and cultural events. Famous for their Independence Day celebrations and large drinking festivals.", },
        new Organizer { Name = "ABC Co", Description = "Leading in event planning and entertainment.", },
        new Organizer { Name = "GH Co", Description = "Specializing in large-scale festivals and gatherings.", },
        new Organizer { Name = "TLM Co", Description = "Experts in creating vibrant and memorable celebrations.", },
        new Organizer { Name = "HGP Co", Description = "Focused on cultural events and community engagement.", },
        new Organizer { Name = "VCO Co", Description = "Creating magical experiences for couples and romantic events.", },
        new Organizer { Name = "CREO Co", Description = "Passionate about music, arts, and creative workshops.", }

    };

            context.Organizers.AddRange(organizers);

            context.SaveChanges();

        }
    }
}