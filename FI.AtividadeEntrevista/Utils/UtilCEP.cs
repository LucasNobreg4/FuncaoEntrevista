using System.Linq;

namespace FI.AtividadeEntrevista.Utils
{
    public static class UtilCEP
    {
        /// <summary>
        /// Valida CEP básico (apenas formato)
        /// </summary>
        /// <param name="cep">CEP com ou sem formatação</param>
        /// <returns>True se CEP é válido</returns>
        public static bool ValidarCEP(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return false;

            // Remove formatação
            cep = RemoverFormatacao(cep);

            // CEP deve ter exatamente 8 dígitos
            if (cep.Length != 8)
                return false;

            // Verifica se todos os caracteres são dígitos
            if (!cep.All(char.IsDigit))
                return false;

            return true;
        }

        /// <summary>
        /// Remove formatação do CEP (hífen, espaços)
        /// </summary>
        public static string RemoverFormatacao(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return string.Empty;

            return cep.Replace("-", "").Replace(" ", "").Trim();
        }

        /// <summary>
        /// Formata CEP (apenas números) para o padrão 00000-000
        /// </summary>
        public static string Formatar(string cep)
        {
            cep = RemoverFormatacao(cep);

            if (cep.Length != 8)
                return cep;

            return string.Format("{0}-{1}", 
                cep.Substring(0, 5), 
                cep.Substring(5, 3));
        }
    }
}
