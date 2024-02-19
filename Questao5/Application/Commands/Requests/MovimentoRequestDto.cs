namespace Questao5.Application.Commands.Requests
{
    public class MovimentoRequestDto
    {
        public string RequestId { get; set; }
        public string IdContaCorrente { get; set; }
        public double Valor { get; set; }
        public char TipoMovimento { get; set; }
    }
}
