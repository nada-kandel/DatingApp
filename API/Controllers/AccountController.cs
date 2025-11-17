using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context , ITokenService tokenService):BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDTO registerdto)
    {
        if (await EmailExists(registerdto.Email)) return BadRequest("Email Not Valid");
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            Email = registerdto.Email,
            DisplayName = registerdto.DisplayName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
            PasswordSalt = hmac.Key,
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user.ToDto(tokenService);
        // return new UserDto
        // {
        //     Id = user.Id,
        //     DisplayName = user.DisplayName,
        //     Email = user.Email,
        //     Token = tokenService.CreateToken(user),
        // };
        
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto logindto)
    {
        var user = await context.Users.SingleOrDefaultAsync(e => e.Email == logindto.Email);
        if (user == null) return Unauthorized("Invalid Email Address");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));
        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        // return new UserDto
        // {
        //     Id = user.Id,
        //     DisplayName = user.DisplayName,
        //     Email = user.Email,
        //     Token = tokenService.CreateToken(user),
        // };
        return user.ToDto(tokenService);
    }
    private async Task<bool> EmailExists( string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
