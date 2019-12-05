using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControleCreditos.Models
{
    public class Credit
    {
        public int Id { get; set; }

        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Required(ErrorMessage = "Valor é obrigatório")]
        public int Value { get; set; }

        [Display(Name = "Validade")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Validade é obrigatória")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Validity { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Descrição é obrigatório")]
        public string Description { get; set; }

        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Required(ErrorMessage = "Saldo é obrigatório")]
        public int Balance { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public Credit()
        {
        }

        public Credit(int value, DateTime validity, string description)
        {
            Value = value;
            Validity = validity;
            Description = description;
        }
    }
}
