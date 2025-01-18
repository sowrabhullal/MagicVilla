﻿using System.ComponentModel.DataAnnotations;

namespace MagicVillaWebApi.Models
{
    public class LocalUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
