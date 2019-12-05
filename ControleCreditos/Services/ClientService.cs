using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleCreditos.Data;
using ControleCreditos.Models;
using ControleCreditos.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ControleCreditos.Services
{
    public class ClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Client> FindAll()
        {
            return _context.Client.ToList();
        }

        public void Insert(Client client)
        {
            _context.Add(client);

            _context.SaveChanges();
        }

        public List<Client> FindByUserId(string userId)
        {
            return _context.Client.Where(cl => cl.UserId == userId).ToList();
        }

        public Client FindById(int? id)
        {
            return _context.Client.FirstOrDefault(cl => cl.Id == id);
        }

        public void Update(Client cliente)
        {
            if (!_context.Client.Any(cl => cl.Id == cliente.Id))
            {
                throw new NotFoundException("Id não encontrado");
            }

            try
            {
                _context.Update(cliente);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
