namespace GestionParcAuto.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Mileage { get; set; } //KM
        public string Registration { get; set; }
        public string VIN { get; set; }
        public char Status { get; set; }
    }
}
