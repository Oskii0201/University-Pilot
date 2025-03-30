using Microsoft.EntityFrameworkCore;

namespace UniversityPilot.DAL.Areas.Shared
{
    internal class Repository<T> : IRepository<T> where T : class, IModelBase
    {
        protected readonly UniversityPilotContext _context;

        public Repository(UniversityPilotContext context)
        {
            _context = context;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public ICollection<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public T? Get(int id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public async Task<T> AddAsync(T model)
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public T Add(T model)
        {
            _context.Add(model);
            _context.SaveChanges();
            return model;
        }

        public async Task<ICollection<T>> AddRangeAsync(ICollection<T> model)
        {
            await _context.AddRangeAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public ICollection<T> AddRange(ICollection<T> model)
        {
            _context.AddRange(model);
            _context.SaveChanges();
            return model;
        }

        public async Task UpdateAsync(T model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public void Update(T model)
        {
            _context.Update(model);
            _context.SaveChanges();
        }

        public async Task UpdateRangeAsync(ICollection<T> model)
        {
            _context.UpdateRange(model);
            await _context.SaveChangesAsync();
        }

        public void UpdateRange(ICollection<T> model)
        {
            _context.UpdateRange(model);
            _context.SaveChanges();
        }

        public virtual async Task DeleteAsync(T model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }

        public void Delete(T model)
        {
            _context.Remove(model);
            _context.SaveChanges();
        }

        public bool Exist(int id)
        {
            return _context.Set<T>().Any(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}