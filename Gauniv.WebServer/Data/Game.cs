﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gauniv.WebServer.Data
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public byte[] Payload { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Relation many-to-many avec Category (classe imbriquée)
        public ICollection<Category> Categories { get; set; } = new List<Category>();

        // Facultatif : liste des utilisateurs ayant acheté ce jeu
        public ICollection<User> PurchasedByUsers { get; set; } = new List<User>();


    // Classe imbriquée Category
    public  class Category
        {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            public List<Game>? Games { get; set; }// Liste des jeux associés à la catégorie
        }
    }
}
