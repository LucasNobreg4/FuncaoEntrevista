using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FI.AtividadeEntrevista.DAL
{
    internal class AcessoDados
    {
        private string stringDeConexao
        {
            get
            {
                ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["BancoDeDados"];
                if (conn != null)
                    return conn.ConnectionString;
                else
                    return string.Empty;
            }
        }

        internal void Executar(string NomeProcedure, List<SqlParameter> parametros)
        {
            using (SqlConnection conexao = new SqlConnection(stringDeConexao))
            using (SqlCommand comando = new SqlCommand())
            {
                comando.Connection = conexao;
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = NomeProcedure;

                foreach (var item in parametros)
                    comando.Parameters.Add(item);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }

        internal DataSet Consultar(string NomeProcedure, List<SqlParameter> parametros)
        {
            using (SqlConnection conexao = new SqlConnection(stringDeConexao))
            using (SqlCommand comando = new SqlCommand())
            using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
            {
                comando.Connection = conexao;
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = NomeProcedure;

                foreach (var item in parametros)
                    comando.Parameters.Add(item);

                DataSet ds = new DataSet();
                conexao.Open();
                adapter.Fill(ds);
                return ds;
            }
        }

    }
}
