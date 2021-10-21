using System;

namespace Sisteg_Dashboard
{
    class BudgetedProduct
    {
        private Int32 idProdutoOrcado;
        private Int32 idProduto;
        private Int32 numeroOrcamento;
        private Int32 item;
        private Int32 quantidadeProduto;
        private Decimal valorTotal;

        public BudgetedProduct()
        {
            this.idProdutoOrcado = 0;
            this.idProduto = 0;
            this.numeroOrcamento = 0;
            this.item = 0;
            this.quantidadeProduto = 0;
            this.valorTotal = 0;
        }

        public Int32 IdProdutoOrcado
        {
            get { return idProdutoOrcado; }
            set { this.idProdutoOrcado = value; }
        }

        public Int32 IdProduto
        {
            get { return idProduto; }
            set { this.idProduto = value; }
        }

        public Int32 NumeroOrcamento
        {
            get { return numeroOrcamento; }
            set { this.numeroOrcamento = value; }
        }

        public Int32 Item
        {
            get { return item; }
            set { this.item = value; }
        }

        public Int32 QuantidadeProduto
        {
            get { return quantidadeProduto; }
            set { this.quantidadeProduto = value; }
        }

        public Decimal ValorTotal
        {
            get { return valorTotal; }
            set { this.valorTotal = value; }
        }
    }
}
