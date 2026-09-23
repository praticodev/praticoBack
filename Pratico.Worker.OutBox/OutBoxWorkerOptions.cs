namespace Pratico.Worker.OutBox
{
    public class OutBoxWorkerOptions
    {
        public bool Habilitado { get; set; } = true;
        public int IntervaloSegundos { get; set; } = 30;
        public int TamanhoLote { get; set; } = 10;
        public int MaximoTentativas { get; set; } = 5;
    }
}
