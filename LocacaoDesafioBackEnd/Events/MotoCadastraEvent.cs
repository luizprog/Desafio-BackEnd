namespace LocacaoDesafioBackEnd.Events
{
    public class MotoCadastradaEvent
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Placa { get; set; }
        public int Ano { get; set; }
        public string Cor { get; set; }
    }
}
