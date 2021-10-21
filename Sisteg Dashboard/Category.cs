using System;

namespace Sisteg_Dashboard
{
    class Category
    {
        private Int32 idCategoria;
        private Boolean categoriaReceita;
        private Boolean categoriaDespesa;
        private Boolean categoriaProduto;
        private string nomeCategoria;

        public Category()
        {
            this.idCategoria = 0;
            this.categoriaReceita = false;
            this.categoriaDespesa = false;
            this.categoriaProduto = false;
            this.nomeCategoria = null;
        }

        public Int32 IdCategoria
        {
            get { return idCategoria; }
            set { this.idCategoria = value; }
        }

        public Boolean CategoriaReceita
        {
            get { return categoriaReceita; }
            set { this.categoriaReceita = value; }
        }

        public Boolean CategoriaDespesa
        {
            get { return categoriaDespesa; }
            set { this.categoriaDespesa = value; }
        }

        public Boolean CategoriaProduto
        {
            get { return categoriaProduto; }
            set { this.categoriaProduto = value; }
        }

        public string NomeCategoria
        {
            get { return nomeCategoria; }
            set { this.nomeCategoria = value; }
        }
    }
}
