using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DailyOrganzier.Models
{
    public class Quest
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int XpReward { get; set; }

        // Tracking properties
        public bool IsCompleted { get; set; } = false;  
        public bool IsSystemQuest { get; set; } = false;

        // Determine if it is in today's active rotation
        public bool IsActive { get; set; } = true;
    }
}
