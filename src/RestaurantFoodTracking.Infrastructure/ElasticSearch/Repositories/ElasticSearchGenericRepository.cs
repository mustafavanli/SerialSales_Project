﻿using AutoMapper;
using Nest;
using RestaurantFoodTracking.Application.Interface.ElasticSearch;
using RestaurantFoodTracking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantFoodTracking.Infrastructure.ElasticSearch.Repositories
{
    public abstract class ElasticSearchGenericRepository<Entity> : IElasticSearchGenericRepository<Entity> where Entity : BaseEntity,new()
    {
        private readonly IElasticClient client;
        public abstract string IndexName { get; }

        public ElasticSearchGenericRepository(IElasticClient client)
        {
            this.client = client;

        }
        public async Task<bool> CreateIndexAsync()
        {
            if (!(await client.Indices.ExistsAsync(IndexName)).Exists)
            {
                await client.Indices.CreateAsync(IndexName, c =>
                {
                    c.Map<Entity>(p => p.AutoMap());
                    return c;
                });
            }
            return true;
        }
        public async virtual Task<Entity> AddAsync(Entity entity)
        {
            var response = await client.CreateDocumentAsync(entity);

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return entity;
        }

        public async virtual Task<bool> DeleteAsync(Guid id)
        {
            await client.DeleteAsync<Entity>(new DocumentPath<Entity>(id));
            return true;
        }

        public async virtual Task<Entity> GetByIdAsync(Guid id)
        {
            var response = await client.GetAsync<Entity>(id);
            var document = response.Source;
            return document;
        }

        public async virtual Task<List<Entity>> GetListAsync()
        {
            var response = await client.SearchAsync<Entity>(x => x.Query(q => q.MatchAll()));
            var documents = response.Documents.ToList();
            return documents;
        }

        public async virtual Task<Entity> UpdateAsync(Entity entity)
        {
            var result = await client.UpdateAsync<Entity>(entity.Id, u =>
                        u.Doc(entity).Index(IndexName));
            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
            return entity;
        }
    }
}
