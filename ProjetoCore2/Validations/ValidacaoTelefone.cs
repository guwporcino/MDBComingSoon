namespace ProjetoCore2.Validations
{
    public class ValidacaoTelefone
    {
        public static bool Validate(string celular)
        {
            // Remover caracteres não numéricos do número de celular
            var digitsOnly = new string(celular.Where(char.IsDigit).ToArray());

            // Verificar se o número de celular tem o tamanho esperado
            if (digitsOnly.Length != 11)
            {
                return false;
            }

            // Verificar se o número começa com "9" (código de celular no Brasil)
            if (digitsOnly[2] != '9')
            {
                return false;
            }

            return true;
        }
    }
}
