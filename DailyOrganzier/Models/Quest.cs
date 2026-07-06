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
    }
}
