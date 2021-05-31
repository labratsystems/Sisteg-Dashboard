using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sisteg_Dashboard
{
    class Parcel
    {
        public Int32 idParcela;
        public Int32 idReceita;
        public Int32 idDespesa;
        public Decimal valorParcela;
        public string descricaoParcela;
        public DateTime dataTransacao;
        public string categoriaParcela;
        public string observacoesParcela;
        public Boolean recebimentoConfirmado;
        public Boolean pagamentoConfirmado;
    }
}
