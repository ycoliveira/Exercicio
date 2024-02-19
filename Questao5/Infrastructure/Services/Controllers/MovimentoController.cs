using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Services.Interfaces;
using System.Net;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/movimentos")]
    public class MovimentoController : ControllerBase
    {
        private readonly IMovimentoService _movimentoService;
        private readonly ISaldoService _saldoService;

        public MovimentoController(IMovimentoService movimentoService, ISaldoService saldoService)
        {
            _movimentoService = movimentoService;
            _saldoService = saldoService;
        }

        /// <summary>
        /// Gera uma movimentação para uma conta.
        /// </summary>
        /// <param name="movimentoRequestDto">O request do movimento.</param>
        /// <returns>O ID do movimento.</returns>
        /// <response code="200">Retorna o ID do movimento gerado.</response>
        /// <response code="400">Se o request é invalido ou se tem erro de validação.</response>
        [HttpPost]
        public async Task<IActionResult> GerarMovimento([FromBody] MovimentoRequestDto movimentoRequestDto)
        {
            try
            {
                var movimentoId = await _movimentoService.GerarMovimentoAsync(movimentoRequestDto);

                var responseDto = new MovimentoResponseDto
                {
                    MovimentoId = movimentoId,
                    Mensagem = "Movimento processado com successo."
                };

                return Ok(responseDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new MovimentoResponseDto { Mensagem = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Consulta o saldo de uma conta.
        /// </summary>
        /// <param name="idConta">O id da conta ser consultada</param>
        /// <returns>Retorna o saldo da conta.</returns>
        /// <response code="200">Retorna o saldo da conta.</response>
        /// <response code="400">Se o request é invalido ou se tem erro de validação.</response>
        [HttpGet("{idConta}/saldo")]
        public async Task<IActionResult> GetSaldo(string idConta)
        {
            try
            {
                var saldoResponse = await _saldoService.GetSaldo(idConta);
                return Ok(saldoResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new MovimentoResponseDto { Mensagem = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
