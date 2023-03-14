using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestWebApiApp.Data;
using TestWebApiApp.Models;
using MySql.Data.MySqlClient;
using System;

namespace TestWebApiApp.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase
    {
        private const string FILENAME = "users.json";
        private List<User> _users = UsersData.GetUsers();
        string connectionString = "Server=192.168.1.21;Port=3306;Database=DenemeDB;Uid=remote_user;Password=rootroot;";
        //veritabanindaki butun veriler cek //GET METHOD
        [HttpGet]
        public List<User> Get()
        {
            return _users;
        }

        //veritabanindaki eslesen id verilerini cek //GET METHOD
        [HttpGet("{id}")]
        public User Get(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users (firstname, lastname) VALUES (@firstname, @lastname);";

            //command.Parameters.AddWithValue("@id", user.Id);
            command.Parameters.AddWithValue("@firstname", user.FirstName);
            command.Parameters.AddWithValue("@lastname", user.LastName);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                return BadRequest("Kullanıcı eklenirken bir hata oluştu.");
            }
        }

        [HttpPut]
        public User Put([FromBody] User user)
        {
            var editedUser = _users.FirstOrDefault(x => x.Id == user.Id);
            if (editedUser != null)
            {
                editedUser.FirstName = user.FirstName;
                editedUser.LastName = user.LastName;

                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                int rowsAffected = command.ExecuteNonQuery();

                connection.Close();


                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
