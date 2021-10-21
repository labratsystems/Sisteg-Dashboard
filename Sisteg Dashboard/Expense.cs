using System;

namespace Sisteg_Dashboard
{
    class Expense
    {
        private Int32 idDespesa;
        private Int32 idConta;
        private Int32 idCategoria;
        private Decimal valorDespesa;
        private string descricaoDespesa;
        private DateTime dataTransacao;
        private string observacoesDespesa;
        private Boolean pagamentoConfirmado;
        private Boolean repetirParcelarDespesa;
        private Boolean valorFixoDespesa;
        private Int32 repeticoesValorFixoDespesa;
        private Boolean parcelarValorDespesa;
        private Int32 parcelasDespesa;
        private string periodoRepetirParcelarDespesa;

        public Expense()
        {
            this.idDespesa = 0;
            this.idConta = 0;
            this.idCategoria = 0;
            this.valorDespesa = 0;
            this.descricaoDespesa = null;
            this.dataTransacao = new DateTime();
            this.observacoesDespesa = null;
            this.pagamentoConfirmado = false;
            this.repetirParcelarDespesa = false;
            this.valorFixoDespesa = false;
            this.repeticoesValorFixoDespesa = 0;
            this.parcelarValorDespesa = false;
            this.parcelasDespesa = 0;
            this.periodoRepetirParcelarDespesa = null;
        }

        public Int32 IdDespesa
        {
            get{ return idDespesa; }
            set { this.idDespesa = value;  }
        }

        public Int32 IdConta
        {
            get { return idConta; }
            set { this.idConta = value; }
        }

        public Int32 IdCategoria
        {
            get { return idCategoria; }
            set { this.idCategoria = value; }
        }

        public Decimal ValorDespesa
        {
            get { return valorDespesa; }
            set { this.valorDespesa = value; }
        }

        public string DescricaoDespesa
        {
            get { return descricaoDespesa; }
            set { this.descricaoDespesa = value; }
        }

        public DateTime DataTransacao
        {
            get { return dataTransacao; }
            set { this.dataTransacao = value; }
        }

        public string ObservacoesDespesa
        {
            get { return observacoesDespesa; }
            set { this.observacoesDespesa = value; }
        }

        public Boolean PagamentoConfirmado
        {
            get { return pagamentoConfirmado; }
            set { this.pagamentoConfirmado = value; }
        }

        public Boolean RepetirParcelarDespesa
        {
            get { return repetirParcelarDespesa; }
            set { this.repetirParcelarDespesa = value; }
        }

        public Boolean ValorFixoDespesa
        {
            get { return valorFixoDespesa; }
            set { this.valorFixoDespesa = value; }
        }

        public Int32 RepeticoesValorFixoDespesa
        {
            get { return repeticoesValorFixoDespesa; }
            set { this.repeticoesValorFixoDespesa = value; }
        }

        public Boolean ParcelarValorDespesa
        {
            get { return parcelarValorDespesa; }
            set { this.parcelarValorDespesa = value; }
        }

        public Int32 ParcelasDespesa
        {
            get { return parcelasDespesa; }
            set { this.parcelasDespesa = value; }
        }

        public string PeriodoRepetirParcelarDespesa
        {
            get { return periodoRepetirParcelarDespesa; }
            set { this.periodoRepetirParcelarDespesa = value; }
        }
    }
}
