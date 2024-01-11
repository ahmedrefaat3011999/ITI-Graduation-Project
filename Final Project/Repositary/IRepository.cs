namespace Final_Project.Repositary
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetByID(string Id);
        void Add(TEntity entity );
        void Delete(string Id);
        void Update(string Id,TEntity entity);
        void Save();
        //unitofwork  
        //data access layer
        //presention  layer view
        //bu
    }
}
