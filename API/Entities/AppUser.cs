using System;
using System.ComponentModel.DataAnnotations.Schema;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string DisplayName { get; set; }
    public string? ImageUrl { get; set; }
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; } //if two user write the same  Password it make uniqe hash
    public Member Member { get; set; } = null!;
}
