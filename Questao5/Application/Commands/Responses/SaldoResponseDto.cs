namespace Questao5.Application.Commands.Responses
{
    public class SaldoResponseDto
    {
        public int NumeroContaCorrente { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataResponse { get; set; }
        public decimal SaldoAtual { get; set; }
    }
}
