using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        private readonly DaoBeneficiario _dao;

        public BoBeneficiario()
        {
            _dao = new DaoBeneficiario();
        }

        public long Incluir(Beneficiario beneficiario)
        {
            return _dao.Incluir(beneficiario);
        }

        public void Alterar(Beneficiario beneficiario)
        {
            _dao.Alterar(beneficiario);
        }

        public Beneficiario Consultar(long id)
        {
            return _dao.Consultar(id);
        }

        public List<Beneficiario> ListarPorCliente(long idCliente)
        {
            return _dao.ListarPorCliente(idCliente);
        }

        public void Excluir(long id)
        {
            _dao.Excluir(id);
        }

        public bool VerificarCPFDuplicado(string cpf, long idCliente, long idBeneficiario = 0)
        {
            return _dao.VerificarCPFDuplicado(cpf, idCliente, idBeneficiario);
        }

        public long? ConsultarClientePorCPFBeneficiario(string cpf)
        {
            return _dao.ConsultarClientePorCPFBeneficiario(cpf);
        }
    }
}
