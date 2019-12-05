using ControleCreditos.Data;
using ControleCreditos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleCreditos.Services
{
    public class ExtractService
    {
        private readonly ApplicationDbContext _context;

        public ExtractService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Insert(Extract extract)
        {
            _context.Extract.Add(extract);
            _context.SaveChanges();
        }

        public List<Extract> FindExtractsByClient(int? idClient)
        {
            return _context.Extract.Where(ex => ex.ClientId == idClient)
                .OrderBy(ex => ex.Date).ToList();
        }
    }
}
