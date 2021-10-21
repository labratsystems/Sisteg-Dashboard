using System;

namespace Sisteg_Dashboard
{
    class Client
    {
        private Int32 idCliente;
        private string nomeCliente;
        private string enderecoCliente;
        private string numeroResidencia;
        private string bairroCliente;
        private string cidadeCliente;
        private string estadoCliente;
        private string emailCliente;

        public Client()
        {
            this.idCliente = 0;
            this.nomeCliente = null;
            this.enderecoCliente = null;
            this.numeroResidencia = null;
            this.bairroCliente = null;
            this.cidadeCliente = null;
            this.estadoCliente = null;
            this.emailCliente = null;
        }

        public Int32 IdCliente
        {
            get { return idCliente; }
            set { this.idCliente = value;  }
        }

        public string NomeCliente
        {
            get { return nomeCliente; }
            set { this.nomeCliente = value; }
        }

        public string EnderecoCliente
        {
            get { return enderecoCliente; }
            set { this.enderecoCliente = value; }
        }

        public string NumeroResidencia
        {
            get { return numeroResidencia; }
            set { this.numeroResidencia = value; }
        }

        public string BairroCliente
        {
            get { return bairroCliente; }
            set { this.bairroCliente = value; }
        }

        public string CidadeCliente
        {
            get { return cidadeCliente; }
            set { this.cidadeCliente = value; }
        }

        public string EstadoCliente
        {
            get { return estadoCliente; }
            set { this.estadoCliente = value; }
        }

        public string EmailCliente
        {
            get { return emailCliente; }
            set { this.emailCliente = value; }
        }
    }
}
