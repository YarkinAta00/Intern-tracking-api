using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using İnternApi.Data;
using İnternApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace İnternApi.Controllers
{
	
    [Route("api/[controller]")]
    public class InternshipTrackerController : ControllerBase
	{
        string signinKey = "aspinterntrackerapideneme";
        private readonly ApplicationDBContext applicationDBContext;
    
        public InternshipTrackerController(ApplicationDBContext applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
        }
       
       
        //Verileri gösterme işlemi
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(applicationDBContext.users.ToList());
        }


        //ID'ye göre veriyi gösterme işlemi
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var item = applicationDBContext.users.FirstOrDefault(x => x.ID== id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        //Name'e göre veriyi gösterme işlemi
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var item = applicationDBContext.users.FirstOrDefault(x => x.FirstName == name);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        //Email'e göre veriyi gösterme işlemi
        [HttpGet("GetByEmail")]
        public IActionResult GetByEmail(string email)
        {
            var item = applicationDBContext.users.FirstOrDefault(x => x.Email == email);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        //Veri ekleme işlemi
        [HttpPost("AddUser")]
        public IActionResult AddUser(User user)
        {
            //User ekleniyor
            applicationDBContext.users.Add(user);
            //Eklenen user kayıt ediliyor.
            applicationDBContext.SaveChanges();
            
            return CreatedAtAction(nameof(GetUsers), new { id = user.ID }, user);
        }
        //Kayıt işlemi
        [HttpPost("Register")]
        public IActionResult Register(string firstName, string lastName, string email, string password, string address,string phoneNumber,char gender)
        {
        
            //User ekleniyor
            User user = new User();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email =email;
            user.Password = password;
            user.Address = address;
            user.phoneNumber = phoneNumber;
            user.Gender = gender;
            user.depID = 0;
            user.roleID = 0;
            user.TaskID = 0;
            user.isIntern = true;
            user.isActive = true;
            user.isDeleted = false;
            user.hasPermition = false;
            user.CurrentTask = "";
            user.LatesTask = "";
            user.StartDate = DateTime.Now.Date;
            user.EndDate = DateTime.Now.Date.AddMonths(2);
            applicationDBContext.users.Add(user);
            //Eklenen user kayıt ediliyor.
            applicationDBContext.SaveChanges();

            return Ok("Success");
        }
       
        //Veriyi güncelleme işlemi
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(int id,string email,string password,string address,string phoneNumber)
        {
            var user = applicationDBContext.users.Where(x => x.ID == id).FirstOrDefault();
            user.Email = email;
            user.Password = password;
            user.Address = address;
            user.phoneNumber = phoneNumber;
            applicationDBContext.SaveChanges();
            return Ok("User Updated");
        }
        
        //Veri silme işlemi
        [HttpDelete("DeleteUser")]
        public string DeleteUser(int id)
        {
            var item= applicationDBContext.users.Where(x => x.ID == id).FirstOrDefault();
            applicationDBContext.users.Remove(item);
            applicationDBContext.SaveChanges();
            return "User Deleted";
        }
        
        [HttpPost("Login")]
        public IActionResult Login(string email,string password)
        {
            var myUser = applicationDBContext.users.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
            if (myUser == null)
            {
                return NotFound("Failed");
            }

            string myToken = CreateToken(myUser);
            return Ok(myToken);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public string CreateToken(User user)
        {
            var Claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),

            };
            var bytes = Encoding.UTF8.GetBytes(signinKey);
            SymmetricSecurityKey key = new SymmetricSecurityKey(bytes);
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer:"http://localhost",
                audience: "http://localhost",
                notBefore:DateTime.Now,
                claims:Claims,
                expires:DateTime.Now.AddMinutes(30),
                signingCredentials:credentials
                );
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            return jwtSecurityTokenHandler.WriteToken(token);
        }


      
        [HttpGet("GetAuthorizedUser"), Authorize]
        public IActionResult GetAuthorizedUser()
        {
            var userEmail = User?.FindFirstValue(ClaimTypes.Email);
            var userName = User?.FindFirstValue(ClaimTypes.Name);
            var userSurname = User?.FindFirstValue(ClaimTypes.Surname);
           

            return Ok(new { userName, userSurname, userEmail });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool ValidateToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey));
            try
            {
                JwtSecurityTokenHandler handler = new();
                handler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,


                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return true;
            }
            catch(System.Exception)
            {
                return false;
            }
        } 
        
       
    }
}

