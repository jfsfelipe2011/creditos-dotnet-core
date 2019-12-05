using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControleCreditos.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public string Name { get; set; }

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "Documento é obrigatório")]
        [StringLength(14, ErrorMessage = "Documento deve ter no máximo 14 caracteres")]
        public string Document { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        public string Email { get; set; }

        [Display(Name = "Telefone")]
        [Phone(ErrorMessage = "Informe um telefone válido")]
        public int Phone { get; set; }

        public string UserId { get; set; }

        public ICollection<Credit> Credits { get; set; } = new List<Credit>();

        public ICollection<Extract> Extracts { get; set; } = new List<Extract>();

        public Client()
        {
        }

        public Client(string document, string email, int phone, string userId)
        {
            Document = document;
            Email = email;
            Phone = phone;
            UserId = userId;
        }

        public void AddCredit(Credit credit)
        {
            Credits.Add(credit);
        }

        public void RemoveCredit(Credit credit)
        {
            Credits.Remove(credit);
        }

        public void AddExtract(Extract extract)
        {
            Extracts.Add(extract);
        }

        public void RemoveExtract(Extract extract)
        {
            Extracts.Add(extract);
        }

        public IEnumerable<Credit> GetValidCredits()
        {
            DateTime hoje = new DateTime();

            return Credits.Where(cr => cr.Validity >= hoje && cr.Balance > 0)
                .OrderBy(cr => cr.Validity);
        }

        public IEnumerable<Extract> GetExtracts()
        {
            return Extracts.OrderBy(ex => ex.Date);
        }

        public int GetTotalBalance()
        {
            DateTime hoje = new DateTime();

            return Credits.Where(cr => cr.Validity >= hoje)
                .Sum(cr => cr.Balance);
        }


        public IEnumerable<Credit> GetReversibleCredits()
        {
            DateTime hoje = new DateTime();

            return Credits.Where(cr => cr.Validity >= hoje)
                .OrderByDescending(cr => cr.Validity);
        }

        public int GetQuantityCredits()
        {
            DateTime hoje = new DateTime();

            return Credits.Where(cr => cr.Validity >= hoje).Count();
        }

        public int getQuantityActiveCredits()
        {
            DateTime hoje = new DateTime();

            return Credits.Where(cr => cr.Validity >= hoje && cr.Balance > 0).Count();
        }
    }
}
