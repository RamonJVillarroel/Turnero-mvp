namespace MVP_Turnero.Models
{
    public class TipoServicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ProfesionalId { get; set; }
        public Profesional?  Profesional { get; set; }
        public int Duracion { get; set; }
    }
}
