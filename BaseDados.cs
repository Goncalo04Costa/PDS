using System;
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

                    Sobremesa.Desconectar();
                }
                else
                {
                    Console.WriteLine("Falha ao conectar à base de dados.");
                }


                  Sobremesa.RemoverSobremesa(1);

                    Sobremesa.Desconectar();
            }
        }
    }
}
