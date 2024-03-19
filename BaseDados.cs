/*using System;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoDesenvolvimentoSoftware
{
    public class Sobremesa
    {
        private static SqlConnection conexao;

        static Sobremesa()
        {
            string connectionString = "Data Source=GONCALO;Initial Catalog=PDS;User ID=GONCALO\\gonca;Integrated Security=True;";
            conexao = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Conectar à base de dados
        /// </summary>
        /// <returns>True se a conexão for bem-sucedida, false caso contrário.</returns>
        public static bool Conectar()
        {
            try
            {
                conexao.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar à base de dados: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Desconectar da base de dados
        /// </summary>
        public static void Desconectar()
        {
            if (conexao != null && conexao.State != System.Data.ConnectionState.Closed)
            {
                conexao.Close();
            }
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Tipo { get; set; }

        public Sobremesa() { }

        /// <summary>
        /// Construtor para sobremesa.
        /// Recebe uma tabela com dados e de acordo com as colunas vai adicionar ao objeto.
        /// </summary>
        /// <param name="tabela">Tabela de dados.</param>
        public Sobremesa(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            if (tabela.Table.Columns.Contains("Descricao"))
            {
                this.Descricao = tabela.Field<string>("Descricao");
            }
            if (tabela.Table.Columns.Contains("Tipo"))
            {
                this.Tipo = tabela.Field<bool>("Tipo");
            }
        }

        /// <summary>
        /// Método para obter uma sobremesa com um determinado id.
        /// </summary>
        /// <param name="connectionString">String de ligação à base de dados.</param>
        /// <param name="id">Id da sobremesa a obeter.</param>
        /// <returns></returns>
        public static Sobremesa ObterSobremesaId(string connectionString, int id)
        {
            Sobremesa obj = null;
            using (SqlConnection ligacao = new SqlConnection(connectionString))
            {
                try
                {
                    ligacao.Open();

                    string sql = $"Select Id, Nome, Descricao, Tipo From Sobremesas where Id = {id};";
                    SqlCommand comando = new SqlCommand(sql, ligacao);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);

                    DataTable dataTable = new DataTable();
                    adaptador.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        obj = new Sobremesa(dataTable.Rows[0]);
                    }

                    ligacao.Close();
                    adaptador.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
            return obj;
        }

        /// <summary>
        /// Adiciona uma nova sobremesa à base de dados.
        /// </summary>
        /// <param name="nome">Nome da sobremesa.</param>
        /// <param name="descricao">Descrição da sobremesa.</param>
        /// <param name="tipo">Tipo da sobremesa (true para dieta, false para normal).</param>
        /// <returns>True se a sobremesa foi adicionada com sucesso, false caso contrário.</returns>
public static bool NovaSobremesa(string nome, string descricao, bool tipo)
{
    try
    {
        string sql = "INSERT INTO Sobremesas (Nome, Descricao, Tipo) VALUES (@Nome, @Descricao, @Tipo);";

        using (SqlCommand comando = new SqlCommand(sql, conexao))
        {
            comando.Parameters.AddWithValue("@Nome", nome);
            comando.Parameters.AddWithValue("@Descricao", descricao);
            comando.Parameters.AddWithValue("@Tipo", tipo);

            conexao.Open();
            comando.ExecuteNonQuery();
            return true;
        }
    }
    catch (SqlException ex)
    {
        Console.WriteLine("Erro SQL ao adicionar nova sobremesa: " + ex.Message);
        return false;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao adicionar nova sobremesa: " + ex.Message);
        return false;
    }
    finally
    {
        if (conexao.State != System.Data.ConnectionState.Closed)
        {
            conexao.Close();
        }
    }
}



   /// <summary>
        /// Remove uma sobremesa da base de dados com base no ID.
        /// </summary>
        /// <param name="id">ID da sobremesa a ser removida.</param>
        /// <returns>True se a sobremesa foi removida com sucesso, false caso contrário.</returns>
        public static bool RemoverSobremesa(int id)
        {
            try
            {
                string sql = "DELETE FROM Sobremesas WHERE Id = @Id;";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    conexao.Open();
                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Console.WriteLine("Sobremesa removida com sucesso!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma sobremesa encontrada com o ID especificado.");
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro SQL ao remover sobremesa: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao remover sobremesa: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexao.State != System.Data.ConnectionState.Closed)
                {
                    conexao.Close();
                }
            }
        }

        /// <summary>
        /// Altera os dados de uma sobremesa na de dados com base no ID.
        /// </summary>
        /// <param name="id">ID da sobremesa a ser alterada.</param>
        /// <param name="nome">Novo nome da sobremesa.</param>
        /// <param name="descricao">Nova descrição da sobremesa.</param>
        /// <param name="tipo">Novo tipo da sobremesa (true para dieta, false para normal).</param>
        /// <returns>True se os dados da sobremesa foram alterados com sucesso, false caso contrário.</returns>
        public static bool AlterarSobremesa(int id, string nome, string descricao, bool tipo)
        {
            try
            {
                string sql = "UPDATE Sobremesas SET Nome = @Nome, Descricao = @Descricao, Tipo = @Tipo WHERE Id = @Id;";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Nome", nome);
                    comando.Parameters.AddWithValue("@Descricao", descricao);
                    comando.Parameters.AddWithValue("@Tipo", tipo);
                    comando.Parameters.AddWithValue("@Id", id);

                    conexao.Open();
                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Console.WriteLine("Dados da sobremesa alterados com sucesso!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma sobremesa encontrada com o ID especificado.");
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro SQL ao alterar dados da sobremesa: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao alterar dados da sobremesa: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexao.State != System.Data.ConnectionState.Closed)
                {
                    conexao.Close();
                }
            }
        }

        internal class Program
        {
            static void Main(string[] args)
            {
                string connectionString = "Data Source=GONCALO;Initial Catalog=PDS;User ID=GONCALO\\gonca;Integrated Security=True;";

                if (Sobremesa.Conectar())
                {
                    Console.WriteLine("Conexão bem-sucedida à base de dados.");

                    // Adicionando uma nova sobremesa
                    bool sobremesaAdicionada = Sobremesa.NovaSobremesa("Pudim", "Delicioso pudim de leite", true);

                    if (sobremesaAdicionada)
                    {
                        Console.WriteLine("Nova sobremesa adicionada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Falha ao adicionar nova sobremesa.");
                    }

                    // Obtendo e exibindo a primeira sobremesa
                    Sobremesa obj1 = Sobremesa.ObterSobremesaId(connectionString, 1);
                    if (obj1 != null)
                    {
                        Console.WriteLine($"Sobremesa ID: {obj1.Id}, Nome: {obj1.Nome}, Descrição: {obj1.Descricao}, Tipo: {(obj1.Tipo ? "Dieta" : "Normal")}");
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma sobremesa encontrada com o ID 1.");
                    }

                    // Obtendo e exibindo a segunda sobremesa
                    Sobremesa obj2 = Sobremesa.ObterSobremesaId(connectionString, 2);
                    if (obj2 != null)
                    {
                        Console.WriteLine($"Sobremesa ID: {obj2.Id}, Nome: {obj2.Nome}, Descrição: {obj2.Descricao}, Tipo: {(obj2.Tipo ? "Dieta" : "Normal")}");
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma sobremesa encontrada com o ID 2.");
                    }

                    // Obtendo e exibindo a terceira sobremesa
                    Sobremesa obj3 = Sobremesa.ObterSobremesaId(connectionString, 3);
                    if (obj3 != null)
                    {
                        Console.WriteLine($"Sobremesa ID: {obj3.Id}, Nome: {obj3.Nome}, Descrição: {obj3.Descricao}, Tipo: {(obj3.Tipo ? "Dieta" : "Normal")}");
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma sobremesa encontrada com o ID 3.");
                    }

                    // Alterando os dados da terceira sobremesa
                    Sobremesa.AlterarSobremesa(2, "Bolo de Chocolate", "Bolo de chocolate com cobertura cremosa", false);

                    Sobremesa.Desconectar();
                }
                else
                {
                    Console.WriteLine("Falha ao conectar à base de dados.");
                }
            }
        }
    }
}
*/

