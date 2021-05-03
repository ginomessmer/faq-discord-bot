using System;
using System.ComponentModel.DataAnnotations;

namespace FaqDiscordBot.Models
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}