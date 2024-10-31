using LocacaoDesafioBackEnd.Models;
using System;

namespace LocacaoDesafioBackEnd.Services
{
    public class LocacaoService
    {
        public decimal CalcularValorLocacao(int dias)
        {
            if (dias == 7) return dias * 30.00m;
            if (dias == 15) return dias * 28.00m;
            if (dias == 30) return dias * 22.00m;
            if (dias == 45) return dias * 20.00m;
            if (dias == 50) return dias * 18.00m;
            throw new ArgumentException("Plano de locação inválido.");
        }

        public DateTime CalcularDataInicio(DateTime dataCriacao)
        {
            return dataCriacao.AddDays(1);
        }

        public bool VerificarHabilitacaoCategoriaA(Entregador entregador)
        {
            return entregador.TipoCNH.Contains("A");
        }
    }
}