using System;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoDesenvolvimentoSoftware
{
    public class Utente
    {
        private static SqlConnection conexao;

        static Utente()
        {
            string connectionString = "Data Source=GONCALO;Initial Catalog=PDS;User ID=GONCALO\\gonca;Integrated Security=True;";
            conexao = new SqlConnection(connectionString);
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int NIF { get; set; }
        public int SNS { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Historico { get; set; }
        public bool Tipo { get; set; }
        public int TiposAdmissaoId { get; set; }
        public string MotivoAdmissao { get; set; }
        public string DiagnosticoAdmissao { get; set; }
        public string Observacoes { get; set; }
        public string NotaAdmissao { get; set; }
        public string AntecedentesPessoais { get; set; }
        public string ExameObjetivo { get; set; }
        public float Mensalidade { get; set; }
        public float Cofinanciamento { get; set; }

        public Utente() { }

        public Utente(DataRow tabela)
        {
            if (tabela.Table.Columns.Contains("Id"))
            {
                this.Id = tabela.Field<int>("Id");
            }
            if (tabela.Table.Columns.Contains("Nome"))
            {
                this.Nome = tabela.Field<string>("Nome");
            }
            // Preencher outras propriedades aqui de acordo com o esquema do base de dados
        }

        public static bool Conectar()
        {
            try
            {
                conexao.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar à base de dados: " + ex.Message);
                return false;
            }
        }

        public static void Desconectar()
        {
            if (conexao != null && conexao.State != ConnectionState.Closed)
            {
                conexao.Close();
            }
        }

        public static bool InserirUtente(string nome, int nif, int sns, DateTime dataAdmissao, DateTime dataNascimento, bool historico, bool tipo, int tiposAdmissaoId, string motivoAdmissao, string diagnosticoAdmissao, string observacoes, string notaAdmissao, string antecedentesPessoais, string exameObjetivo, float mensalidade, float cofinanciamento)
        {
            try
            {
                string sql = "INSERT INTO Utentes (Nome, NIF, SNS, DataAdmissao, DataNascimento, Historico, Tipo, TiposAdmissaoId, MotivoAdmissao, DiagnosticoAdmissao, Observacoes, NotaAdmissao, AntecedentesPessoais, ExameObjetivo, Mensalidade, Cofinanciamento) " +
                             "VALUES (@Nome, @NIF, @SNS, @DataAdmissao, @DataNascimento, @Historico, @Tipo, @TiposAdmissaoId, @MotivoAdmissao, @DiagnosticoAdmissao, @Observacoes, @NotaAdmissao, @AntecedentesPessoais, @ExameObjetivo, @Mensalidade, @Cofinanciamento);";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Nome", nome);
                    comando.Parameters.AddWithValue("@NIF", nif);
                    comando.Parameters.AddWithValue("@SNS", sns);
                    comando.Parameters.AddWithValue("@DataAdmissao", dataAdmissao);
                    comando.Parameters.AddWithValue("@DataNascimento", dataNascimento);
                    comando.Parameters.AddWithValue("@Historico", historico);
                    comando.Parameters.AddWithValue("@Tipo", tipo);
                    comando.Parameters.AddWithValue("@TiposAdmissaoId", tiposAdmissaoId);
                    comando.Parameters.AddWithValue("@MotivoAdmissao", motivoAdmissao ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@DiagnosticoAdmissao", diagnosticoAdmissao ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@Observacoes", observacoes ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@NotaAdmissao", notaAdmissao ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@AntecedentesPessoais", antecedentesPessoais ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@ExameObjetivo", exameObjetivo ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@Mensalidade", mensalidade);
                    comando.Parameters.AddWithValue("@Cofinanciamento", cofinanciamento);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro SQL ao adicionar novo utente: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao adicionar novo utente: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }
            }
        }

        public static bool AtualizarUtente(int id, string nome, int nif, int sns, DateTime dataAdmissao, DateTime dataNascimento, bool historico, bool tipo, int tiposAdmissaoId, string motivoAdmissao, string diagnosticoAdmissao, string observacoes, string notaAdmissao, string antecedentesPessoais, string exameObjetivo, float mensalidade, float cofinanciamento)
        {
            try
            {
                string sql = "UPDATE Utentes SET Nome = @Nome, NIF = @NIF, SNS = @SNS, DataAdmissao = @DataAdmissao, DataNascimento = @DataNascimento, Historico = @Historico, Tipo = @Tipo, " +
                             "TiposAdmissaoId = @TiposAdmissaoId, MotivoAdmissao = @MotivoAdmissao, DiagnosticoAdmissao = @DiagnosticoAdmissao, Observacoes = @Observacoes, NotaAdmissao = @NotaAdmissao, " +
                             "AntecedentesPessoais = @AntecedentesPessoais, ExameObjetivo = @ExameObjetivo, Mensalidade = @Mensalidade, Cofinanciamento = @Cofinanciamento WHERE Id = @Id;";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Nome", nome);
                    comando.Parameters.AddWithValue("@NIF", nif);
                    comando.Parameters.AddWithValue("@SNS", sns);
                    comando.Parameters.AddWithValue("@DataAdmissao", dataAdmissao);
                    comando.Parameters.AddWithValue("@DataNascimento", dataNascimento);
                    comando.Parameters.AddWithValue("@Historico", historico);
                    comando.Parameters.AddWithValue("@Tipo", tipo);
                    comando.Parameters.AddWithValue("@TiposAdmissaoId", tiposAdmissaoId);
                    comando.Parameters.AddWithValue("@MotivoAdmissao", motivoAdmissao ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@DiagnosticoAdmissao", diagnosticoAdmissao ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@Observacoes", observacoes ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@NotaAdmissao", notaAdmissao ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@AntecedentesPessoais", antecedentesPessoais ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@ExameObjetivo", exameObjetivo ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@Mensalidade", mensalidade);
                    comando.Parameters.AddWithValue("@Cofinanciamento", cofinanciamento);
                    comando.Parameters.AddWithValue("@Id", id);

                    conexao.Open();
                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Console.WriteLine("Dados do utente alterados com sucesso!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Nenhum utente encontrado com o ID especificado.");
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro SQL ao alterar dados do utente: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao alterar dados do utente: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }
            }
        }

        public static Utente ObterUtentePorId(int id)
        {
            try
            {
                Utente obj = null;
                string sql = "SELECT * FROM Utentes WHERE Id = @Id;";
                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Id", id);
                    conexao.Open();
                    SqlDataReader leitor = comando.ExecuteReader();
                    if (leitor.Read())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(leitor);
                        obj = new Utente(dataTable.Rows[0]);
                    }
                    leitor.Close();
                }
                return obj;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro SQL ao obter utente por ID: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter utente por ID: " + ex.Message);
                return null;
            }
            finally
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }
            }
        }

        public static bool RemoverUtente(int id)
        {
            try
            {
                string sql = "DELETE FROM Utentes WHERE Id = @Id;";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    conexao.Open();
                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Console.WriteLine("Utente removido com sucesso!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Nenhum utente encontrado com o ID especificado.");
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erro SQL ao remover utente: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao remover utente: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = "Data Source=GONCALO;Initial Catalog=PDS;User ID=GONCALO\\gonca;Integrated Security=True;";

            if (Utente.Conectar())
            {
                Console.WriteLine("Conexão bem-sucedida à base de dados.");

                // Adicionando um novo utente
                bool utenteAdicionado = Utente.InserirUtente("João", 123456789, 987654321, DateTime.Now, new DateTime(1990, 5, 15), true, true, 1, "Admissão por urgência", "Diagnóstico inicial", "Observações sobre o estado do utente", "Nota sobre a admissão", "Antecedentes médicos do utente", "Exame físico do utente", 500.00f, 100.00f);

                if (utenteAdicionado)
                {
                    Console.WriteLine("Novo utente adicionado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Falha ao adicionar novo utente.");
                }

                // Obtendo e exibindo o utente adicionado
                Utente utenteObtido = Utente.ObterUtentePorId(1);
                if (utenteObtido != null)
                {
                    Console.WriteLine($"Utente ID: {utenteObtido.Id}, Nome: {utenteObtido.Nome}, NIF: {utenteObtido.NIF}, SNS: {utenteObtido.SNS}, Data Admissão: {utenteObtido.DataAdmissao}, Data Nascimento: {utenteObtido.DataNascimento}, Historico: {utenteObtido.Historico}, Tipo: {utenteObtido.Tipo}, TiposAdmissaoId: {utenteObtido.TiposAdmissaoId}");
                }
                else
                {
                    Console.WriteLine("Nenhum utente encontrado com o ID 1.");
                }

                // Atualizando o utente
                bool utenteAtualizado = Utente.AtualizarUtente(1, "João Silva", 123456789, 987654321, DateTime.Now, new DateTime(1990, 5, 15), true, true, 1, "Admissão por urgência", "Diagnóstico inicial", "Observações sobre o estado do utente", "Nota sobre a admissão", "Antecedentes médicos do utente", "Exame físico do utente", 550.00f, 100.00f);

                if (utenteAtualizado)
                {
                    Console.WriteLine("Utente atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Falha ao atualizar utente.");
                }

                // Removendo o utente
                bool utenteRemovido = Utente.RemoverUtente(1);

                if (utenteRemovido)
                {
                    Console.WriteLine("Utente removido com sucesso!");
                }
                else
                {
                    Console.WriteLine("Falha ao remover utente.");
                }

                Utente.Desconectar();
            }
            else
            {
                Console.WriteLine("Falha ao conectar à base de dados.");
            }
        }
    }
}
