using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using my_shoppinglist_api.Helpers;
using my_shoppinglist_api.Helpers.Context;
using my_shoppinglist_api.Helpers.enums;
using my_shoppinglist_api.Interfaces;
using my_shoppinglist_api.Models.Authorization;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Services
{
  public class AuthService : IAuthService
  {
    private readonly MainContext _context;
    private readonly IConfiguration _config;
    private readonly IPasswordHasher<User> _passwordHasher;
    public AuthService(MainContext context, IConfiguration config, IPasswordHasher<User> passwordHasher)
    {
      _context = context;
      _config = config;
      _passwordHasher = passwordHasher;
    }

    public async Task<UserLoginResponse> Login(Credentials credentials)
    {
      var user = await _context.User.FirstOrDefaultAsync(x => x.Email == credentials.Email);
      if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, credentials.Password) != PasswordVerificationResult.Success) return null;

      var now = DateTime.Now;
      var expiryDate = now.AddDays(7);
      var refreshToken = TokenHelper.GenerateRefreshToken();
      var tokenResponse = new UserToken()
      {
        UserId = user.Id,
        ExpiryDate = expiryDate,
        LastUpdated = now,
        RefreshToken = refreshToken
      };
      await _context.AddAsync(tokenResponse);
      await _context.SaveChangesAsync();
      var defaultShoppingGroup = await _context.ShoppingGroupUser.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Default);

      var tokens = new TokenResponse() {
        Id = user.Id,
        Role = user.Role,
      }.ToToken(_config);
      tokens.RefreshToken = refreshToken;


      return new UserLoginResponse() {
        Token = tokens,
        Email = user.Email,
        Id = user.Id,
        Role = user.Role,
        DefaultShoppingGroupId = defaultShoppingGroup?.ShoppingGroupId,
      };
    }

    public async Task<bool> Register(User user)
    {
      if(String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(user.Password)) return false;
      user.Role = Role.User;
      user.Password = _passwordHasher.HashPassword(user, user.Password);

      await _context.AddAsync(user);
      await _context.SaveChangesAsync();

      return true;
    }

    public async Task<JWTToken> Refresh(RefreshRequest tokens)
    {
      var claims = TokenResponse.FromToken(tokens.JWTToken);
      var allTokens = _context.UserToken.Where(x => x.UserId == claims.Id);
      var refreshToken = await allTokens.FirstOrDefaultAsync(x => x.RefreshToken == tokens.RefreshToken);
      if(refreshToken == null || refreshToken.ExpiryDate < DateTime.Now) return null;

      var user = await _context.User.FirstOrDefaultAsync(x => x.Id == claims.Id);
      if(user == null) return null;

      var newJwtToken = new TokenResponse() {
        Id = user.Id,
        Role = user.Role
      }.ToToken(_config);
      var newRefreshToken = TokenHelper.GenerateRefreshToken();
      var now = DateTime.Now;
      var expiryDate = now.AddDays(7);


      refreshToken = new UserToken() {
        LastUpdated = now,
        ExpiryDate = expiryDate,
        RefreshToken = newRefreshToken,
        Id = refreshToken.Id,
        UserId = refreshToken.UserId,
        User = refreshToken.User
      };
      
      _context.Update(refreshToken);
      await _context.SaveChangesAsync();
      return new JWTToken() {
        JwtToken = newJwtToken.JwtToken,
        RefreshToken = newRefreshToken,
        Exp = new DateTimeOffset(expiryDate).ToUnixTimeSeconds(),
        Nbf = new DateTimeOffset(now).ToUnixTimeSeconds()
      };
    }

    public async Task<User> CheckToken(string token)
    {
      var userToken = token.Split(' ')[1];
      var tokenSettings = TokenResponse.FromToken(userToken);
      var user = await _context.User.FirstOrDefaultAsync(x => x.Id == tokenSettings.Id);
      return user;
    }
    
  }
}