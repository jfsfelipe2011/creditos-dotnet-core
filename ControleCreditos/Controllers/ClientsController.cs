using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleCreditos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ControleCreditos.Models;
using ControleCreditos.Models.ViewModels;
using System.Globalization;

namespace ControleCreditos.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ClientService _clientService;
        private readonly CreditService _creditService;
        private readonly ExtractService _extractService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;

        public ClientsController(ClientService clientService, CreditService creditService, ExtractService extractService,  IHttpContextAccessor httpContextAccessor)
        {
            _clientService       = clientService;
            _creditService       = creditService;
            _extractService      = extractService;
            _httpContextAccessor = httpContextAccessor;

            userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public IActionResult Index()
        {
            var list = _clientService.FindByUserId(userId);

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            client.UserId = userId;

            _clientService.Insert(client);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
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

            return View(cliente);
        }

        public IActionResult Edit(int? id)
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

            ClientFormViewModel viewModel = new ClientFormViewModel { Client = cliente };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int? id, ClientFormViewModel viewModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (viewModel.Client.Id == id)
            {
                _clientService.Update(viewModel.Client);
            }

            return RedirectToAction("Index");
        }

        [Route("Clients/{id:int}/Credits")]
        public IActionResult Credits(int? id)
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

            if (cliente.UserId != userId)
            {
                return NotFound();
            }

            List<Credit> credits = _creditService.FindValidCreditsByClient(id);
            int totalBalance = _creditService.GetTotalBalance(id);
            ClientFormViewModel viewModel = new ClientFormViewModel { Client = cliente, Credits = credits, TotalBalance = totalBalance};

            return View(viewModel);
        }

        [Route("Clients/{id:int}/Extracts")]
        public IActionResult Extracts(int? id)
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

            if (cliente.UserId != userId)
            {
                return NotFound();
            }

            List<Extract> extracts = _extractService.FindExtractsByClient(id);
            ClientFormViewModel viewModel = new ClientFormViewModel { Client = cliente, Extracts = extracts };

            CultureInfo daDK = CultureInfo.CreateSpecificCulture("da-DK");

            ViewData["TotalBalance"] = _creditService.GetTotalBalance(id).ToString("00.00", daDK);

            return View(viewModel);
        }
    }
}