using System;

namespace Sisteg_Dashboard
{
    class Product
    {
        private Int32 idProduto;
        private Int32 idFornecedor;
        private Int32 idCategoria;
        private string nomeProduto;
        private Decimal valorUnitario;

        public Product()
        {
            this.idProduto = 0;
            this.idFornecedor = 0;
            this.idCategoria = 0;
            this.nomeProduto = null;
            this.valorUnitario = 0;
        }

        public Int32 IdProduto
        {
            get { return idProduto; }
            set { this.idProduto = value; }
        }

        public Int32 IdFornecedor
        {
            get { return idFornecedor; }
            set { this.idFornecedor = value; }
        }

        public Int32 IdCategoria
        {
            get { return idCategoria; }
            set { this.idCategoria = value; }
        }

        public string NomeProduto
        {
            get { return nomeProduto; }
            set { this.nomeProduto = value; }
        }

        public Decimal ValorUnitario
        {
            get { return valorUnitario; }
            set { this.valorUnitario = value; }
        }
    }
}
