using FI.AtividadeEntrevista.DML;
using WebAtividadeEntrevista.Models;
using FI.AtividadeEntrevista.Utils;

namespace FI.WebAtividadeEntrevista.Mappers
{
    public static class BeneficiarioMapper
    {
        public static Beneficiario ParaEntidade(BeneficiarioModel model)
        {
            if (model == null)
                return null;

            return new Beneficiario
            {
                Id = model.Id,
                CPF = UtilCPF.RemoverFormatacao(model.CPF),
                Nome = model.Nome,
                IdCliente = model.IDCliente
            };
        }

        public static BeneficiarioModel ParaModel(Beneficiario beneficiario)
        {
            if (beneficiario == null)
                return null;

            return new BeneficiarioModel
            {
                Id = beneficiario.Id,
                CPF = UtilCPF.Formatar(beneficiario.CPF),
                Nome = beneficiario.Nome,
                IDCliente = beneficiario.IdCliente
            };
        }
    }
}
