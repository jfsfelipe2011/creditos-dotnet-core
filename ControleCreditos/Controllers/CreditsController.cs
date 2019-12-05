using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleCreditos.Models;
using ControleCreditos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleCreditos.Controllers
{
    [Authorize]
    public class CreditsController : Controller
    {
        private readonly ClientService _clientService;
        private readonly CreditService _creditService;

        public CreditsController(ClientService clientService, CreditService creditService)
        {
            _clientService = clientService;
            _creditService = creditService;
        }

        public IActionResult Recharge(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _clientService.FindById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["IdCliente"] = cliente.Id;

            return View();
        }

        [HttpPost]
        [Route("Credits/Recharge/{clienteId:int}")]
        public IActionResult Recharge(int clienteId, Credit credit)
        {
            var cliente = _clientService.FindById(clienteId);

            if (cliente == null)
            {
                return NotFound();
            }

            _creditService.Recharge(credit, clienteId);

            return RedirectToRoute(new { controller = "Clients", action = "Credits", id = clienteId });
        }

        public IActionResult Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _clientService.FindById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            int totalBalance = _creditService.GetTotalBalance(id);

            if (totalBalance <= 0)
            {
                return RedirectToRoute(new { controller = "Clients", action = "Credits", id = id });
            }

            ViewData["IdCliente"] = cliente.Id;

            return View();
        }

        [HttpPost]
        [Route("Credits/Remove/{clienteId:int}")]
        public IActionResult Remove(int clienteId, Credit credit)
        {
            var cliente = _clientService.FindById(clienteId);

            if (cliente == null)
            {
                return NotFound();
            }

            _creditService.Remove(credit, clienteId);

            return RedirectToRoute(new { controller = "Clients", action = "Credits", id = clienteId });
        }

        public IActionResult Chargeback(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _clientService.FindById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            int totalBalance = _creditService.GetTotalBalance(id);

            if (totalBalance <= 0)
            {
                return RedirectToRoute(new { controller = "Clients", action = "Credits", id = id });
            }

            ViewData["IdCliente"] = cliente.Id;

            return View();
        }

        [HttpPost]
        [Route("Credits/Chargeback/{clienteId:int}")]
        public IActionResult Chargeback(int clienteId, Credit credit)
        {
            var cliente = _clientService.FindById(clienteId);

            if (cliente == null)
            {
                return NotFound();
            }

            _creditService.Chargeback(credit, clienteId);

            return RedirectToRoute(new { controller = "Clients", action = "Credits", id = clienteId });
        }
    }
}