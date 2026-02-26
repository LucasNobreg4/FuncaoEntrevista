using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    internal class DaoBeneficiario : AcessoDados
    {
        internal long Incluir(Beneficiario beneficiario)
        {
            string cpfSemFormatacao = beneficiario.CPF.Replace(".", "").Replace("-", "");

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", cpfSemFormatacao),
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("IdCliente", beneficiario.IdCliente)
            };

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        internal void Alterar(Beneficiario beneficiario)
        {
            string cpfSemFormatacao = beneficiario.CPF.Replace(".", "").Replace("-", "");

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", beneficiario.Id),
                new SqlParameter("CPF", cpfSemFormatacao),
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("IdCliente", beneficiario.IdCliente)
            };

            base.Executar("FI_SP_AltBeneficiario", parametros);
        }

        internal Beneficiario Consultar(long id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", id)
            };

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> beneficiarios = Converter(ds);

            return beneficiarios.FirstOrDefault();
        }

        internal List<Beneficiario> ListarPorCliente(long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("IdCliente", idCliente)
            };

            DataSet ds = base.Consultar("FI_SP_ListBeneficiariosPorCliente", parametros);
            List<Beneficiario> beneficiarios = Converter(ds);

            return beneficiarios;
        }

        internal void Excluir(long id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", id)
            };

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        internal bool VerificarCPFDuplicado(string cpf, long idCliente, long idBeneficiario = 0)
        {
            string cpfSemFormatacao = cpf.Replace(".", "").Replace("-", "");

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", cpfSemFormatacao),
                new SqlParameter("IdCliente", idCliente)
            };

            if (idBeneficiario > 0)
            {
                parametros.Add(new SqlParameter("IdBeneficiario", idBeneficiario));
            }

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiarioCPF", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        internal long? ConsultarClientePorCPFBeneficiario(string cpf)
        {
            string cpfSemFormatacao = cpf.Replace(".", "").Replace("-", "");

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", cpfSemFormatacao)
            };

            DataSet ds = base.Consultar("FI_SP_ConsClientePorCPFBeneficiario", parametros);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0].Field<long>("IdCliente");
            }

            return null;
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario beneficiario = new Beneficiario
                    {
                        Id = row.Field<long>("Id"),
                        CPF = row.Field<string>("CPF"),
                        Nome = row.Field<string>("Nome"),
                        IdCliente = row.Field<long>("IdCliente")
                    };
                    lista.Add(beneficiario);
                }
            }

            return lista;
        }
    }
}
