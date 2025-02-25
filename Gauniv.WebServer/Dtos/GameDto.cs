﻿using AutoMapper;
using Elfie.Serialization;
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Dtos;

namespace Gauniv.WebServer.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Name { get; set;  }
        public string Description { get; set; }
        public byte[] Payload { get; set; }
        public decimal Price { get; set; }

    }
}