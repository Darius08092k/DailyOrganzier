using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DailyOrganzier.Models
{
    public class Quest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int XpReward { get; set; }

        // Quest management properties
        public bool IsSystemQuest { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public bool IsCompleted { get; set; } = false;

        // Links this quest to a specific user. Null means it is a global System Quest template.
        public string? UserId { get; set; }
    }
}
