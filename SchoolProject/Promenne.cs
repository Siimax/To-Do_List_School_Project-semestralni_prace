using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SchoolProject
{
    public class Ukol
    {
        public required string Predmet { get; set; }
        public Status Status { get; set; }
        public DateTime? Termin { get; set; }
        public bool Dulezitost { get; set; }
    }

    public enum Status
    {
        čeká,
        probíhá,
        hotovo
    }
}