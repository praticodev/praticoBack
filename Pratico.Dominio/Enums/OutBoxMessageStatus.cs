namespace Pratico.Dominio.Enums
{
    public static class OutBoxMessageStatus
    {
        public const string Pendente = "Pendente";
        public const string Processando = "Processando";
        public const string Processado = "Processado";
        public const string Falha = "Falha";
    }
}
