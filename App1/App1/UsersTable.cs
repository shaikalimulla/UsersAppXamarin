using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace App1
{
    class UsersTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]

        public int Id { get; set; } // AutoIncrement and set primarykey  

        [MaxLength(50)]

        public string Email { get; set; }

        [MaxLength(50), Indexed(Name = "Username", Unique = false)]

        public string Username { get; set; }

        [MaxLength(50)]

        public string Password { get; set; }

    }
}