using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;   

namespace CarCat.Models
{

    public enum TransmissionType
    {
        Manual =0,
        Automatic =1   
    }

    public sealed class Car
    {
        public int Id { get; set; }

        public string? Brand { get; set; }
        public string? Model { get; set; }

        public int Year { get; set; }
        public int HorsePower { get; set; }

        public  int EngineDisplacementCc { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransmissionType Transmission { get; set; }

        public List<string>? Images { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }

    }
}
