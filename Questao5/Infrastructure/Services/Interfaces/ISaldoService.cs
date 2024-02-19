using Questao5.Application.Commands.Responses;

namespace Questao5.Infrastructure.Services.Interfaces
{
    public interface ISaldoService
    {
        Task<SaldoResponseDto> GetSaldo(string idConta);
    }
}
