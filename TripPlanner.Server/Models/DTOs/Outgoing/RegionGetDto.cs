﻿using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models.DTOs.Outgoing
{
    public class RegionGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Country { get; set; }
        public List<string> Cities { get; set; }
        public List<string>? Images { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
