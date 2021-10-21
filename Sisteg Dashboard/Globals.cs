using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace Sisteg_Dashboard
{
    class Globals
    {
        //Structs
        public static Color[] myPalette = new Color[6]{
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
            Color.FromArgb(0, 104, 232),
            Color.FromKnownColor(KnownColor.Transparent),
        };

        //Int
        public static int step = 0, month = 0, idReceita = 0, idDespesa = 0, idConta = 0, idCategoria = 0, idCliente = 0, numeroOrcamento = 0, idProduto = 0, idFornecedor = 0;
        
        //Decimal
        public static decimal saldoConta = 0;

        //String
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

        //Objects
        public static IncomeGraphs incomeGraphs;
        public static Transactions transactions;
        public static ExpenseGraphs expenseGraphs;
        public static Main main;
        public static ClientStep clientStep;
        public static BudgetStep budgetStep;
        public static ProductStep productStep;
        public static BudgetForm budgetForm;
        public static ConfigForm configForm;
        public static AccountSettings accountSettings;
        public static CategorySettings categorySettings;

        //DataTables
        public static DataTable incomeDataTable;
        public static DataTable expenseDataTable;
        public static DataTable clientDataTable;
        public static DataTable telephoneDataTable;
        public static DataTable productDataTable;
        public static DataTable supplierDataTable;
        public static DataTable clientStepDataTable;
        public static DataTable budgetStepDataTable;
        public static DataTable productStepDataTable;
        public static DataTable budgetedProductDataTable;

        //Lists
        public static List<Repeat> repeats = new List<Repeat>();
        public static List<Parcel> parcels = new List<Parcel>();
    }
}
