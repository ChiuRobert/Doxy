using System.Data;
using DataBase;
using Entities;
using SbLogger;
using SbLogger.Levels;
using ScotchBoardSQL;
using Utils;

namespace Repositories.Impl
{
    internal class BaseWordRepository : IBaseWordRepository
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(BaseWordRepository), FileService.GetLogPath());

        [UseProperty]
        private IDialectRepository dialectRepository;

        private IDialectRepository DialectRepository =>
            dialectRepository ??
            (dialectRepository = RepositoryFactory.GetRepository<IDialectRepository>());
        
        public BaseWord GetById(int id)
        {
            BaseWord result = null;
            string selectById = new Query(Const.SCHEMA, Const.BASEWORD_TABLE).Select().
                Where().
                Column(Const.ID).Equal().Value(id).
                Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectById);

            while (reader.Read())
            {
                result = new BaseWord()
                {
                    Id = reader.GetInt32(0),
                    Word = reader.GetString(1),
                    Dialect = DialectRepository.GetById(reader.GetInt32(2))
                };
            }

            LOGGER.Log(Level.FINE, "Object returned", new Param {Name = nameof(result), Value = result});
            
            return result;
        }
        
        public void Persist(BaseWord entity)
        {
            string persistEntity = new Query(Const.SCHEMA, Const.BASEWORD_TABLE).Insert().
                Column(Const.BASEWORD_WORD).Column(Const.BASEWORD_DIALECT).
                Values().
                Value(entity.Word).Value(entity.Dialect.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(persistEntity);
        }

        public void Merge(BaseWord entity)
        {
            string mergeEntity = new Query(Const.SCHEMA, Const.BASEWORD_TABLE).Update().
                Column(Const.BASEWORD_WORD).Value(entity.Word).
                Column(Const.BASEWORD_DIALECT).Value(entity.Dialect.Id).
                Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(mergeEntity);
        }

        public void Delete(BaseWord entity)
        {
            string deleteEntity = new Query(Const.SCHEMA, Const.BASEWORD_TABLE).Delete()
                .Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(deleteEntity);
        }
    }
}
