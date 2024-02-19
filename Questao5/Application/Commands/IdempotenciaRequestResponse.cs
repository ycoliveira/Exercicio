namespace Questao5.Application.Commands
{
    public class IdempotenciaRequestResponse
    {
        public string chave_idempotencia { get; set; }
        public string requisicao { get; set; }
        public string resultado { get; set; }
    }
}
