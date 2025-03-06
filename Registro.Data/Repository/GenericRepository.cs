using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registro.DAL.Repository.IRepository;
using Registro.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Registro.DAL.Repository
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly ApplicationDbContext _dbcontext;
        public GenericRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro)
        {
            try
            {
                TModel modelo = await _dbcontext.Set<TModel>().FirstOrDefaultAsync(filtro);
                return modelo;
            }
            catch
            {
                throw;
            }
        }
        public async Task<TModel> Crear(TModel modelo)
        {
            try
            {
                _dbcontext.Set<TModel>().Add(modelo);
                await _dbcontext.SaveChangesAsync();
                return modelo;

            }
            catch
            {
                throw;
            }
        }
        public async Task<TModel> Editar(TModel modelo)
        {
            try
            {
                _dbcontext.Set<TModel>().Update(modelo);
                await _dbcontext.SaveChangesAsync();
                return modelo;

            }
            catch
            {
                throw;
            }
        }
        public async Task<TModel> Eliminar(TModel modelo)
        {
            try
            {
                _dbcontext.Set<TModel>().Remove(modelo);
                await _dbcontext.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }
        public Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModel> queryModelo = _dbcontext.Set<TModel>();
                if (filtro != null)
                {
                    queryModelo = queryModelo.Where(filtro);
                }
                return Task.FromResult(queryModelo);
            }
            catch
            {
                throw;
            }
        }
    }
}
