using System;

namespace Sisteg_Dashboard
{
    class Account
    {
        private Int32 idConta;
        private Decimal saldoConta;
        private string nomeConta;
        private string tipoConta;
        private Boolean somarTotal;
        private Boolean contaAtiva;

        public Account()
        {
            this.idConta = 0;
            this.saldoConta = 0;
            this.nomeConta = null;
            this.tipoConta = null;
            this.somarTotal = false;
            this.contaAtiva = false;
        }

        public Int32 IdConta
        {
            get { return idConta; }
            set { this.idConta = value; }
        }

        public Decimal SaldoConta
        {
            get { return saldoConta; }
            set { this.saldoConta = value; }
        }

        public string NomeConta
        {
            get { return nomeConta; }
            set { this.nomeConta = value; }
        }

        public string TipoConta
        {
            get { return tipoConta; }
            set { this.tipoConta = value; }
        }

        public Boolean SomarTotal
        {
            get { return somarTotal; }
            set { this.somarTotal = value; }
        }

        public Boolean ContaAtiva
        {
            get { return contaAtiva; }
            set { this.contaAtiva = value; }
        }

    }
}
