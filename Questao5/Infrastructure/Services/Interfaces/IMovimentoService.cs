using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Interfaces
{
    public interface IMovimentoService
    {
        Task<string> GerarMovimentoAsync(MovimentoRequestDto requestDto);
    }
}
