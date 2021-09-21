using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public List<Owner> GetAllOwners()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT o.Id 'ownerId',
	                                           o.Name 'ownerName',
	                                           o.Email,
	                                           o.Address,
	                                           o.Phone,
	                                           n.Id 'neighborhoodId',
	                                           n.Name 'neighborhoodName'
                                        FROM Owner o
                                        LEFT JOIN Neighborhood n ON o.NeighborhoodId = n.Id;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Owner> owners = new List<Owner>();

                        while (reader.Read())
                        {
                            Owner owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ownerId")),
                                Name = reader.GetString(reader.GetOrdinal("ownerName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                Neighborhood = new Neighborhood()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("neighborhoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("neighborhoodName"))
                                }
                            };

                            owners.Add(owner);
                        }

                        return owners;
                    }
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT o.Id 'ownerId',
	                                           o.Name 'ownerName',
	                                           o.Email,
	                                           o.Address,
	                                           o.Phone,
	                                           n.Id 'neighborhoodId',
	                                           n.Name 'neighborhoodName'
                                        FROM Owner o
                                        LEFT JOIN Neighborhood n ON o.NeighborhoodId = n.Id
                                        WHERE ownerId = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Owner owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ownerId")),
                                Name = reader.GetString(reader.GetOrdinal("ownerName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                Neighborhood = new Neighborhood()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("neighborhoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("neighborhoodName"))
                                }
                            };

                            return owner;
                        }

                        return null;
                    }
                }
            }
        }
    }
}
