namespace GestionParcAuto.Models
{
    public class Expertise
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public string? Note { get; set; }
        public Vehicle Vehicle { get; set; }
        public User? User { get; set; }
    }
}
