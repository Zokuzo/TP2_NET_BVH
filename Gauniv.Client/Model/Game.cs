using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windows.System;

namespace Gauniv.Client.Model
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] Payload { get; set; } // Contenu binaire du jeu
        public decimal Price { get; set; }

        // Relation many-to-many avec Category (classe imbriquée)
        public ICollection<Category> Categories { get; set; } = new List<Category>();

        // Facultatif : liste des utilisateurs ayant acheté ce jeu
        public ICollection<User> PurchasedByUsers { get; set; } = new List<User>();

        // Classe imbriquée Category
        public class Category
        {
          
            public int Id { get; set; }

            public string Name { get; set; }

            // Relation inverse : facultative si besoin de naviguer de Category vers Game
            public ICollection<Game> Games { get; set; } = new List<Game>();
        }
    }
}
