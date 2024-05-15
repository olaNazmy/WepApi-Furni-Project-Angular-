using Microsoft.EntityFrameworkCore;
using RealApiData.Models;
using System;

namespace RealApiData.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        databaseContext _context;
        public GenericRepository(databaseContext context)
        {
            _context = context;
        }

        //all needed operations
        //1-get all
        public List<TEntity> getall()
        {
            return _context.Set<TEntity>().ToList();
        }

        //2- get by id
        public void deletEntity(int id)
        {
            TEntity entity = _context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
            }

        }

        //3-add 
        public void addEntity(TEntity t)
        {
            _context.Set<TEntity>().Add(t);
        }

        //4- edit
        public void EditEntity(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlException && sqlException.Number == 547)
                {
                    throw new InvalidOperationException("Updating this entity violates a foreign key constraint.");
                }
                else
                {
                    throw;
                }
            }
        }

        //5- save
        public void Save()
        {
            _context.SaveChanges();
        }

        //6- get by id
        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }




    }
}
