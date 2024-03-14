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
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=GONCALO;Initial Catalog=PDS;User ID=GONCALO\\gonca;Integrated Security=True;";

            if (Sobremesa.Conectar())
            {
                Console.WriteLine("Conexão bem-sucedida à base de dados.");

                Sobremesa obj = Sobremesa.ObterSobremesaId(connectionString, 1);

                if (obj != null)
                {
                    if (obj.Tipo)
                        Console.WriteLine($"Sobremesa ID: {obj.Id}, Nome: {obj.Nome}, Descrição: {obj.Descricao}, Tipo: Dieta");
                    else
                        Console.WriteLine($"Sobremesa ID: {obj.Id}, Nome: {obj.Nome}, Descrição: {obj.Descricao}, Tipo: Normal");
                }
                else
                {
                    Console.WriteLine("Nenhuma sobremesa encontrada com o ID especificado.");
                }

                Sobremesa.Desconectar();
            }
            else
            {
                Console.WriteLine("Falha ao conectar à base de dados.");
            }
        }
    }

}