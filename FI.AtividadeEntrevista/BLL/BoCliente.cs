using System.Collections.Generic;
using FI.AtividadeEntrevista.Utils;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        private readonly DAL.DaoCliente _daoCliente;

        public BoCliente()
        {
            _daoCliente = new DAL.DaoCliente();
        }

        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            cliente.CPF = UtilCPF.RemoverFormatacao(cliente.CPF);
            cliente.CEP = UtilCPF.RemoverFormatacao(cliente.CEP);
            return _daoCliente.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            cliente.CPF = UtilCPF.RemoverFormatacao(cliente.CPF);
            cliente.CEP = UtilCPF.RemoverFormatacao(cliente.CEP);
            _daoCliente.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            return _daoCliente.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            _daoCliente.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            return _daoCliente.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            return _daoCliente.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            return _daoCliente.VerificarExistencia(CPF);
        }
    }
}
