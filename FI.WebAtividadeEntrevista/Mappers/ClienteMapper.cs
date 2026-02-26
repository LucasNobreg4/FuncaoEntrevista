using FI.AtividadeEntrevista.DML;
using WebAtividadeEntrevista.Models;
using FI.AtividadeEntrevista.Utils;

namespace FI.WebAtividadeEntrevista.Mappers
{
    /// <summary>
    /// Classe responsável pelo mapeamento entre ClienteModel e Cliente (entidade DML)
    /// </summary>
    public static class ClienteMapper
    {
        /// <summary>
        /// Converte ClienteModel para Cliente (entidade DML)
        /// </summary>
        /// <param name="model">Model da camada de apresentação</param>
        /// <returns>Entidade Cliente ou null se model for null</returns>
        public static Cliente ParaEntidade(ClienteModel model)
        {
            if (model == null)
                return null;

            return new Cliente
            {
                Id = model.Id,
                CEP = UtilCEP.RemoverFormatacao(model.CEP),
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone,
                CPF = UtilCPF.RemoverFormatacao(model.CPF)
            };
        }

        /// <summary>
        /// Converte Cliente (entidade DML) para ClienteModel
        /// </summary>
        /// <param name="cliente">Entidade da camada de domínio</param>
        /// <returns>ClienteModel ou null se cliente for null</returns>
        public static ClienteModel ParaModel(Cliente cliente)
        {
            if (cliente == null)
                return null;

            return new ClienteModel
            {
                Id = cliente.Id,
                CEP = UtilCEP.Formatar(cliente.CEP),
                Cidade = cliente.Cidade,
                Email = cliente.Email,
                Estado = cliente.Estado,
                Logradouro = cliente.Logradouro,
                Nacionalidade = cliente.Nacionalidade,
                Nome = cliente.Nome,
                Sobrenome = cliente.Sobrenome,
                Telefone = cliente.Telefone,
                CPF = UtilCPF.Formatar(cliente.CPF)
            };
        }
    }
}
