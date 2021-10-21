using System;

namespace Sisteg_Dashboard
{
    class Budget
    {
        private Int32 numeroOrcamento;
        private Int32 idCliente;
        private DateTime dataOrcamento;
        private Decimal valorTrabalho;
        private Decimal valorTotal;
        private string condicaoPagamento;
        private Boolean orcamentoConfirmado;

        public Budget()
        {
            this.numeroOrcamento = 0;
            this.idCliente = 0;
            this.dataOrcamento = new DateTime(0, 0, 0);
            this.valorTrabalho = 0;
            this.valorTotal = 0;
            this.condicaoPagamento = null;
            this.orcamentoConfirmado = false;
        }

        public Int32 NumeroOrcamento
        {
            get { return numeroOrcamento; }
            set { this.numeroOrcamento = value; }
        }

        public Int32 IdCliente
        {
            get { return idCliente; }
            set { this.idCliente = value; }
        }

        public DateTime DataOrcamento
        {
            get { return dataOrcamento; }
            set { this.dataOrcamento = value; }
        }

        public Decimal ValorTrabalho
        {
            get { return valorTrabalho; }
            set { this.valorTrabalho = value; }
        }

        public Decimal ValorTotal
        {
            get { return valorTotal; }
            set { this.valorTotal = value; }
        }

        public string CondicaoPagamento
        {
            get { return condicaoPagamento; }
            set { this.condicaoPagamento = value; }
        }

        public Boolean OrcamentoConfirmado
        {
            get { return orcamentoConfirmado; }
            set { this.orcamentoConfirmado = value; }
        }
    }
}
