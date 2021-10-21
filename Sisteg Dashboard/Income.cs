using System;

namespace Sisteg_Dashboard
{
    class Income
    {
        private Int32 idReceita;
        private Int32 idConta;
        private Int32 numeroOrcamento;
        private Int32 idCategoria;
        private Decimal valorReceita;
        private string descricaoReceita;
        private DateTime dataTransacao;
        private string observacoesReceita;
        private Boolean recebimentoConfirmado;
        private Boolean repetirParcelarReceita;
        private Boolean valorFixoReceita;
        private Int32 repeticoesValorFixoReceita;
        private Boolean parcelarValorReceita;
        private Int32 parcelasReceita;
        private string periodoRepetirParcelarReceita;

        public Income()
        {
            this.idReceita = 0;
            this.idConta = 0;
            this.numeroOrcamento = 0;
            this.idCategoria = 0;
            this.valorReceita = 0;
            this.descricaoReceita = null;
            this.dataTransacao = new DateTime();
            this.observacoesReceita = null;
            this.recebimentoConfirmado = false;
            this.repetirParcelarReceita = false;
            this.valorFixoReceita = false;
            this.repeticoesValorFixoReceita = 0;
            this.parcelarValorReceita = false;
            this.parcelasReceita = 0;
            this.periodoRepetirParcelarReceita = null;
        }

        public Int32 IdReceita
        {
            get { return idReceita; }
            set { this.idReceita = value; }
        }

        public Int32 IdConta
        {
            get { return idConta; }
            set { this.idConta = value; }
        }

        public Int32 NumeroOrcamento
        {
            get { return numeroOrcamento; }
            set { this.numeroOrcamento = value; }
        }

        public Int32 IdCategoria
        {
            get { return idCategoria; }
            set { this.idCategoria = value; }
        }

        public Decimal ValorReceita
        {
            get { return valorReceita; }
            set { this.valorReceita = value; }
        }

        public string DescricaoReceita
        {
            get { return descricaoReceita; }
            set { this.descricaoReceita = value; }
        }

        public DateTime DataTransacao
        {
            get { return dataTransacao; }
            set { this.dataTransacao = value; }
        }

        public string ObservacoesReceita
        {
            get { return observacoesReceita; }
            set { this.observacoesReceita = value; }
        }

        public Boolean RecebimentoConfirmado
        {
            get { return recebimentoConfirmado; }
            set { this.recebimentoConfirmado = value; }
        }

        public Boolean RepetirParcelarReceita
        {
            get { return repetirParcelarReceita; }
            set { this.repetirParcelarReceita = value; }
        }

        public Boolean ValorFixoReceita
        {
            get { return valorFixoReceita; }
            set { this.valorFixoReceita = value; }
        }

        public Int32 RepeticoesValorFixoReceita
        {
            get { return repeticoesValorFixoReceita; }
            set { this.repeticoesValorFixoReceita = value; }
        }

        public Boolean ParcelarValorReceita
        {
            get { return parcelarValorReceita; }
            set { this.parcelarValorReceita = value; }
        }

        public Int32 ParcelasReceita
        {
            get { return parcelasReceita; }
            set { this.parcelasReceita = value; }
        }

        public string PeriodoRepetirParcelarReceita
        {
            get { return periodoRepetirParcelarReceita; }
            set { this.periodoRepetirParcelarReceita = value; }
        }
    }
}
