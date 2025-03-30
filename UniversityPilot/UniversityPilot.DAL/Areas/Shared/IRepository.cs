namespace UniversityPilot.DAL.Areas.Shared
{
    public interface IRepository<Model>
    {
        public Task<ICollection<Model>> GetAllAsync();

        public ICollection<Model> GetAll();

        public Task<Model?> GetAsync(int id);

        public Model? Get(int id);

        public Task<Model> AddAsync(Model model);

        public Model Add(Model model);

        public Task<ICollection<Model>> AddRangeAsync(ICollection<Model> model);

        public ICollection<Model> AddRange(ICollection<Model> model);

        public Task UpdateAsync(Model model);

        public void Update(Model model);

        public Task UpdateRangeAsync(ICollection<Model> model);

        public void UpdateRange(ICollection<Model> model);

        public Task DeleteAsync(Model model);

        public void Delete(Model model);

        public bool Exist(int id);

        public Task SaveChangesAsync();
    }
}