using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestWebApiApp.Models;

namespace TestWebApiApp.Data
{
    public class UsersData
    {
        private static List<User> _users = new List<User>();
        private const string FILENAME = "users.json";
        public static List<User> GetUsers()
        {
            if (File.Exists(FILENAME))
            {
               
                // Dosya varsa, dosyadaki verileri kullan
                string jsonData = File.ReadAllText(FILENAME);
                _users = JsonConvert.DeserializeObject<List<User>>(jsonData);
            }
            else
            {
                // Dosya yoksa, örnek kullanıcılarla başla
                User user1 = new User(1, "tarik", "cucun");
                User user2 = new User(2, "emre", "selim");

                _users.Add(user1);
                _users.Add(user2);
            }

            return _users;
        }

    }
}
