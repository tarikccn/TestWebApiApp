using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestWebApiApp.Data;
using TestWebApiApp.Models;

namespace TestWebApiApp.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase
    {
        private const string FILENAME = "users.json";
        private List<User> _users = UsersData.GetUsers();
        

        [HttpGet]
        public List<User> Get()
        {
            return _users;
        }


        [HttpGet("{id}")]
        public User Get(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {

            if (_users.Any(u => u.Id == user.Id))
            {
                return BadRequest("Aynı kimliğe sahip bir kullanıcı zaten var.");
            }

            _users.Add(user);
            System.IO.File.WriteAllText(FILENAME, JsonConvert.SerializeObject(_users));
            return Ok(user);
        }

        [HttpPut] 
        public User Put([FromBody] User user)
        {
            var editedUser = _users.FirstOrDefault(x => x.Id == user.Id);
            editedUser.FirstName = user.FirstName;
            editedUser.LastName = user.LastName;
            System.IO.File.WriteAllText(FILENAME, JsonConvert.SerializeObject(_users));

            return user;
        }
    }
}
