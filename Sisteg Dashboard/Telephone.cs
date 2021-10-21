using System;

namespace Sisteg_Dashboard
{
    class Telephone
    {
        private Int32 idTelefone;
        private Int32 idCliente;
        private Int32 idFornecedor;
        private string tipoTelefone;
        private string numeroTelefone;

        public Telephone()
        {
            this.idTelefone = 0;
            this.idCliente = 0;
            this.idFornecedor = 0;
            this.tipoTelefone = null;
            this.numeroTelefone = null;
        }

        public Int32 IdTelefone
        {
            get { return idTelefone; }
            set { this.idTelefone = value; }
        }

        public Int32 IdCliente
        {
            get { return idCliente; }
            set { this.idCliente = value; }
        }

        public Int32 IdFornecedor
        {
            get { return idFornecedor; }
            set { this.idFornecedor = value; }
        }

        public string TipoTelefone
        {
            get { return tipoTelefone; }
            set { this.tipoTelefone = value; }
        }

        public string NumeroTelefone
        {
            get { return numeroTelefone; }
            set { this.numeroTelefone = value; }
        }
    }
}
