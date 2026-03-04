
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Repositories.Interfaces
{
	public interface IClientRepository
	{
		Task AddAsync(Client client);
		Task<Client?> GetByEmailAsync(string email);
		Task SaveChangesAsync();
	}
}
