using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControleCreditos.Models
{
    public class Extract
    {
        public int Id { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Tipo é obrigatório")]
        public ExtractType Type { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Descrição é obrigatório")]
        public string Description { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data é obrigatória")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Créditos")]
        [Required(ErrorMessage = "Créditos é obrigatório")]
        public int Credits { get; set; }

        [Display(Name = "Operação")]
        [Required(ErrorMessage = "Operação é obrigatório")]
        public Operation Operation { get; set; }

        [Display(Name = "Saldo")]
        [Required(ErrorMessage = "Saldo é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public int Balance { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public Extract()
        {
        }

        public Extract(ExtractType type, string description, DateTime date, int credits, Operation operation, int balance, int clientId)
        {
            Type = type;
            Description = description;
            Date = date;
            Credits = credits;
            Operation = operation;
            Balance = balance;
            ClientId = clientId;
        }
    }
}
