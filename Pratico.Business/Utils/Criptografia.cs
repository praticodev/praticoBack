using System;

namespace Pratico.Business.Utils
{
    public static class Criptografia
    {
        private const uint Chave = 0x5A17C3E9;

        public static string GerarHashCodCondominio(int valor)
        {
            var codificado = unchecked((uint)valor) ^ Chave;
            return InsereMascara(codificado).PadLeft(10, '0');
        }

        public static int RecuperarHashCodCondominio(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Valor inválido.", nameof(valor));

            var codificado = RemoveMascara(valor.Trim().TrimStart('0'));
            var original = codificado ^ Chave;

            return unchecked((int)original);
        }

        private static string InsereMascara(uint valor)
        {
            const string caracteres = "0123456789abcdefghijklmnopqrstuvwxyz";

            if (valor == 0)
                return "0";

            var resultado = string.Empty;

            while (valor > 0)
            {
                var resto = (int)(valor % 36);
                resultado = caracteres[resto] + resultado;
                valor /= 36;
            }

            return resultado;
        }

        private static uint RemoveMascara(string valor)
        {
            const string caracteres = "0123456789abcdefghijklmnopqrstuvwxyz";

            if (string.IsNullOrWhiteSpace(valor))
                return 0;

            uint resultado = 0;

            foreach (var c in valor.ToLowerInvariant())
            {
                var indice = caracteres.IndexOf(c);
                if (indice < 0)
                    throw new FormatException("Valor informado não é um código válido.");

                resultado = checked((resultado * 36) + (uint)indice);
            }

            return resultado;
        }
    }
}
