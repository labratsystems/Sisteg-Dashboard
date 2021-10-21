using System;
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
            try
            {
                connection = new SQLiteConnection("Data Source=" + databasePath + databaseName);
                connection.Open();
                return connection;
            }
            catch (Exception exception) { throw exception; }
        }

        //FUNÇÃO QUE EXECUTA UMA CONSULTA QUALQUER
        public static DataTable query(string query)
        {
            SQLiteDataAdapter dataAdapter;
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
                    DateTime dateTime = DateTime.Parse(income.DataTransacao.ToString());
                    string formatForSQLite = dateTime.ToString("yyyy-MM-dd");
                    command.CommandText = "INSERT INTO receita (idConta, numeroOrcamento, idCategoria, valorReceita, descricaoReceita, dataTransacao, observacoesReceita, recebimentoConfirmado, repetirParcelarReceita, valorFixoReceita, repeticoesValorFixoReceita, parcelarValorReceita, parcelasReceita, periodoRepetirParcelarReceita) VALUES (@idConta, @numeroOrcamento, @idCategoria, @valorReceita, @descricaoReceita, @dataTransacao, @observacoesReceita, @recebimentoConfirmado, @repetirParcelarReceita, @valorFixoReceita, @repeticoesValorFixoReceita, @parcelarValorReceita, @parcelasReceita, @periodoRepetirParcelarReceita)";
                    command.Parameters.AddWithValue("@idConta", income.IdConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", income.NumeroOrcamento);
                    command.Parameters.AddWithValue("@idCategoria", income.IdCategoria);
                    command.Parameters.AddWithValue("@valorReceita", income.ValorReceita);
                    command.Parameters.AddWithValue("@descricaoReceita", income.DescricaoReceita);
                    command.Parameters.AddWithValue("@dataTransacao", formatForSQLite);
                    command.Parameters.AddWithValue("@observacoesReceita", income.ObservacoesReceita);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", income.RecebimentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarReceita", income.RepetirParcelarReceita);
                    command.Parameters.AddWithValue("@valorFixoReceita", income.ValorFixoReceita);
                    command.Parameters.AddWithValue("@repeticoesValorFixoReceita", income.RepeticoesValorFixoReceita);
                    command.Parameters.AddWithValue("@parcelarValorReceita", income.ParcelarValorReceita);
                    command.Parameters.AddWithValue("@parcelasReceita", income.ParcelasReceita);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarReceita", income.PeriodoRepetirParcelarReceita);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }catch(Exception exception) { return false; }
            }

            //Atualizar receita
            public static Boolean updateIncome(Income income)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET idConta = @idConta, numeroOrcamento = @numeroOrcamento, idCategoria = @idCategoria, valorReceita = @valorReceita, descricaoReceita = @descricaoReceita, dataTransacao = @dataTransacao, observacoesReceita = @observacoesReceita, recebimentoConfirmado = @recebimentoConfirmado, repetirParcelarReceita = @repetirParcelarReceita, valorFixoReceita = @valorFixoReceita, repeticoesValorFixoReceita = @repeticoesValorFixoReceita, parcelarValorReceita = @parcelarValorReceita, parcelasReceita = @parcelasReceita, periodoRepetirParcelarReceita = @periodoRepetirParcelarReceita WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idConta", income.IdConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", income.NumeroOrcamento);
                    command.Parameters.AddWithValue("@idCategoria", income.IdCategoria);
                    command.Parameters.AddWithValue("@valorReceita", income.ValorReceita);
                    command.Parameters.AddWithValue("@descricaoReceita", income.DescricaoReceita);
                    command.Parameters.AddWithValue("@dataTransacao", income.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesReceita", income.ObservacoesReceita);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", income.RecebimentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarReceita", income.RepetirParcelarReceita);
                    command.Parameters.AddWithValue("@valorFixoReceita", income.ValorFixoReceita);
                    command.Parameters.AddWithValue("@repeticoesValorFixoReceita", income.RepeticoesValorFixoReceita);
                    command.Parameters.AddWithValue("@parcelarValorReceita", income.ParcelarValorReceita);
                    command.Parameters.AddWithValue("@parcelasReceita", income.ParcelasReceita);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarReceita", income.PeriodoRepetirParcelarReceita);
                    command.Parameters.AddWithValue("@idReceita", income.IdReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
        }

            //Atualizar valor total da receita
            public static Boolean updateIncomeTotalValue(BudgetedProduct budgetedProduct, decimal valorReceita)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET valorReceita = @valorReceita WHERE numeroOrcamento = @numeroOrcamento;";
                    command.Parameters.AddWithValue("@valorReceita", valorReceita);
                    command.Parameters.AddWithValue("@numeroOrcamento", budgetedProduct.NumeroOrcamento);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
        }

            //Atualizar receita não parcelada ou repetida
            public static Boolean updateIncomeNotParceledOrRepeated(Income income)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET idConta = @idConta, numeroOrcamento = @numeroOrcamento, idCategoria = @idCategoria, valorReceita = @valorReceita, descricaoReceita = @descricaoReceita, dataTransacao = @dataTransacao, observacoesReceita = @observacoesReceita, recebimentoConfirmado = @recebimentoConfirmado, repetirParcelarReceita = @repetirParcelarReceita, valorFixoReceita = @valorFixoReceita, repeticoesValorFixoReceita = @repeticoesValorFixoReceita, parcelarValorReceita = @parcelarValorReceita, parcelasReceita = @parcelasReceita, periodoRepetirParcelarReceita = @periodoRepetirParcelarReceita WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idConta", income.IdConta);
                    command.Parameters.AddWithValue("@numeroOrcamento", income.NumeroOrcamento);
                    command.Parameters.AddWithValue("@idCategoria", income.IdCategoria);
                    command.Parameters.AddWithValue("@valorReceita", income.ValorReceita);
                    command.Parameters.AddWithValue("@descricaoReceita", income.DescricaoReceita);
                    command.Parameters.AddWithValue("@dataTransacao", income.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesReceita", income.ObservacoesReceita);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", income.RecebimentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarReceita", false);
                    command.Parameters.AddWithValue("@valorFixoReceita", null);
                    command.Parameters.AddWithValue("@repeticoesValorFixoReceita", null);
                    command.Parameters.AddWithValue("@parcelarValorReceita", null);
                    command.Parameters.AddWithValue("@parcelasReceita", null);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarReceita", null);
                    command.Parameters.AddWithValue("@idReceita", income.IdReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir receita
            public static Boolean deleteIncome(Income income)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM receita WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idReceita", income.IdReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Pagar parcela
            public static Boolean payIncome(Income income)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET recebimentoConfirmado = true WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idReceita", income.IdReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //DESPESA

            //Cadastrar despesa
            public static Boolean newExpense(Expense expense)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO despesa (idConta, idCategoria, valorDespesa, descricaoDespesa, dataTransacao, observacoesDespesa, pagamentoConfirmado, repetirParcelarDespesa, valorFixoDespesa, repeticoesValorFixoDespesa, parcelarValorDespesa, parcelasDespesa, periodoRepetirParcelarDespesa) VALUES (@idConta, @idCategoria, @valorDespesa, @descricaoDespesa, @dataTransacao, @observacoesDespesa, @pagamentoConfirmado, @repetirParcelarDespesa, @valorFixoDespesa, @repeticoesValorFixoDespesa, @parcelarValorDespesa, @parcelasDespesa, @periodoRepetirParcelarDespesa)";
                    command.Parameters.AddWithValue("@idConta", expense.IdConta);
                    command.Parameters.AddWithValue("@idCategoria", expense.IdCategoria);
                    command.Parameters.AddWithValue("@valorDespesa", expense.ValorDespesa);
                    command.Parameters.AddWithValue("@descricaoDespesa", expense.DescricaoDespesa);
                    command.Parameters.AddWithValue("@dataTransacao", expense.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesDespesa", expense.ObservacoesDespesa);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", expense.PagamentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarDespesa", expense.RepetirParcelarDespesa);
                    command.Parameters.AddWithValue("@valorFixoDespesa", expense.ValorFixoDespesa);
                    command.Parameters.AddWithValue("@repeticoesValorFixoDespesa", expense.RepeticoesValorFixoDespesa);
                    command.Parameters.AddWithValue("@parcelarValorDespesa", expense.ParcelarValorDespesa);
                    command.Parameters.AddWithValue("@parcelasDespesa", expense.ParcelasDespesa);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarDespesa", expense.PeriodoRepetirParcelarDespesa);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar despesa
            public static Boolean updateExpense(Expense expense)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE despesa SET idConta = @idConta, idCategoria = @idCategoria, valorDespesa = @valorDespesa, descricaoDespesa = @descricaoDespesa, dataTransacao = @dataTransacao, observacoesDespesa = @observacoesDespesa, pagamentoConfirmado = @pagamentoConfirmado, repetirParcelarDespesa = @repetirParcelarDespesa, valorFixoDespesa = @valorFixoDespesa, repeticoesValorFixoDespesa = @repeticoesValorFixoDespesa, parcelarValorDespesa = @parcelarValorDespesa, parcelasDespesa = @parcelasDespesa, periodoRepetirParcelarDespesa = @periodoRepetirParcelarDespesa WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idConta", expense.IdConta);
                    command.Parameters.AddWithValue("@idCategoria", expense.IdCategoria);
                    command.Parameters.AddWithValue("@valorDespesa", expense.ValorDespesa);
                    command.Parameters.AddWithValue("@descricaoDespesa", expense.DescricaoDespesa);
                    command.Parameters.AddWithValue("@dataTransacao", expense.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesDespesa", expense.ObservacoesDespesa);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", expense.PagamentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarDespesa", expense.RepetirParcelarDespesa);
                    command.Parameters.AddWithValue("@valorFixoDespesa", expense.ValorFixoDespesa);
                    command.Parameters.AddWithValue("@repeticoesValorFixoDespesa", expense.RepeticoesValorFixoDespesa);
                    command.Parameters.AddWithValue("@parcelarValorDespesa", expense.ParcelarValorDespesa);
                    command.Parameters.AddWithValue("@parcelasDespesa", expense.ParcelasDespesa);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarDespesa", expense.PeriodoRepetirParcelarDespesa);
                    command.Parameters.AddWithValue("@idDespesa", expense.IdDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar despesa não parcelada ou repetida
            public static Boolean updateExpenseNotParceledOrRepeated(Expense expense)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE despesa SET idConta = @idConta, idCategoria = @idCategoria, valorDespesa = @valorDespesa, descricaoDespesa = @descricaoDespesa, dataTransacao = @dataTransacao, observacoesDespesa = @observacoesDespesa, pagamentoConfirmado = @pagamentoConfirmado, repetirParcelarDespesa = @repetirParcelarDespesa, valorFixoDespesa = @valorFixoDespesa, repeticoesValorFixoDespesa = @repeticoesValorFixoDespesa, parcelarValorDespesa = @parcelarValorDespesa, parcelasDespesa = @parcelasDespesa, periodoRepetirParcelarDespesa = @periodoRepetirParcelarDespesa WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idConta", expense.IdConta);
                    command.Parameters.AddWithValue("@idCategoria", expense.IdCategoria);
                    command.Parameters.AddWithValue("@valorDespesa", expense.ValorDespesa);
                    command.Parameters.AddWithValue("@descricaoDespesa", expense.DescricaoDespesa);
                    command.Parameters.AddWithValue("@dataTransacao", expense.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesDespesa", expense.ObservacoesDespesa);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", expense.PagamentoConfirmado);
                    command.Parameters.AddWithValue("@repetirParcelarDespesa", false);
                    command.Parameters.AddWithValue("@valorFixoDespesa", null);
                    command.Parameters.AddWithValue("@repeticoesValorFixoDespesa", null);
                    command.Parameters.AddWithValue("@parcelarValorDespesa", null);
                    command.Parameters.AddWithValue("@parcelasDespesa", null);
                    command.Parameters.AddWithValue("@periodoRepetirParcelarDespesa", null);
                    command.Parameters.AddWithValue("@idReceita", expense.IdDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir despesa
            public static Boolean deleteExpense(Expense expense)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM despesa WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idDespesa", expense.IdDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //REPETIÇÃO

            //Cadastrar repetição
            public static Boolean newRepeat(Repeat repeat)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO repeticao (idReceita, idDespesa, idConta, idCategoria, valorRepeticao, descricaoRepeticao, dataTransacao, observacoesRepeticao, recebimentoConfirmado, pagamentoConfirmado) VALUES (@idReceita, @idDespesa, @idConta, @idCategoria, @valorRepeticao, @descricaoRepeticao, @dataTransacao, @observacoesRepeticao, @recebimentoConfirmado, @pagamentoConfirmado)";
                    command.Parameters.AddWithValue("@idReceita", repeat.IdReceita);
                    command.Parameters.AddWithValue("@idDespesa", repeat.IdDespesa);
                    command.Parameters.AddWithValue("@idConta", repeat.IdConta);
                    command.Parameters.AddWithValue("@idCategoria", repeat.IdCategoria);
                    command.Parameters.AddWithValue("@valorRepeticao", repeat.ValorRepeticao);
                    command.Parameters.AddWithValue("@descricaoRepeticao", repeat.DescricaoRepeticao);
                    command.Parameters.AddWithValue("@dataTransacao", repeat.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesRepeticao", repeat.ObservacoesRepeticao);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", repeat.RecebimentoConfirmado);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", repeat.PagamentoConfirmado);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar repetição
            public static Boolean updateRepeat(Repeat repeat)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE repeticao SET idConta = @idConta, idCategoria = @idCategoria, valorRepeticao = @valorRepeticao, descricaoRepeticao = @descricaoRepeticao, dataTransacao = @dataTransacao, observacoesRepeticao = @observacoesRepeticao, recebimentoConfirmado = @recebimentoConfirmado, pagamentoConfirmado = @pagamentoConfirmado WHERE idRepeticao = @idRepeticao;";
                    command.Parameters.AddWithValue("@idConta", repeat.IdConta);
                    command.Parameters.AddWithValue("@idCategoria", repeat.IdCategoria);
                    command.Parameters.AddWithValue("@valorRepeticao", repeat.ValorRepeticao);
                    command.Parameters.AddWithValue("@descricaoRepeticao", repeat.DescricaoRepeticao);
                    command.Parameters.AddWithValue("@dataTransacao", repeat.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesRepeticao", repeat.ObservacoesRepeticao);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", repeat.RecebimentoConfirmado);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", repeat.PagamentoConfirmado);
                    command.Parameters.AddWithValue("@idRepeticao", repeat.IdRepeticao);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir repetição
            public static Boolean deleteRepeat(Repeat repeat)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM repeticao WHERE idRepeticao = @idRepeticao;";
                    command.Parameters.AddWithValue("@idRepeticao", repeat.IdRepeticao);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todas as repetições da receita
            public static Boolean deleteAllRepeats(Income income)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM repeticao WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idReceita", income.IdReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todas as repetições da despesa
            public static Boolean deleteAllRepeats(Expense expense)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM repeticao WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idDespesa", expense.IdDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //PARCELA

            //Cadastrar parcela
            public static Boolean newParcel(Parcel parcel)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO parcela (idReceita, idDespesa, idConta, idCategoria, valorParcela, descricaoParcela, dataTransacao, observacoesParcela, recebimentoConfirmado, pagamentoConfirmado) VALUES (@idReceita, @idDespesa, @idConta, @idCategoria, @valorParcela, @descricaoParcela, @dataTransacao, @observacoesParcela, @recebimentoConfirmado, @pagamentoConfirmado)";
                    command.Parameters.AddWithValue("@idReceita", parcel.IdReceita);
                    command.Parameters.AddWithValue("@idDespesa", parcel.IdDespesa);
                    command.Parameters.AddWithValue("@idConta", parcel.IdConta);
                    command.Parameters.AddWithValue("@idCategoria", parcel.IdCategoria);
                    command.Parameters.AddWithValue("@valorParcela", parcel.ValorParcela);
                    command.Parameters.AddWithValue("@descricaoParcela", parcel.DescricaoParcela);
                    command.Parameters.AddWithValue("@dataTransacao", parcel.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesParcela", parcel.ObservacoesParcela);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", parcel.RecebimentoConfirmado);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", parcel.PagamentoConfirmado);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //Atualizar parcela
        public static Boolean updateParcelValue(Parcel parcel)
        {
            SQLiteDataAdapter dataAdapter;
            try
            {
                var connection = databaseConnection();
                var command = databaseConnection().CreateCommand();
                command.CommandText = "UPDATE parcela SET valorParcela = @valorParcela WHERE idParcela = @idParcela;";
                command.Parameters.AddWithValue("@valorParcela", parcel.ValorParcela);
                command.Parameters.AddWithValue("@idParcela", parcel.IdParcela);
                dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception exception) { return false; }
        }

        //Atualizar parcela
        public static Boolean updateParcel(Parcel parcel)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE parcela SET idConta = @idConta, idCategoria = @idCategoria, valorParcela = @valorParcela, descricaoParcela = @descricaoParcela, dataTransacao = @dataTransacao, observacoesParcela = @observacoesParcela, recebimentoConfirmado = @recebimentoConfirmado, pagamentoConfirmado = @pagamentoConfirmado WHERE idParcela = @idParcela;";
                    command.Parameters.AddWithValue("@idConta", parcel.IdConta);
                    command.Parameters.AddWithValue("@idCategoria", parcel.IdCategoria);
                    command.Parameters.AddWithValue("@valorParcela", parcel.ValorParcela);
                    command.Parameters.AddWithValue("@descricaoParcela", parcel.DescricaoParcela);
                    command.Parameters.AddWithValue("@dataTransacao", parcel.DataTransacao.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@observacoesParcela", parcel.ObservacoesParcela);
                    command.Parameters.AddWithValue("@recebimentoConfirmado", parcel.RecebimentoConfirmado);
                    command.Parameters.AddWithValue("@pagamentoConfirmado", parcel.PagamentoConfirmado);
                    command.Parameters.AddWithValue("@idParcela", parcel.IdParcela);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir parcela
            public static Boolean deleteParcel(Parcel parcel)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM parcela WHERE idParcela = @idParcela;";
                    command.Parameters.AddWithValue("@idParcela", parcel.IdParcela);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todas as parcelas da receita
            public static Boolean deleteAllParcels(Income income)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM parcela WHERE idReceita = @idReceita;";
                    command.Parameters.AddWithValue("@idReceita", income.IdReceita);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todas as parcelas da despesa
            public static Boolean deleteAllParcels(Expense expense)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM parcela WHERE idDespesa = @idDespesa;";
                    command.Parameters.AddWithValue("@idDespesa", expense.IdDespesa);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Pagar parcela
            public static Boolean payParcel(Parcel parcel)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE parcela SET recebimentoConfirmado = true WHERE idParcela = @idParcela;";
                    command.Parameters.AddWithValue("@idParcela", parcel.IdParcela);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
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
                    command.Parameters.AddWithValue("@nomeCliente", client.NomeCliente);
                    command.Parameters.AddWithValue("@enderecoCliente", client.EnderecoCliente);
                    command.Parameters.AddWithValue("@numeroResidencia", client.NumeroResidencia);
                    command.Parameters.AddWithValue("@bairroCliente", client.BairroCliente);
                    command.Parameters.AddWithValue("@cidadeCliente", client.CidadeCliente);
                    command.Parameters.AddWithValue("@estadoCliente", client.EstadoCliente);
                    command.Parameters.AddWithValue("@emailCliente", client.EmailCliente);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }


            //Atualizar cliente
            public static Boolean updateClient(Client client)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE cliente SET nomeCliente = @nomeCliente, enderecoCliente = @enderecoCliente, numeroResidencia = @numeroResidencia, bairroCliente = @bairroCliente, cidadeCliente = @cidadeCliente, estadoCliente = @estadoCliente, emailCliente = @emailCliente WHERE idCliente = @idCliente;";
                    command.Parameters.AddWithValue("@nomeCliente", client.NomeCliente);
                    command.Parameters.AddWithValue("@enderecoCliente", client.EnderecoCliente);
                    command.Parameters.AddWithValue("@numeroResidencia", client.NumeroResidencia);
                    command.Parameters.AddWithValue("@bairroCliente", client.BairroCliente);
                    command.Parameters.AddWithValue("@cidadeCliente", client.CidadeCliente);
                    command.Parameters.AddWithValue("@estadoCliente", client.EstadoCliente);
                    command.Parameters.AddWithValue("@emailCliente", client.EmailCliente);
                    command.Parameters.AddWithValue("@idCliente", client.IdCliente);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Desassocia a receita do orçamento do cliente, a fim de exclui-lo
            public static Boolean updateBudgetNumber(Client client)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE receita SET numeroOrcamento = null WHERE idCliente = @idCliente;";
                    command.Parameters.AddWithValue("@idCliente", client.IdCliente);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir cliente
            public static Boolean deleteClient(Client client)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var commandClient = databaseConnection().CreateCommand();
                    commandClient.CommandText = "DELETE FROM cliente WHERE idCliente = @idCliente;";
                    commandClient.Parameters.AddWithValue("@idCliente", client.IdCliente);
                    dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                    commandClient.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
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
                    command.Parameters.AddWithValue("@idCliente", telephone.IdCliente);
                    command.Parameters.AddWithValue("@idFornecedor", telephone.IdFornecedor);
                    command.Parameters.AddWithValue("@tipoTelefone", telephone.TipoTelefone);
                    command.Parameters.AddWithValue("@numeroTelefone", telephone.NumeroTelefone);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar telefone
            public static Boolean updateTelephone(Telephone telephone)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE telefone SET tipoTelefone = @tipoTelefone, numeroTelefone = @numeroTelefone WHERE idTelefone = @idTelefone;";
                    command.Parameters.AddWithValue("@tipoTelefone", telephone.TipoTelefone);
                    command.Parameters.AddWithValue("@numeroTelefone", telephone.NumeroTelefone);
                    command.Parameters.AddWithValue("@idTelefone", telephone.IdTelefone);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todas os telefones do cliente
            public static Boolean deleteTelephone(Telephone telephone)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM telefone WHERE idTelefone = @idTelefone;";
                    command.Parameters.AddWithValue("@idTelefone", telephone.IdTelefone);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todas os telefones do cliente
            public static Boolean deleteAllTelephones(Client client)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM telefone WHERE idCliente = @idCliente;";
                    command.Parameters.AddWithValue("@idCliente", client.IdCliente);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todas os telefones do fornecedor
            public static Boolean deleteAllTelephones(Supplier supplier)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM telefone WHERE idFornecedor = @idFornecedor;";
                    command.Parameters.AddWithValue("@idFornecedor", supplier.IdFornecedor);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //PRODUTO

            //Cadastrar produto
            public static Boolean newProduct(Product product)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO produto (idFornecedor, idCategoria, nomeProduto, valorUnitario) VALUES (@idFornecedor, @idCategoria, @nomeProduto, @valorUnitario)";
                    command.Parameters.AddWithValue("@idFornecedor", product.IdFornecedor);
                    command.Parameters.AddWithValue("@idCategoria", product.IdCategoria);
                    command.Parameters.AddWithValue("@nomeProduto", product.NomeProduto);
                    command.Parameters.AddWithValue("@valorUnitario", product.ValorUnitario);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar produto
            public static Boolean updateProduct(Product product)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE produto SET idFornecedor = @idFornecedor, idCategoria = @idCategoria, nomeProduto = @nomeProduto, valorUnitario = @valorUnitario WHERE idProduto = @idProduto;";
                    command.Parameters.AddWithValue("@idFornecedor", product.IdFornecedor);
                    command.Parameters.AddWithValue("@idCategoria", product.IdCategoria);
                    command.Parameters.AddWithValue("@nomeProduto", product.NomeProduto);
                    command.Parameters.AddWithValue("@valorUnitario", product.ValorUnitario);
                    command.Parameters.AddWithValue("@idProduto", product.IdProduto);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir produto
            public static Boolean deleteProduct(Product product)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM produto WHERE idProduto = @idProduto;";
                    command.Parameters.AddWithValue("@idProduto", product.IdProduto);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todos os produtos
            public static Boolean deleteAllProducts(Supplier supplier)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM produto WHERE idFornecedor = @idFornecedor;";
                    command.Parameters.AddWithValue("@idFornecedor", supplier.IdFornecedor);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
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
                    command.Parameters.AddWithValue("@nomeFornecedor", supplier.NomeFornecedor);
                    command.Parameters.AddWithValue("@enderecoFornecedor", supplier.EnderecoFornecedor);
                    command.Parameters.AddWithValue("@numeroResidencia", supplier.NumeroResidencia);
                    command.Parameters.AddWithValue("@bairroFornecedor", supplier.BairroFornecedor);
                    command.Parameters.AddWithValue("@cidadeFornecedor", supplier.CidadeFornecedor);
                    command.Parameters.AddWithValue("@estadoFornecedor", supplier.EstadoFornecedor);
                    command.Parameters.AddWithValue("@emailFornecedor", supplier.EmailFornecedor);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }


            //Atualizar fornecedor
            public static Boolean updateSupplier(Supplier supplier)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE fornecedor SET nomeFornecedor = @nomeFornecedor, enderecoFornecedor = @enderecoFornecedor, numeroResidencia = @numeroResidencia, bairroFornecedor = @bairroFornecedor, cidadeFornecedor = @cidadeFornecedor, estadoFornecedor = @estadoFornecedor,  emailFornecedor = @emailFornecedor WHERE idFornecedor = @idFornecedor;";
                    command.Parameters.AddWithValue("@nomeFornecedor", supplier.NomeFornecedor);
                    command.Parameters.AddWithValue("@enderecoFornecedor", supplier.EnderecoFornecedor);
                    command.Parameters.AddWithValue("@numeroResidencia", supplier.NumeroResidencia);
                    command.Parameters.AddWithValue("@bairroFornecedor", supplier.BairroFornecedor);
                    command.Parameters.AddWithValue("@cidadeFornecedor", supplier.CidadeFornecedor);
                    command.Parameters.AddWithValue("@estadoFornecedor", supplier.EstadoFornecedor);
                    command.Parameters.AddWithValue("@emailFornecedor", supplier.EmailFornecedor);
                    command.Parameters.AddWithValue("@idFornecedor", supplier.IdFornecedor);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir fornecedor
            public static Boolean deleteSupplier(Supplier supplier)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var commandClient = databaseConnection().CreateCommand();
                    commandClient.CommandText = "DELETE FROM fornecedor WHERE idFornecedor = @idFornecedor;";
                    commandClient.Parameters.AddWithValue("@idFornecedor", supplier.IdFornecedor);
                    dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                    commandClient.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //ORÇAMENTO

            //Cadastrar orçamento
            public static Boolean newBudget(Budget budget)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    DateTime dateTime = DateTime.Parse(budget.DataOrcamento.ToString());
                    string formatForSQLite = dateTime.ToString("yyyy-MM-dd");
                    command.CommandText = "INSERT INTO orcamento (idCliente, dataOrcamento, valorTrabalho, valorTotal, condicaoPagamento, orcamentoConfirmado) VALUES (@idCliente, @dataOrcamento, @valorTrabalho, @valorTotal, @condicaoPagamento, @orcamentoConfirmado)";
                    command.Parameters.AddWithValue("@idCliente", budget.IdCliente);
                    command.Parameters.AddWithValue("@dataOrcamento", formatForSQLite);
                    command.Parameters.AddWithValue("@valorTrabalho", budget.ValorTrabalho);
                    command.Parameters.AddWithValue("@valorTotal", budget.ValorTotal);
                    command.Parameters.AddWithValue("@condicaoPagamento", budget.CondicaoPagamento);
                    command.Parameters.AddWithValue("@orcamentoConfirmado", budget.OrcamentoConfirmado);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar orçamento
            public static Boolean updateBudget(Budget budget)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    DateTime dateTime = DateTime.Parse(budget.DataOrcamento.ToString());
                    string formatForSQLite = dateTime.ToString("yyyy-MM-dd");
                    command.CommandText = "UPDATE orcamento SET dataOrcamento = @dataOrcamento, valorTrabalho = @valorTrabalho, valorTotal = @valorTotal, condicaoPagamento = @condicaoPagamento, orcamentoConfirmado = @orcamentoConfirmado WHERE numeroOrcamento = @numeroOrcamento;";
                    command.Parameters.AddWithValue("@dataOrcamento", formatForSQLite);
                    command.Parameters.AddWithValue("@valorTrabalho", budget.ValorTrabalho);
                    command.Parameters.AddWithValue("@valorTotal", budget.ValorTotal);
                    command.Parameters.AddWithValue("@condicaoPagamento", budget.CondicaoPagamento);
                    command.Parameters.AddWithValue("@orcamentoConfirmado", budget.OrcamentoConfirmado);
                    command.Parameters.AddWithValue("@numeroOrcamento", budget.NumeroOrcamento);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Confirmar orçamento
            public static Boolean confirmBudget(Budget budget)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE orcamento SET orcamentoConfirmado = @orcamentoConfirmado WHERE numeroOrcamento = @numeroOrcamento;";
                    command.Parameters.AddWithValue("@orcamentoConfirmado", budget.OrcamentoConfirmado);
                    command.Parameters.AddWithValue("@numeroOrcamento", budget.NumeroOrcamento);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar valor total do orçamento
            public static Boolean updateBudgetTotalValue(BudgetedProduct budgetedProduct, decimal valorTotal)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE orcamento SET valorTotal = @valorTotal WHERE numeroOrcamento = @numeroOrcamento;";
                    command.Parameters.AddWithValue("@valorTotal", valorTotal);
                    command.Parameters.AddWithValue("@numeroOrcamento", budgetedProduct.NumeroOrcamento);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir orçamento
            public static Boolean deleteBudget(Budget budget)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var commandClient = databaseConnection().CreateCommand();
                    commandClient.CommandText = "DELETE FROM orcamento WHERE numeroOrcamento = @numeroOrcamento;";
                    commandClient.Parameters.AddWithValue("@numeroOrcamento", budget.NumeroOrcamento);
                    dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                    commandClient.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todos os orçamentos
            public static Boolean deleteAllBudgets(Client client)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var commandClient = databaseConnection().CreateCommand();
                    commandClient.CommandText = "DELETE FROM orcamento WHERE idCliente = @idCliente;";
                    commandClient.Parameters.AddWithValue("@idCliente", client.IdCliente);
                    dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                    commandClient.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //PRODUTO ORÇADO
        
            //Cadastrar produto orçado
            public static Boolean newBudgetedProduct(BudgetedProduct budgetedProduct)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO produtoOrcado (idProduto, numeroOrcamento, item, quantidadeProduto, valorTotal) VALUES (@idProduto, @numeroOrcamento, @item, @quantidadeProduto, @valorTotal)";
                    command.Parameters.AddWithValue("@idProduto", budgetedProduct.IdProduto);
                    command.Parameters.AddWithValue("@numeroOrcamento", budgetedProduct.NumeroOrcamento);
                    command.Parameters.AddWithValue("@item", budgetedProduct.Item);
                    command.Parameters.AddWithValue("@quantidadeProduto", budgetedProduct.QuantidadeProduto);
                    command.Parameters.AddWithValue("@valorTotal", budgetedProduct.ValorTotal);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar produto orçado
            public static Boolean updateBudgetedProduct(BudgetedProduct budgetedProduct)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE produtoOrcado SET idProduto = @idProduto, numeroOrcamento = @numeroOrcamento, item = @item, quantidadeProduto = @quantidadeProduto, valorTotal = @valorTotal WHERE idProdutoOrcado = @idProdutoOrcado;";
                    command.Parameters.AddWithValue("@idProduto", budgetedProduct.IdProduto);
                    command.Parameters.AddWithValue("@numeroOrcamento", budgetedProduct.NumeroOrcamento);
                    command.Parameters.AddWithValue("@item", budgetedProduct.Item);
                    command.Parameters.AddWithValue("@quantidadeProduto", budgetedProduct.QuantidadeProduto);
                    command.Parameters.AddWithValue("@valorTotal", budgetedProduct.ValorTotal);
                    command.Parameters.AddWithValue("@idProdutoOrcado", budgetedProduct.IdProdutoOrcado);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir produto orçado
            public static Boolean deleteBudgetedProduct(BudgetedProduct budgetedProduct)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var commandClient = databaseConnection().CreateCommand();
                    commandClient.CommandText = "DELETE FROM produtoOrcado WHERE idProdutoOrcado = @idProdutoOrcado;";
                    commandClient.Parameters.AddWithValue("@idProdutoOrcado", budgetedProduct.IdProdutoOrcado);
                    dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                    commandClient.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todos os produtos orçados
            public static Boolean deleteAllBudgetedProducts(Product product)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM produtoOrcado WHERE idProduto = @idProduto;";
                    command.Parameters.AddWithValue("@idProduto", product.IdProduto);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir todos os produtos orçados
            public static Boolean deleteAllBudgetedProducts(Budget budget)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM produtoOrcado WHERE numeroOrcamento = @numeroOrcamento;";
                    command.Parameters.AddWithValue("@numeroOrcamento", budget.NumeroOrcamento);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar número do item dos produtos orçados
            public static Boolean updateBudgetedProductItemValue(BudgetedProduct budgetedProduct)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var commandClient = databaseConnection().CreateCommand();
                    commandClient.CommandText = "UPDATE produtoOrcado SET item = @item WHERE idProdutoOrcado = @idProdutoOrcado;";
                    commandClient.Parameters.AddWithValue("@item", budgetedProduct.Item);
                    commandClient.Parameters.AddWithValue("@idProdutoOrcado", budgetedProduct.IdProdutoOrcado);
                    dataAdapter = new SQLiteDataAdapter(commandClient.CommandText, connection);
                    commandClient.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //CONTA

            //Cadastrar conta
            public static Boolean newAccount(Account account)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO conta (saldoConta, nomeConta, tipoConta, somarTotal) VALUES (@saldoConta, @nomeConta, @tipoConta, @somarTotal)";
                    command.Parameters.AddWithValue("@saldoConta", account.SaldoConta);
                    command.Parameters.AddWithValue("@nomeConta", account.NomeConta);
                    command.Parameters.AddWithValue("@tipoConta", account.TipoConta);
                    command.Parameters.AddWithValue("@somarTotal", account.SomarTotal);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar conta
            public static Boolean updateAccount(Account account)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE conta SET saldoConta = @saldoConta, nomeConta = @nomeConta, tipoConta = @tipoConta, somarTotal = @somarTotal WHERE idConta = @idConta;";
                    command.Parameters.AddWithValue("@saldoConta", account.SaldoConta);
                    command.Parameters.AddWithValue("@nomeConta", account.NomeConta);
                    command.Parameters.AddWithValue("@tipoConta", account.TipoConta);
                    command.Parameters.AddWithValue("@somarTotal", account.SomarTotal);
                    command.Parameters.AddWithValue("@idConta", account.IdConta);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir conta
            public static Boolean deleteAccount(Account account)
            {
            SQLiteDataAdapter dataAdapter;
            try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM conta WHERE idConta = @idConta;";
                    command.Parameters.AddWithValue("@idConta", account.IdConta);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar conta ativa
            public static Boolean updateActiveAccount(Account account)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE conta SET contaAtiva = @contaAtiva WHERE idConta = @idConta;";
                    command.Parameters.AddWithValue("@contaAtiva", account.ContaAtiva);
                    command.Parameters.AddWithValue("@idConta", account.IdConta);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar saldo da conta
            public static Boolean updateAccountBalance(Account account)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE conta SET saldoConta = @saldoConta WHERE idConta = @idConta;";
                    command.Parameters.AddWithValue("@saldoConta", account.SaldoConta);
                    command.Parameters.AddWithValue("@idConta", account.IdConta);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

        //CATEGORIA

        //Cadastrar categoria
        public static Boolean newCategory(Category category)
            {
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "INSERT INTO categoria (categoriaReceita, categoriaDespesa, categoriaProduto, nomeCategoria) VALUES (@categoriaReceita, @categoriaDespesa, @categoriaProduto, @nomeCategoria)";
                    command.Parameters.AddWithValue("@categoriaReceita", category.CategoriaReceita);
                    command.Parameters.AddWithValue("@categoriaDespesa", category.CategoriaDespesa);
                    command.Parameters.AddWithValue("@categoriaProduto", category.CategoriaProduto);
                    command.Parameters.AddWithValue("@nomeCategoria", category.NomeCategoria);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Atualizar categoria
            public static Boolean updateCategory(Category category)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "UPDATE categoria SET nomeCategoria = @nomeCategoria WHERE idCategoria = @idCategoria;";
                    command.Parameters.AddWithValue("@nomeCategoria", category.NomeCategoria);
                    command.Parameters.AddWithValue("@idCategoria", category.IdCategoria);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }

            //Excluir categoria
            public static Boolean deleteCategory(Category category)
            {
                SQLiteDataAdapter dataAdapter;
                try
                {
                    var connection = databaseConnection();
                    var command = databaseConnection().CreateCommand();
                    command.CommandText = "DELETE FROM categoria WHERE idCategoria = @idCategoria;";
                    command.Parameters.AddWithValue("@idCategoria", category.IdCategoria);
                    dataAdapter = new SQLiteDataAdapter(command.CommandText, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception exception) { return false; }
            }
    }
}
