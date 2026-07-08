using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DailyOrganzier.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Raw string for development; hash this for production

        // Gamification Stats
        public int CurrentLevel { get; set; } = 1;
        public int CurrentXp { get; set; } = 0;
        public int XpToNextLevel { get; set; } = 100;
    }
}
