using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleCreditos.Models.ViewModels
{
    public class ClientFormViewModel
    {
        public Client Client { get; set; }
        public ICollection<Credit> Credits { get; set; }
        public ICollection<Extract> Extracts { get; set; }
        public int TotalBalance { get; set; }
    }
}
