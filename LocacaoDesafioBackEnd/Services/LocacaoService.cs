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

        public decimal CalcularValorTotal(Locacao locacao)
        {
            // Calcular o total de dias da locação
            int totalDias = (locacao.DataPrevisaoTermino - locacao.DataLocacao).Days;

            // Calcular o valor total das diárias
            decimal totalDiarias = totalDias * locacao.ValorDiaria;

            if (locacao.DataDevolucao.HasValue)
            {
                int diasAdicionais = (locacao.DataDevolucao.Value - locacao.DataPrevisaoTermino).Days;

                // Verifica se a devolução foi antes da data prevista
                if (locacao.DataDevolucao < locacao.DataPrevisaoTermino)
                {
                    int diasNaoEfetivados = (locacao.DataPrevisaoTermino - locacao.DataDevolucao.Value).Days;
                    decimal multa = 0;

                    // Calcular a multa com base no plano de locação
                    if (totalDias <= 7)
                    {
                        multa = 0.20m * (diasNaoEfetivados * locacao.ValorDiaria);
                    }
                    else if (totalDias <= 15)
                    {
                        multa = 0.40m * (diasNaoEfetivados * locacao.ValorDiaria);
                    }

                    return totalDiarias - (diasNaoEfetivados * locacao.ValorDiaria) + multa;
                }
                // Se a devolução foi depois da data prevista
                else if (diasAdicionais > 0)
                {
                    return totalDiarias + (diasAdicionais * 50); // R$50,00 por diária adicional
                }
            }

            return totalDiarias; // Valor total se não houver devolução informada
        }

    }


}
