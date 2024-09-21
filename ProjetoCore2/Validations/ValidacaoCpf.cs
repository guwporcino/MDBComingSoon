namespace ProjetoCore2.Validations
{
    public class ValidacaoCpf
    {
        public static bool ValidarCPF(string cpf)
        {
            // Remover caracteres não numéricos do CPF
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verificar se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verificar se todos os dígitos são iguais (caso contrário, não é um CPF válido)
            bool todosDigitosIguais = cpf.Distinct().Count() == 1;
            if (todosDigitosIguais)
                return false;

            // Calcular o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int primeiroDigitoVerificador = (soma * 10) % 11;
            if (primeiroDigitoVerificador == 10)
                primeiroDigitoVerificador = 0;

            // Calcular o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            int segundoDigitoVerificador = (soma * 10) % 11;
            if (segundoDigitoVerificador == 10)
                segundoDigitoVerificador = 0;

            // Verificar se os dígitos verificadores são iguais aos dígitos informados no CPF
            return primeiroDigitoVerificador == int.Parse(cpf[9].ToString()) &&
                   segundoDigitoVerificador == int.Parse(cpf[10].ToString());
        }
    }
}
