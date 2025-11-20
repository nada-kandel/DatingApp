
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUser(AppDbContext context)
    {
        
        if (await context.Users.AnyAsync()) return;// data in database
        var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);

        if (members == null)
        {
            System.Console.WriteLine("No Member in seed Data");
            return;
        }


        foreach (var member in members)
        {
           using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email,
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    Description = member.Description,
                    ImageUrl = member.ImageUrl,
                    Created = member.Created,
                    LastActive = member.LastActive,
                    City = member.City,
                    Country = member.Country,
                    Gender = member.Gender,
                    DateOfBirth = member.DateOfBirth,

                }
            };
            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id,

            });
            context.Users.Add(user);
        }
            await context.SaveChangesAsync();

    }
}
