using Microsoft.AspNetCore.Mvc;
using UltraconTesteAPI.Models;
using UltraconTesteAPI.Service;

namespace UltraconTesteAPI.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly LoanService _loanService;

        public LoanController(LoanService loanService)  
        {
            _loanService = loanService;
        }

        [HttpPost("simulate")]
        public IActionResult Simulate([FromBody] LoanRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Parâmetros inválidos");

            try
            {
                // Chamando o serviço para criar a proposta e obter a simulação
                var response = _loanService.SimulateLoan(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Erro ao simular empréstimo");
                return StatusCode(500, "Erro interno no servidor");
            }
        }


    }

}
