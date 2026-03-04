
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Data;
using MarketPlaceApi.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceApi.Data.Repositories
{
	public class ClientRepository : IClientRepository
	{
		private readonly MarketplaceDbContext _context;

		public ClientRepository(MarketplaceDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Client client)
		{
			
			await _context.Clients.AddAsync(client);
		}

		public async Task<Client?> GetByEmailAsync(string email)
		{
			var client =  await _context.Clients
			.AsNoTracking()
			.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

			return client;
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
