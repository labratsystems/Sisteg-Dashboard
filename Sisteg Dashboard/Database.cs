using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace Sisteg_Dashboard
{
    class Database
    {
        private static SQLiteConnection connection;
        public static string path = System.Environment.CurrentDirectory;
        public static string databaseName = "sistegDatabase.db";
        public static string databasePath = path + @"\database\";

        //INICIA CONEXÃO COM BASE NA LOCALIZAÇÃO DO BANCO DE DADOS INTERNO DA APLICAÇÃO
        private static SQLiteConnection databaseConnection()
        {
            connection = new SQLiteConnection("Data Source="+databasePath+databaseName);
            connection.Open();
            return connection;
        }

        //FUNÇÃO QUE EXECUTA UMA CONSULTA QUALQUER
        public static DataTable query(string query)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = query;
                dataAdapter = new SQLiteDataAdapter(command.CommandText, databaseConnection());
                dataAdapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
            catch(Exception exception) { throw exception; }
        }

        //RECEITA

            //Cadastrar receita
            public static Boolean newIncome(Income income)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    DateTime dateTime = DateTime.Parse(income.dataTransacao.ToString());
                    string formatForSQLite = dateTime.ToString("yyyy-MM-dd");
                    command.CommandText = "INSERT INTO receita (idConta, numeroOrcamento, valorReceita, descricaoReceita, dataTransacao, categoriaReceita, observacoesReceita, recebimentoConfirmado, repetirParcelarReceita, valorFixoReceita, repeticoesValorFixoReceita, parcelarValorReceita, parcelasReceita, periodoRepetirParcelarReceita) VALUES (@idConta, @numeroOrcamento, @valorReceita, @descricaoReceita, @dataTransacao, @categoriaReceita, @observacoesReceita, @recebimentoConfirmado, @repetirParcelarReceita, @valorFixoReceita, @repeticoesValorFixoReceita, @parcelarValorReceita, @parcelasReceita, @periodoRepetirParcelarReceita)";
                    command.Parameters.AddWithValue("@idConta", income.idConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", income.numeroOrcamento);
                    command.Parameters.AddWithValue("@valorReceita", income.valorReceita);
                    command.Parameters.AddWithValue("@descricaoReceita", income.descricaoReceita);
                    command.Parameters.AddWithValue("@dataTransacao", formatForSQLite);
                    command.Parameters.AddWithValue("@categoriaReceita", income.categoriaReceita);
                    command.Parameters.AddWithValue("@observacoesReceita", income.observacoesReceita);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", income.recebimentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarReceita", income.repetirParcelarReceita);
                    command.Parameters.AddWithValue("@valorFixoReceita", income.valorFixoReceita);
                    command.Parameters.AddWithValue("@repeticoesValorFixoReceita", income.repeticoesValorFixoReceita);
                    command.Parameters.AddWithValue("@parcelarValorReceita", income.parcelarValorReceita);
                    command.Parameters.AddWithValue("@parcelasReceita", income.parcelasReceita);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarReceita", income.periodoRepetirParcelarReceita);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }catch(Exception exception) { throw exception; }
            }

            //Atualizar receita
            public static Boolean updateIncome(Income income)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET idConta = @idConta, numeroOrcamento = @numeroOrcamento, valorReceita = @valorReceita, descricaoReceita = @descricaoReceita, dataTransacao = @dataTransacao, categoriaReceita = @categoriaReceita, observacoesReceita = @observacoesReceita, recebimentoConfirmado = @recebimentoConfirmado, repetirParcelarReceita = @repetirParcelarReceita, valorFixoReceita = @valorFixoReceita, repeticoesValorFixoReceita = @repeticoesValorFixoReceita, parcelarValorReceita = @parcelarValorReceita, parcelasReceita = @parcelasReceita, periodoRepetirParcelarReceita = @periodoRepetirParcelarReceita WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idConta", income.idConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", income.numeroOrcamento);
                    command.Parameters.AddWithValue("@valorReceita", income.valorReceita);
                    command.Parameters.AddWithValue("@descricaoReceita", income.descricaoReceita);
                    command.Parameters.AddWithValue("@dataTransacao", income.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaReceita", income.categoriaReceita);
                    command.Parameters.AddWithValue("@observacoesReceita", income.observacoesReceita);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", income.recebimentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarReceita", income.repetirParcelarReceita);
                    command.Parameters.AddWithValue("@valorFixoReceita", income.valorFixoReceita);
                    command.Parameters.AddWithValue("@repeticoesValorFixoReceita", income.repeticoesValorFixoReceita);
                    command.Parameters.AddWithValue("@parcelarValorReceita", income.parcelarValorReceita);
                    command.Parameters.AddWithValue("@parcelasReceita", income.parcelasReceita);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarReceita", income.periodoRepetirParcelarReceita);
                    command.Parameters.AddWithValue("@idReceita", income.idReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Atualizar valor total da receita
            public static Boolean updateIncomeTotalValue(BudgetedProduct budgetedProduct, decimal valorReceita)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET valorReceita = @valorReceita WHERE numeroOrcamento = @numeroOrcamento;";
                    command.Parameters.AddWithValue("@valorReceita", valorReceita);
                    command.Parameters.AddWithValue("@numeroOrcamento", budgetedProduct.numeroOrcamento);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Atualizar receita não parcelada ou repetida
            public static Boolean updateIncomeNotParceledOrRepeated(Income income)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET idConta = @idConta, numeroOrcamento = @numeroOrcamento, valorReceita = @valorReceita, descricaoReceita = @descricaoReceita, dataTransacao = @dataTransacao, categoriaReceita = @categoriaReceita, observacoesReceita = @observacoesReceita, recebimentoConfirmado = @recebimentoConfirmado, repetirParcelarReceita = @repetirParcelarReceita, valorFixoReceita = @valorFixoReceita, repeticoesValorFixoReceita = @repeticoesValorFixoReceita, parcelarValorReceita = @parcelarValorReceita, parcelasReceita = @parcelasReceita, periodoRepetirParcelarReceita = @periodoRepetirParcelarReceita WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idConta", income.idConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", income.numeroOrcamento);
                    command.Parameters.AddWithValue("@valorReceita", income.valorReceita);
                    command.Parameters.AddWithValue("@descricaoReceita", income.descricaoReceita);
                    command.Parameters.AddWithValue("@dataTransacao", income.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaReceita", income.categoriaReceita);
                    command.Parameters.AddWithValue("@observacoesReceita", income.observacoesReceita);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", income.recebimentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarReceita", false);
                    command.Parameters.AddWithValue("@valorFixoReceita", null);
                    command.Parameters.AddWithValue("@repeticoesValorFixoReceita", null);
                    command.Parameters.AddWithValue("@parcelarValorReceita", null);
                    command.Parameters.AddWithValue("@parcelasReceita", null);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarReceita", null);
                    command.Parameters.AddWithValue("@idReceita", income.idReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir receita
            public static Boolean deleteIncome(Income income)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM receita WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idReceita", income.idReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Pagar parcela
            public static Boolean payIncome(Income income)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET recebimentoConfirmado = true WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idReceita", income.idReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todas as receitas
            public static Boolean deleteAllIncomes(Income income)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM receita WHERE numeroOrcamento = @numeroOrcamento;";
                    command.Parameters.AddWithValue("@numeroorcamento", income.numeroOrcamento);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

        //DESPESA

            //Cadastrar despesa
            public static Boolean newExpense(Expense expense)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO despesa (idConta, numeroOrcamento, valorDespesa, descricaoDespesa, dataTransacao, categoriaDespesa, observacoesDespesa, pagamentoConfirmado, repetirParcelarDespesa, valorFixoDespesa, repeticoesValorFixoDespesa, parcelarValorDespesa, parcelasDespesa, periodoRepetirParcelarDespesa) VALUES (@idConta, @numeroOrcamento, @valorDespesa, @descricaoDespesa, @dataTransacao, @categoriaDespesa, @observacoesDespesa, @pagamentoConfirmado, @repetirParcelarDespesa, @valorFixoDespesa, @repeticoesValorFixoDespesa, @parcelarValorDespesa, @parcelasDespesa, @periodoRepetirParcelarDespesa)";
                    command.Parameters.AddWithValue("@idConta", expense.idConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", expense.numeroOrcamento);
                    command.Parameters.AddWithValue("@valorDespesa", expense.valorDespesa);
                    command.Parameters.AddWithValue("@descricaoDespesa", expense.descricaoDespesa);
                    command.Parameters.AddWithValue("@dataTransacao", expense.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaDespesa", expense.categoriaDespesa);
                    command.Parameters.AddWithValue("@observacoesDespesa", expense.observacoesDespesa);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", expense.pagamentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarDespesa", expense.repetirParcelarDespesa);
                    command.Parameters.AddWithValue("@valorFixoDespesa", expense.valorFixoDespesa);
                    command.Parameters.AddWithValue("@repeticoesValorFixoDespesa", expense.repeticoesValorFixoDespesa);
                    command.Parameters.AddWithValue("@parcelarValorDespesa", expense.parcelarValorDespesa);
                    command.Parameters.AddWithValue("@parcelasDespesa", expense.parcelasDespesa);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarDespesa", expense.periodoRepetirParcelarDespesa);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Atualizar despesa
            public static Boolean updateExpense(Expense expense)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE despesa SET idConta = @idConta, numeroOrcamento = @numeroOrcamento, valorDespesa = @valorDespesa, descricaoDespesa = @descricaoDespesa, dataTransacao = @dataTransacao, categoriaDespesa = @categoriaDespesa, observacoesDespesa = @observacoesDespesa, pagamentoConfirmado = @pagamentoConfirmado, repetirParcelarDespesa = @repetirParcelarDespesa, valorFixoDespesa = @valorFixoDespesa, repeticoesValorFixoDespesa = @repeticoesValorFixoDespesa, parcelarValorDespesa = @parcelarValorDespesa, parcelasDespesa = @parcelasDespesa, periodoRepetirParcelarDespesa = @periodoRepetirParcelarDespesa WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idConta", expense.idConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", expense.numeroOrcamento);
                    command.Parameters.AddWithValue("@valorDespesa", expense.valorDespesa);
                    command.Parameters.AddWithValue("@descricaoDespesa", expense.descricaoDespesa);
                    command.Parameters.AddWithValue("@dataTransacao", expense.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaDespesa", expense.categoriaDespesa);
                    command.Parameters.AddWithValue("@observacoesDespesa", expense.observacoesDespesa);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", expense.pagamentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarDespesa", expense.repetirParcelarDespesa);
                    command.Parameters.AddWithValue("@valorFixoDespesa", expense.valorFixoDespesa);
                    command.Parameters.AddWithValue("@repeticoesValorFixoDespesa", expense.repeticoesValorFixoDespesa);
                    command.Parameters.AddWithValue("@parcelarValorDespesa", expense.parcelarValorDespesa);
                    command.Parameters.AddWithValue("@parcelasDespesa", expense.parcelasDespesa);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarDespesa", expense.periodoRepetirParcelarDespesa);
                    command.Parameters.AddWithValue("@idDespesa", expense.idDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Atualizar despesa não parcelada ou repetida
            public static Boolean updateExpenseNotParceledOrRepeated(Expense expense)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE despesa SET idConta = @idConta, numeroOrcamento = @numeroOrcamento, valorDespesa = @valorDespesa, descricaoDespesa = @descricaoDespesa, dataTransacao = @dataTransacao, categoriaDespesa = @categoriaDespesa, observacoesDespesa = @observacoesDespesa, pagamentoConfirmado = @pagamentoConfirmado, repetirParcelarDespesa = @repetirParcelarDespesa, valorFixoDespesa = @valorFixoDespesa, repeticoesValorFixoDespesa = @repeticoesValorFixoDespesa, parcelarValorDespesa = @parcelarValorDespesa, parcelasDespesa = @parcelasDespesa, periodoRepetirParcelarDespesa = @periodoRepetirParcelarDespesa WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idConta", expense.idConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", expense.numeroOrcamento);
                    command.Parameters.AddWithValue("@valorDespesa", expense.valorDespesa);
                    command.Parameters.AddWithValue("@descricaoDespesa", expense.descricaoDespesa);
                    command.Parameters.AddWithValue("@dataTransacao", expense.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaDespesa", expense.categoriaDespesa);
                    command.Parameters.AddWithValue("@observacoesDespesa", expense.observacoesDespesa);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", expense.pagamentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarDespesa", false);
                    command.Parameters.AddWithValue("@valorFixoDespesa", null);
                    command.Parameters.AddWithValue("@repeticoesValorFixoDespesa", null);
                    command.Parameters.AddWithValue("@parcelarValorDespesa", null);
                    command.Parameters.AddWithValue("@parcelasDespesa", null);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarDespesa", null);
                    command.Parameters.AddWithValue("@idReceita", expense.idDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir despesa
            public static Boolean deleteExpense(Expense expense)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM despesa WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idDespesa", expense.idDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

        //REPETIÇÃO

            //Cadastrar repetição
            public static Boolean newRepeat(Repeat repeat)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO repeticao (idReceita, idDespesa, valorRepeticao, descricaoRepeticao, dataTransacao, categoriaRepeticao, observacoesRepeticao, recebimentoConfirmado, pagamentoConfirmado) VALUES (@idReceita, @idDespesa, @valorRepeticao, @descricaoRepeticao, @dataTransacao, @categoriaRepeticao, @observacoesRepeticao, @recebimentoConfirmado, @pagamentoConfirmado)";
                    command.Parameters.AddWithValue("@idReceita", repeat.idReceita);
                    command.Parameters.AddWithValue("@idDespesa", repeat.idDespesa);
                    command.Parameters.AddWithValue("@valorRepeticao", repeat.valorRepeticao);
                    command.Parameters.AddWithValue("@descricaoRepeticao", repeat.descricaoRepeticao);
                    command.Parameters.AddWithValue("@dataTransacao", repeat.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaRepeticao", repeat.categoriaRepeticao);
                    command.Parameters.AddWithValue("@observacoesRepeticao", repeat.observacoesRepeticao);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", repeat.recebimentoConfirmado);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", repeat.pagamentoConfirmado);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Atualizar repetição
            public static Boolean updateRepeat(Repeat repeat)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE repeticao SET valorRepeticao = @valorRepeticao, descricaoRepeticao = @descricaoRepeticao, dataTransacao = @dataTransacao, categoriaRepeticao = @categoriaRepeticao, observacoesRepeticao = @observacoesRepeticao, recebimentoConfirmado = @recebimentoConfirmado, pagamentoConfirmado = @pagamentoConfirmado WHERE idRepeticao = @idRepeticao;";
                    command.Parameters.AddWithValue("@valorRepeticao", repeat.valorRepeticao);
                    command.Parameters.AddWithValue("@descricaoRepeticao", repeat.descricaoRepeticao);
                    command.Parameters.AddWithValue("@dataTransacao", repeat.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaRepeticao", repeat.categoriaRepeticao);
                    command.Parameters.AddWithValue("@observacoesRepeticao", repeat.observacoesRepeticao);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", repeat.recebimentoConfirmado);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", repeat.pagamentoConfirmado);
                    command.Parameters.AddWithValue("@idRepeticao", repeat.idRepeticao);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir repetição
            public static Boolean deleteRepeat(Repeat repeat)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM repeticao WHERE idRepeticao = @idRepeticao;";
                    command.Parameters.AddWithValue("@idRepeticao", repeat.idRepeticao);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todas as repetições da receita
            public static Boolean deleteAllRepeats(Income income)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM repeticao WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idReceita", income.idReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todas as repetições da despesa
            public static Boolean deleteAllRepeats(Expense expense)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM repeticao WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idDespesa", expense.idDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

        //PARCELA

            //Cadastrar parcela
            public static Boolean newParcel(Parcel parcel)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO parcela (idReceita, idDespesa, valorParcela, descricaoParcela, dataTransacao, categoriaParcela, observacoesParcela, recebimentoConfirmado, pagamentoConfirmado) VALUES (@idReceita, @idDespesa, @valorParcela, @descricaoParcela, @dataTransacao, @categoriaParcela, @observacoesParcela, @recebimentoConfirmado, @pagamentoConfirmado)";
                    command.Parameters.AddWithValue("@idReceita", parcel.idReceita);
                    command.Parameters.AddWithValue("@idDespesa", parcel.idDespesa);
                    command.Parameters.AddWithValue("@valorParcela", parcel.valorParcela);
                    command.Parameters.AddWithValue("@descricaoParcela", parcel.descricaoParcela);
                    command.Parameters.AddWithValue("@dataTransacao", parcel.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaParcela", parcel.categoriaParcela);
                    command.Parameters.AddWithValue("@observacoesParcela", parcel.observacoesParcela);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", parcel.recebimentoConfirmado);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", parcel.pagamentoConfirmado);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

        //Atualizar parcela
        public static Boolean updateParcelValue(Parcel parcel)
        {
            SQLiteDataAdapter dataAdapter = null;
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = "UPDATE parcela SET valorParcela = @valorParcela WHERE idParcela = @idParcela;";
                command.Parameters.AddWithValue("@valorParcela", parcel.valorParcela);
                command.Parameters.AddWithValue("@idParcela", parcel.idParcela);
                dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception) { throw exception; }
        }

        //Atualizar parcela
        public static Boolean updateParcel(Parcel parcel)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE parcela SET valorParcela = @valorParcela, descricaoParcela = @descricaoParcela, dataTransacao = @dataTransacao, categoriaParcela = @categoriaParcela, observacoesParcela = @observacoesParcela, recebimentoConfirmado = @recebimentoConfirmado, pagamentoConfirmado = @pagamentoConfirmado WHERE idParcela = @idParcela;";
                    command.Parameters.AddWithValue("@valorParcela", parcel.valorParcela);
                    command.Parameters.AddWithValue("@descricaoParcela", parcel.descricaoParcela);
                    command.Parameters.AddWithValue("@dataTransacao", parcel.dataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@categoriaParcela", parcel.categoriaParcela);
                    command.Parameters.AddWithValue("@observacoesParcela", parcel.observacoesParcela);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", parcel.recebimentoConfirmado);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", parcel.pagamentoConfirmado);
                    command.Parameters.AddWithValue("@idParcela", parcel.idParcela);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir parcela
            public static Boolean deleteParcel(Parcel parcel)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM parcela WHERE idParcela = @idParcela;";
                    command.Parameters.AddWithValue("@idParcela", parcel.idParcela);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todas as parcelas da receita
            public static Boolean deleteAllParcels(Income income)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM parcela WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idReceita", income.idReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todas as parcelas da despesa
            public static Boolean deleteAllParcels(Expense expense)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM parcela WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idDespesa", expense.idDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Pagar parcela
            public static Boolean payParcel(Parcel parcel)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE parcela SET recebimentoConfirmado = true WHERE idParcela = @idParcela;";
                    command.Parameters.AddWithValue("@idParcela", parcel.idParcela);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

        //CLIENTE

        //Cadastrar cliente
        public static Boolean newClient(Client client)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO cliente (nomeCliente, enderecoCliente, numeroResidencia, bairroCliente, cidadeCliente, estadoCliente, emailCliente) VALUES (@nomeCliente, @enderecoCliente, @numeroResidencia, @bairroCliente, @cidadeCliente, @estadoCliente, @emailCliente)";
                    command.Parameters.AddWithValue("@nomeCliente", client.nomeCliente);
                    command.Parameters.AddWithValue("@enderecoCliente", client.enderecoCliente);
                    command.Parameters.AddWithValue("@numeroResidencia", client.numeroResidencia);
                    command.Parameters.AddWithValue("@bairroCliente", client.bairroCliente);
                    command.Parameters.AddWithValue("@cidadeCliente", client.cidadeCliente);
                    command.Parameters.AddWithValue("@estadoCliente", client.estadoCliente);
                    command.Parameters.AddWithValue("@emailCliente", client.emailCliente);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }


            //Atualizar cliente
            public static Boolean updateClient(Client client)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE cliente SET nomeCliente = @nomeCliente, enderecoCliente = @enderecoCliente, numeroResidencia = @numeroResidencia, bairroCliente = @bairroCliente, cidadeCliente = @cidadeCliente, estadoCliente = @estadoCliente, emailCliente = @emailCliente WHERE idCliente = @idCliente;";
                    command.Parameters.AddWithValue("@nomeCliente", client.nomeCliente);
                    command.Parameters.AddWithValue("@enderecoCliente", client.enderecoCliente);
                    command.Parameters.AddWithValue("@numeroResidencia", client.numeroResidencia);
                    command.Parameters.AddWithValue("@bairroCliente", client.bairroCliente);
                    command.Parameters.AddWithValue("@cidadeCliente", client.cidadeCliente);
                    command.Parameters.AddWithValue("@estadoCliente", client.estadoCliente);
                    command.Parameters.AddWithValue("@emailCliente", client.emailCliente);
                    command.Parameters.AddWithValue("@idCliente", client.idCliente);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir cliente
            public static Boolean deleteClient(Client client)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var commandClient = databaseConnection().CreateCommand();
                    commandClient.CommandText = "DELETE FROM cliente WHERE idCliente = @idCliente;";
                    commandClient.Parameters.AddWithValue("@idCliente", client.idCliente);
                    dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                    commandClient.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

        //TELEFONE

            //Cadastrar telefone
            public static Boolean newTelephone(Telephone telephone)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO telefone (idCliente, idFornecedor, tipoTelefone, numeroTelefone) VALUES (@idCliente, @idFornecedor, @tipoTelefone, @numeroTelefone)";
                    command.Parameters.AddWithValue("@idCliente", telephone.idCliente);
                    command.Parameters.AddWithValue("@idFornecedor", telephone.idFornecedor);
                    command.Parameters.AddWithValue("@tipoTelefone", telephone.tipoTelefone);
                    command.Parameters.AddWithValue("@numeroTelefone", telephone.numeroTelefone);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Atualizar telefone
            public static Boolean updateTelephone(Telephone telephone)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE telefone SET tipoTelefone = @tipoTelefone, numeroTelefone = @numeroTelefone WHERE idTelefone = @idTelefone;";
                    command.Parameters.AddWithValue("@tipoTelefone", telephone.tipoTelefone);
                    command.Parameters.AddWithValue("@numeroTelefone", telephone.numeroTelefone);
                    command.Parameters.AddWithValue("@idTelefone", telephone.idTelefone);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todas os telefones do cliente
            public static Boolean deleteTelephone(Telephone telephone)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM telefone WHERE idTelefone = @idTelefone;";
                    command.Parameters.AddWithValue("@idTelefone", telephone.idTelefone);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todas os telefones do cliente
            public static Boolean deleteAllTelephones(Client client)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM telefone WHERE idCliente = @idCliente;";
                    command.Parameters.AddWithValue("@idCliente", client.idCliente);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todas os telefones do fornecedor
            public static Boolean deleteAllTelephones(Supplier supplier)
            {
                SQLiteDataAdapter dataAdapter = null;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM telefone WHERE idFornecedor = @idFornecedor;";
                    command.Parameters.AddWithValue("@idFornecedor", supplier.idFornecedor);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

        //PRODUTO

            //Cadastrar produto
            public static Boolean newProduct(Product product)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO produto (idFornecedor, nomeProduto, categoriaProduto, valorUnitario) VALUES (@idFornecedor, @nomeProduto, @categoriaProduto, @valorUnitario)";
                    command.Parameters.AddWithValue("@idFornecedor", product.idFornecedor);
                    command.Parameters.AddWithValue("@nomeProduto", product.nomeProduto);
                    command.Parameters.AddWithValue("@categoriaProduto", product.categoriaProduto);
                    command.Parameters.AddWithValue("@valorUnitario", product.valorUnitario);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Atualizar produto
            public static Boolean updateProduct(Product product)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE produto SET idFornecedor = @idFornecedor, nomeProduto = @nomeProduto, categoriaProduto = @categoriaProduto, valorUnitario = @valorUnitario WHERE idProduto = @idProduto;";
                    command.Parameters.AddWithValue("@idFornecedor", product.idFornecedor);
                    command.Parameters.AddWithValue("@nomeProduto", product.nomeProduto);
                    command.Parameters.AddWithValue("@categoriaProduto", product.categoriaProduto);
                    command.Parameters.AddWithValue("@valorUnitario", product.valorUnitario);
                    command.Parameters.AddWithValue("@idProduto", product.idProduto);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir produto
            public static Boolean deleteProduct(Product product)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM produto WHERE idProduto = @idProduto;";
                    command.Parameters.AddWithValue("@idProduto", product.idProduto);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir todos os produtos
            public static Boolean deleteAllProducts(Supplier supplier)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM produto WHERE idFornecedor = @idFornecedor;";
                    command.Parameters.AddWithValue("@idFornecedor", supplier.idFornecedor);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

        //FORNECEDOR
            

            //Adicionar fornecedor
            public static Boolean newSupplier(Supplier supplier)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO fornecedor (nomeFornecedor, enderecoFornecedor, numeroResidencia, bairroFornecedor, cidadeFornecedor, estadoFornecedor, emailFornecedor) VALUES (@nomeFornecedor, @enderecoFornecedor, @numeroResidencia, @bairroFornecedor, @cidadeFornecedor, @estadoFornecedor, @emailFornecedor)";
                    command.Parameters.AddWithValue("@nomeFornecedor", supplier.nomeFornecedor);
                    command.Parameters.AddWithValue("@enderecoFornecedor", supplier.enderecoFornecedor);
                    command.Parameters.AddWithValue("@numeroResidencia", supplier.numeroResidencia);
                    command.Parameters.AddWithValue("@bairroFornecedor", supplier.bairroFornecedor);
                    command.Parameters.AddWithValue("@cidadeFornecedor", supplier.cidadeFornecedor);
                    command.Parameters.AddWithValue("@estadoFornecedor", supplier.estadoFornecedor);
                    command.Parameters.AddWithValue("@emailFornecedor", supplier.emailFornecedor);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }


            //Atualizar fornecedor
            public static Boolean updateSupplier(Supplier supplier)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE fornecedor SET nomeFornecedor = @nomeFornecedor, enderecoFornecedor = @enderecoFornecedor, numeroResidencia = @numeroResidencia, bairroFornecedor = @bairroFornecedor, cidadeFornecedor = @cidadeFornecedor, estadoFornecedor = @estadoFornecedor,  emailFornecedor = @emailFornecedor WHERE idFornecedor = @idFornecedor;";
                    command.Parameters.AddWithValue("@nomeFornecedor", supplier.nomeFornecedor);
                    command.Parameters.AddWithValue("@enderecoFornecedor", supplier.enderecoFornecedor);
                    command.Parameters.AddWithValue("@numeroResidencia", supplier.numeroResidencia);
                    command.Parameters.AddWithValue("@bairroFornecedor", supplier.bairroFornecedor);
                    command.Parameters.AddWithValue("@cidadeFornecedor", supplier.cidadeFornecedor);
                    command.Parameters.AddWithValue("@estadoFornecedor", supplier.estadoFornecedor);
                    command.Parameters.AddWithValue("@emailFornecedor", supplier.emailFornecedor);
                    command.Parameters.AddWithValue("@idFornecedor", supplier.idFornecedor);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { throw exception; }
            }

            //Excluir fornecedor
            public static Boolean deleteSupplier(Supplier supplier)
            {
                SQLiteDataAdapter dataAdapter = null;
                DataTable dataTable = new DataTable();
                try
                {
                    var connection = databaseConnection();
                    var commandClient = databaseConnection().CreateCommand();
                    commandClient.CommandText = "DELETE FROM fornecedor WHERE idFornecedor = @idFornecedor;";
                    commandClient.Parameters.AddWithValue("@idFornecedor", supplier.idFornecedor);
                    dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                    commandClient.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception)  { throw exception; }
            }

        //ORÇAMENTO

        //CADASTRAR ORÇAMENTO
        public static Boolean newBudget(Budget budget)
        {
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                DateTime dateTime = DateTime.Parse(budget.dataOrcamento.ToString());
                string formatForSQLite = dateTime.ToString("yyyy-MM-dd");
                command.CommandText = "INSERT INTO orcamento (idCliente, dataOrcamento, valorTrabalho, valorTotal, condicaoPagamento, orcamentoConfirmado) VALUES (@idCliente, @dataOrcamento, @valorTrabalho, @valorTotal, @condicaoPagamento, @orcamentoConfirmado)";
                command.Parameters.AddWithValue("@idCliente", budget.idCliente);
                command.Parameters.AddWithValue("@dataOrcamento", formatForSQLite);
                command.Parameters.AddWithValue("@valorTrabalho", budget.valorTrabalho);
                command.Parameters.AddWithValue("@valorTotal", budget.valorTotal);
                command.Parameters.AddWithValue("@condicaoPagamento", budget.condicaoPagamento);
                command.Parameters.AddWithValue("@orcamentoConfirmado", budget.orcamentoConfirmado);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //ATUALIZAR ORÇAMENTO
        public static Boolean updateBudget(Budget budget)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                DateTime dateTime = DateTime.Parse(budget.dataOrcamento.ToString());
                string formatForSQLite = dateTime.ToString("yyyy-MM-dd");
                command.CommandText = "UPDATE orcamento SET dataOrcamento = @dataOrcamento, valorTrabalho = @valorTrabalho, valorTotal = @valorTotal, condicaoPagamento = @condicaoPagamento, orcamentoConfirmado = @orcamentoConfirmado WHERE numeroOrcamento = @numeroOrcamento;";
                command.Parameters.AddWithValue("@dataOrcamento", formatForSQLite);
                command.Parameters.AddWithValue("@valorTrabalho", budget.valorTrabalho);
                command.Parameters.AddWithValue("@valorTotal", budget.valorTotal);
                command.Parameters.AddWithValue("@condicaoPagamento", budget.condicaoPagamento);
                command.Parameters.AddWithValue("@orcamentoConfirmado", budget.orcamentoConfirmado);
                command.Parameters.AddWithValue("@numeroOrcamento", budget.numeroOrcamento);
                dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //CONFIRMAR ORÇAMENTO
        public static Boolean confirmBudget(Budget budget)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = "UPDATE orcamento SET orcamentoConfirmado = @orcamentoConfirmado WHERE numeroOrcamento = @numeroOrcamento;";
                command.Parameters.AddWithValue("@orcamentoConfirmado", budget.orcamentoConfirmado);
                command.Parameters.AddWithValue("@numeroOrcamento", budget.numeroOrcamento);
                dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //ATUALIZAR VALOR TOTAL DO ORÇAMENTO
        public static Boolean updateBudgetTotalValue(BudgetedProduct budgetedProduct, decimal valorTotal)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = "UPDATE orcamento SET valorTotal = @valorTotal WHERE numeroOrcamento = @numeroOrcamento;";
                command.Parameters.AddWithValue("@valorTotal", valorTotal);
                command.Parameters.AddWithValue("@numeroOrcamento", budgetedProduct.numeroOrcamento);
                dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //EXCLUIR ORÇAMENTO
        public static Boolean deleteBudget(Budget budget)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var commandClient = databaseConnection().CreateCommand();
                commandClient.CommandText = "DELETE FROM orcamento WHERE numeroOrcamento = @numeroOrcamento;";
                commandClient.Parameters.AddWithValue("@numeroOrcamento", budget.numeroOrcamento);
                dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                commandClient.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //EXCLUIR TODOS OS ORÇAMENTOS
        public static Boolean deleteAllBudgets(Client client)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var commandClient = databaseConnection().CreateCommand();
                commandClient.CommandText = "DELETE FROM orcamento WHERE idCliente = @idCliente;";
                commandClient.Parameters.AddWithValue("@idCliente", client.idCliente);
                dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                commandClient.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //CADASTRAR PRODUTO ORÇADO
        public static Boolean newBudgetedProduct(BudgetedProduct budgetedProduct)
        {
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = "INSERT INTO produtoOrcado (idProduto, numeroOrcamento, item, quantidadeProduto, valorTotal) VALUES (@idProduto, @numeroOrcamento, @item, @quantidadeProduto, @valorTotal)";
                command.Parameters.AddWithValue("@idProduto", budgetedProduct.idProduto);
                command.Parameters.AddWithValue("@numeroOrcamento", budgetedProduct.numeroOrcamento);
                command.Parameters.AddWithValue("@item", budgetedProduct.item);
                command.Parameters.AddWithValue("@quantidadeProduto", budgetedProduct.quantidadeProduto);
                command.Parameters.AddWithValue("@valorTotal", budgetedProduct.valorTotal);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //ATUALIZAR PRODUTO ORÇADO
        public static Boolean updateBudgetedProduct(BudgetedProduct budgetedProduct)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = "UPDATE produtoOrcado SET idProduto = @idProduto, numeroOrcamento = @numeroOrcamento, item = @item, quantidadeProduto = @quantidadeProduto, valorTotal = @valorTotal WHERE idProdutoOrcado = @idProdutoOrcado;";
                command.Parameters.AddWithValue("@idProduto", budgetedProduct.idProduto);
                command.Parameters.AddWithValue("@numeroOrcamento", budgetedProduct.numeroOrcamento);
                command.Parameters.AddWithValue("@item", budgetedProduct.item);
                command.Parameters.AddWithValue("@quantidadeProduto", budgetedProduct.quantidadeProduto);
                command.Parameters.AddWithValue("@valorTotal", budgetedProduct.valorTotal);
                command.Parameters.AddWithValue("@idProdutoOrcado", budgetedProduct.idProdutoOrcado);
                dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        //EXCLUIR PRODUTO ORCADO
        public static Boolean deleteBudgetedProduct(BudgetedProduct budgetedProduct)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var commandClient = databaseConnection().CreateCommand();
                commandClient.CommandText = "DELETE FROM produtoOrcado WHERE idProdutoOrcado = @idProdutoOrcado;";
                commandClient.Parameters.AddWithValue("@idProdutoOrcado", budgetedProduct.idProdutoOrcado);
                dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                commandClient.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //Excluir todos os produtos orçados
        public static Boolean deleteAllBudgetedProducts(Product product)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = "DELETE FROM produtoOrcado WHERE idProduto = @idProduto;";
                command.Parameters.AddWithValue("@idProduto", product.idProduto);
                dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception) { throw exception; }
        }

        //Excluir todos os produtos orçados
        public static Boolean deleteAllBudgetedProducts(Budget budget)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = "DELETE FROM produtoOrcado WHERE numeroOrcamento = @numeroOrcamento;";
                command.Parameters.AddWithValue("@numeroOrcamento", budget.numeroOrcamento);
                dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //ATUALIZAR NÚMERO DO ITEM DOS PRODUTOS ORÇADOS
        public static Boolean updateBudgetedProductItemValue(BudgetedProduct budgetedProduct)
        {
            SQLiteDataAdapter dataAdapter = null;
            DataTable dataTable = new DataTable();
            try
            {
                var connection = databaseConnection();
                var commandClient = databaseConnection().CreateCommand();
                commandClient.CommandText = "UPDATE produtoOrcado SET item = @item WHERE idProdutoOrcado = @idProdutoOrcado;";
                commandClient.Parameters.AddWithValue("@item", budgetedProduct.item);
                commandClient.Parameters.AddWithValue("@idProdutoOrcado", budgetedProduct.idProdutoOrcado);
                dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                commandClient.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
