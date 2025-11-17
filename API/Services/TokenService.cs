using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService //IConfiguration take from DI (Dependency Injection)
//عشان يقرأ منه TokenKey من الـ appsettings.json.
{
    public string CreateToken(AppUser user)
    {
        var tokenkey = config["Tokenkey"] ?? throw new Exception("Cannot Get Token key");//read tokenkey from appsettings.json
        if (tokenkey.Length < 64) //secrity to prodect from brute force.
            throw new Exception("Your Token Key Need to be > = 64 character");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey)); //JWT needed Key not string (string to byte)
        var claims = new List<Claim> //create claims //data to token 
        {
            new(ClaimTypes.Email,user.Email),
            new(ClaimTypes.NameIdentifier,user.Id)
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);//secrity
        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds,
        };
        var tokenHandler = new JwtSecurityTokenHandler();//create and read JWT
        var token = tokenHandler.CreateToken(tokenDescription);// Build token
        return tokenHandler.WriteToken(token);  // translate token to string  
    }
}
