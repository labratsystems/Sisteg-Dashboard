using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sisteg_Dashboard
{
    class Expense
    {
        public Int32 idDespesa;
        public Int32 idConta;
        public Decimal valorDespesa;
        public string descricaoDespesa;
        public DateTime dataTransacao;
        public string categoriaDespesa;
        public string observacoesDespesa;
        public Boolean pagamentoConfirmado;
        public Boolean repetirParcelarDespesa;
        public Boolean valorFixoDespesa;
        public Int32 repeticoesValorFixoDespesa;
        public Boolean parcelarValorDespesa;
        public Int32 parcelasDespesa;
        public string periodoRepetirParcelarDespesa;
    }
}
