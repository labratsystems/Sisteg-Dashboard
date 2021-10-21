using System;

namespace Sisteg_Dashboard
{
    class Supplier
    {
        private Int32 idFornecedor;
        private string nomeFornecedor;
        private string enderecoFornecedor;
        private string numeroResidencia;
        private string bairroFornecedor;
        private string cidadeFornecedor;
        private string estadoFornecedor;
        private string emailFornecedor;

        public Supplier()
        {
            this.idFornecedor = 0;
            this.nomeFornecedor = null;
            this.enderecoFornecedor = null;
            this.numeroResidencia = null;
            this.bairroFornecedor = null;
            this.cidadeFornecedor = null;
            this.estadoFornecedor = null;
            this.emailFornecedor = null;
        }

        public Int32 IdFornecedor
        {
            get { return idFornecedor; }
            set { this.idFornecedor = value; }
        }

        public string NomeFornecedor
        {
            get { return nomeFornecedor; }
            set { this.nomeFornecedor = null; }
        }

        public string EnderecoFornecedor
        {
            get { return enderecoFornecedor; }
            set { this.enderecoFornecedor = value; }
        }

        public string NumeroResidencia
        {
            get { return numeroResidencia; }
            set { this.numeroResidencia = value; }
        }

        public string BairroFornecedor
        {
            get { return bairroFornecedor; }
            set { this.bairroFornecedor = value; }
        }

        public string CidadeFornecedor
        {
            get { return cidadeFornecedor; }
            set { this.cidadeFornecedor = value; }
        }

        public string EstadoFornecedor
        {
            get { return estadoFornecedor; }
            set { this.estadoFornecedor = value; }
        }

        public string EmailFornecedor
        {
            get { return emailFornecedor; }
            set { this.emailFornecedor = value; }
        }
    }
}
