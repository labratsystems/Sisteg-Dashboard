using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sisteg_Dashboard
{
    class Income
    {
        public Int32 idReceita;
        public Int32 idConta;
        public Decimal valorReceita;
        public string descricaoReceita;
        public DateTime dataTransacao;
        public string categoriaReceita;
        public string observacoesReceita;
        public Boolean recebimentoConfirmado;
        public Boolean repetirParcelarReceita;
        public Boolean valorFixoReceita;
        public Int32 repeticoesValorFixoReceita;
        public Boolean parcelarValorReceita;
        public Int32 parcelasReceita;
        public string periodoRepetirParcelarReceita;
    }
}
