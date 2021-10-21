using System;

namespace Sisteg_Dashboard
{
    class Parcel
    {
        private Int32 idParcela;
        private Int32 idReceita;
        private Int32 idDespesa;
        private Int32 idConta;
        private Int32 idCategoria;
        private Decimal valorParcela;
        private string descricaoParcela;
        private DateTime dataTransacao;
        private string observacoesParcela;
        private Boolean recebimentoConfirmado;
        private Boolean pagamentoConfirmado;

        public Parcel()
        {
            this.idParcela = 0;
            this.idReceita = 0;
            this.idDespesa = 0;
            this.idConta = 0;
            this.idCategoria = 0;
            this.valorParcela = 0;
            this.descricaoParcela = null;
            this.dataTransacao = new DateTime();
            this.observacoesParcela = null;
            this.recebimentoConfirmado = false;
            this.pagamentoConfirmado = false;
        }

        public Int32 IdParcela
        {
            get { return idParcela; }
            set { this.idParcela = value; }
        }

        public Int32 IdReceita
        {
            get { return idReceita; }
            set { this.idReceita = value; }
        }

        public Int32 IdDespesa
        {
            get { return idDespesa; }
            set { this.idDespesa = value; }
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

        public Decimal ValorParcela
        {
            get { return valorParcela; }
            set { this.valorParcela = value; }
        }

        public string DescricaoParcela
        {
            get { return descricaoParcela; }
            set { this.descricaoParcela = value; }
        }

        public DateTime DataTransacao
        {
            get { return dataTransacao; }
            set { this.dataTransacao = value; }
        }

        public string ObservacoesParcela
        {
            get { return observacoesParcela; }
            set { this.observacoesParcela = value; }
        }

        public Boolean RecebimentoConfirmado
        {
            get { return recebimentoConfirmado; }
            set { this.recebimentoConfirmado = value; }
        }

        public Boolean PagamentoConfirmado
        {
            get { return pagamentoConfirmado; }
            set { this.pagamentoConfirmado = value; }
        }
    }
}
