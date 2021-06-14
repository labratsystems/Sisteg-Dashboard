using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sisteg_Dashboard
{
    class Repeat
    {
        public Int32 idRepeticao;
        public Int32 idReceita;
        public Int32 idDespesa;
        public Int32 idConta;
        public Int32 idCategoria;
        public Decimal valorRepeticao;
        public string descricaoRepeticao;
        public DateTime dataTransacao;
        public string observacoesRepeticao;
        public Boolean recebimentoConfirmado;
        public Boolean pagamentoConfirmado;
    }
}
