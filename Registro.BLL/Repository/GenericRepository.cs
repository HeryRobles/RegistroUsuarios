﻿using Microsoft.EntityFrameworkCore;
using Registro.BLL.Repository.IRepository;
using Registro.DAL.DBContext;
using System.Linq.Expressions;


namespace Registro.BLL.Repository
{
    public class GenericRepository<TModelo> : IGenericRepository<TModelo> where TModelo : class
    {
        private readonly ApplicationDbContext _dbcontext;
        public GenericRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                TModelo? modelo = await _dbcontext.Set<TModelo>().FirstOrDefaultAsync(filtro);
                return modelo!;
            }
            catch
            {
                throw;
            }

        }

        public async Task<TModelo> Crear(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Add(modelo);
                await _dbcontext.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Update(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Remove(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModelo> query = filtro == null ? _dbcontext.Set<TModelo>() :
                    _dbcontext.Set<TModelo>().Where(filtro);
                return query;
            }
            catch
            {
                throw;
            }
        }

    }
}
