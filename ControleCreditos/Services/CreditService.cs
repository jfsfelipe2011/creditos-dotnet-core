using ControleCreditos.Data;
using ControleCreditos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleCreditos.Services
{
    public class CreditService
    {
        private readonly ApplicationDbContext _context;
        private readonly ExtractService _extractService;

        public CreditService(ApplicationDbContext context, ExtractService extractService)
        {
            _context        = context;
            _extractService = extractService;
        }

        public List<Credit> FindValidCreditsByClient(int? idClient)
        {
            DateTime hoje = new DateTime();

            return _context.Credit.Where(cr => cr.Validity >= hoje && cr.Balance > 0 && cr.ClientId == idClient)
                .OrderBy(cr => cr.Validity).ToList();
        }

        public List<Credit> FindReversibleCredits(int? idClient)
        {
            DateTime hoje = new DateTime();

            return _context.Credit.Where(cr => cr.Validity >= hoje && cr.ClientId == idClient)
                .OrderByDescending(cr => cr.Validity).ToList();
        }

        public int GetTotalBalance(int? idClient)
        {
            DateTime hoje = new DateTime();

            return _context.Credit.Where(cr => cr.Validity >= hoje && cr.ClientId == idClient)
                .Sum(cr => cr.Balance);
        }

        public void Recharge(Credit credit, int clientId)
        {
            credit.Balance  = credit.Value;
            credit.ClientId = clientId;

            _context.Credit.Add(credit);
            _context.SaveChanges();

            DateTime hoje = new DateTime();

            Extract extract = new Extract(
                ExtractType.RECARGA, credit.Description, hoje,
                credit.Value, Operation.CREDITO, this.GetTotalBalance(clientId), clientId
            );

            _extractService.Insert(extract);
        }

        public void Remove(Credit credit, int clientId)
        {
            List<Credit> validCredits = this.FindValidCreditsByClient(clientId);
            int quantityCredits = credit.Value;

            foreach (Credit validCredit in validCredits)
            {
                int subtract = validCredit.Balance - quantityCredits;

                if (subtract >= 0)
                {
                    validCredit.Balance = subtract;
                    _context.Credit.Update(validCredit);
                    _context.SaveChanges();

                    break;
                }

                validCredit.Balance = 0;
                _context.Credit.Update(validCredit);
                _context.SaveChanges();

                quantityCredits = Math.Abs(subtract);
            }

            DateTime hoje = new DateTime();

            Extract extract = new Extract(
                ExtractType.REMOCAO, credit.Description, hoje,
                credit.Value, Operation.DEBITO, this.GetTotalBalance(clientId), clientId
            );

            _extractService.Insert(extract);
        }

        public void Chargeback(Credit credit, int clientId)
        {
            List<Credit> reversibleCredits = this.FindReversibleCredits(clientId);
            int quantityCredits = credit.Value;

            foreach (Credit reversibleCredit in reversibleCredits)
            {
                if (reversibleCredit.Balance == reversibleCredit.Value)
                {
                    continue;
                }

                int diff = reversibleCredit.Value - reversibleCredit.Balance;

                if (diff < quantityCredits)
                {
                    reversibleCredit.Balance = reversibleCredit.Balance + diff;

                    _context.Credit.Update(reversibleCredit);
                    _context.SaveChanges();

                    quantityCredits = quantityCredits - diff;
                } else
                {
                    reversibleCredit.Balance = reversibleCredit.Balance + quantityCredits;

                    _context.Credit.Update(reversibleCredit);
                    _context.SaveChanges();

                    quantityCredits = 0;
                }

                if (quantityCredits == 0)
                {
                    break;
                }
            }

            DateTime hoje = new DateTime();

            Extract extract = new Extract(
                ExtractType.ESTORNO, credit.Description, hoje,
                credit.Value, Operation.DEBITO, this.GetTotalBalance(clientId), clientId
            );

            _extractService.Insert(extract);
        }
    }
}
