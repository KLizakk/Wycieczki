//using Wycieczki.Data;
//using Wycieczki.Interfaces;

//namespace Wycieczki.Repository
//{
//    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
//    {
//        private readonly WycieczkaContext _context;
//        BaseRepository(IBaseRepository<T> context)
//        {
//            _context = context;
//        }
//        public Task<T> Add(T entity)
//        {

//            return _context.Set<T>().AddAsync(entity);
//        }

//        public Task<T> Delete(int id)
//        {
//            return _context.Delete(id);
//        }

//        public Task<IEnumerable<T>> GetAll()
//        {
//            return ;
//        }

//        public Task<T> GetById(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<T> Update(int id, T entity)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
