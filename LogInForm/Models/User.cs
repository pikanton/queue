using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInForm.Models
{
    public class User
    {
        private
        int Id;
        string Username;
        string Password;
        string Fio;
        string Group_name;
        string Role;

        public User(int id, string username, string password, string fio, string group_name, string role)
        {
            Id = id;
            Username = username;
            Password = password;
            Fio = fio;
            Group_name = group_name;
            Role = role;
        }
        public User()
        {

        }
        public void SetAll(int id, string username, string password, string gio, string group_name, string role)
        {
            Id = id;
            Username = username;
            Password = password;
            Fio = gio;
            Group_name = group_name;
            Role = role;
        }

        public int Id1 { get => Id; set => Id = value; }
        public string Username1 { get => Username; set => Username = value; }
        public string Password1 { get => Password; set => Password = value; }
        public string Fio1 { get => Fio; set => Fio = value; }
        public string Group_name1 { get => Group_name; set => Group_name = value; }
        public string Role1 { get => Role; set => Role = value; }
    }
}
