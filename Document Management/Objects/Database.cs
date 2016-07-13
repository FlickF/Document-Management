using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement
{
    class Database
    {
        static int counter = 0;

        private int Id { get; set; }
        private string Name { get; set; }
        private string Password { get; set; }

        public Database (string name, string password)
        {
            Id = counter++;
            Name = name;
            Password = password;
        }

        public Database(int id, string name, string password)
        {
            if (counter > id)
            {
                Id = counter++;
            } else
            {
                Id = id;
            }

            Name = name;
            Password = password;
        }

        public string getName ()
        {
            return this.Name;
        }

        public int getId ()
        {
            return this.Id;
        }

        public string getPassword ()
        {
            return this.Password;
        }
    }
}
