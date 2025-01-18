using AutoMapper;
using MagicVillaWebApi.Data;
using MagicVillaWebApi.Models;
using MagicVillaWebApi.Models.Dto;
using MagicVillaWebApi.Repository.IRepository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVillaWebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly string apisecret;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext db, IConfiguration config, IMapper mapper) { 
            _db=db;
            apisecret = config.GetValue<string>("ApiSettings:apisecret");
            _mapper = mapper;
        }
        public bool IsUnique(string username)
        {
            if (_db.localuser.FirstOrDefault(u => u.UserName == username)==null) {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.localuser.FirstOrDefault(u => u.UserName == loginRequestDto.UserName && u.Password == loginRequestDto.Password);
            if (user == null) {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }
            //if user is found then generate JWT token
            var tokenhandler = new JwtSecurityTokenHandler();
            var key= Encoding.ASCII.GetBytes(apisecret);
            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials=new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenhandler.CreateToken(tokendescriptor);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenhandler.WriteToken(token),
                User = user

            };  
            return loginResponseDto;

        }

        public async Task<LocalUser> Register(RegisterationRequestDto registerationRequestDto)
        {
            LocalUser localUser = new (){ 
                UserName=registerationRequestDto.UserName,
                Password=registerationRequestDto.Password,
                Name=registerationRequestDto.Name,
                Role=registerationRequestDto.Role
            };
            _db.localuser.Add(localUser);
            await _db.SaveChangesAsync();
            localUser.Password = "";
            return localUser;
        }
    }
}
