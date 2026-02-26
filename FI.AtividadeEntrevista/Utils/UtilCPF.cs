using System.Linq;

namespace FI.AtividadeEntrevista.Utils
{
    public static class UtilCPF
    {
        /// <summary>
        /// Valida CPF com cálculo dos dígitos verificadores
        /// </summary>
        /// <param name="cpf">CPF com ou sem formatação</param>
        /// <returns>True se CPF é válido</returns>
        public static bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove formatação (pontos, hífens, espaços)
            cpf = RemoverFormatacao(cpf);

            // CPF deve ter exatamente 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os caracteres são dígitos
            if (!cpf.All(char.IsDigit))
                return false;

            // Verifica se todos os dígitos são iguais (ex: 111.111.111-11)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcula e valida o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digitoVerificador1)
                return false;

            // Calcula e valida o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[10].ToString()) != digitoVerificador2)
                return false;

            return true;
        }

        /// <summary>
        /// Remove formatação do CPF (pontos, hífens, espaços)
        /// </summary>
        public static string RemoverFormatacao(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            return cpf.Replace(".", "").Replace("-", "").Replace(" ", "").Trim();
        }

        /// <summary>
        /// Formata CPF (apenas números) para o padrão 000.000.000-00
        /// </summary>
        public static string Formatar(string cpf)
        {
            cpf = RemoverFormatacao(cpf);

            if (cpf.Length != 11)
                return cpf;

            return string.Format("{0}.{1}.{2}-{3}", 
                cpf.Substring(0, 3), 
                cpf.Substring(3, 3), 
                cpf.Substring(6, 3), 
                cpf.Substring(9, 2));
        }
    }
}
