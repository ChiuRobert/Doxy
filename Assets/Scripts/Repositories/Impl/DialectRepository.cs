using System.Collections.Generic;
using System.Data;
using DataBase;
using Entities;
using SbLogger;
using SbLogger.Levels;
using ScotchBoardSQL;
using Utils;
using Utils.Attributes;

namespace Repositories.Impl
{
    internal class DialectRepository : IDialectRepository
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(DialectRepository), FileService.GetLogPath());

        [UseProperty]
        private ILanguageRepository languageRepository;

        private ILanguageRepository LanguageRepository =>
            languageRepository ??
            (languageRepository = RepositoryFactory.GetRepository<ILanguageRepository>());

        public Dialect GetById(int id)
        {
            Dialect result = null;
            string selectById = new Query(Const.SCHEMA, Const.DIALECT_TABLE).Select().
                Where().
                Column(Const.ID).Equal().Value(id).
                Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectById);

            if (reader != null)
            {
                while (reader.Read())
                {
                    result = new Dialect
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Language = LanguageRepository.GetById(reader.GetInt32(2))
                    };
                }

                LOGGER.Log(Level.FINE, "Object returned", new Param {Name = nameof(result), Value = result});
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "The reader was null");
            }

            return result;
        }

        public Dialect GetByNameLanguage(string name, Language language)
        {
            Dialect result = null;
            string selectById = new Query(Const.SCHEMA, Const.DIALECT_TABLE).Select().
                Where().
                Column(Const.DIALECT_NAME).Equal().Value(name).
                And().
                Column(Const.DIALECT_LANGUAGE).Equal().Value(language.Id).
                Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectById);

            if (reader != null)
            {
                while (reader.Read())
                {
                    result = new Dialect 
                    {
                        Id = reader.GetInt32(0), 
                        Name = reader.GetString(1),
                        Language = LanguageRepository.GetById(reader.GetInt32(2))
                    };
                }

                LOGGER.Log(Level.FINE, "Object returned", new Param {Name = nameof(result), Value = result});
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "The reader was null");
            }

            return result;
        }

        public List<Dialect> GetAll()
        {
            List<Dialect> dialects = new List<Dialect>();
            string selectAll = new Query(Const.SCHEMA, Const.DIALECT_TABLE).Select().Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectAll);

            if (reader != null)
            {
                while (reader.Read())
                {
                    dialects.Add(new Dialect
                    {
                        Id = reader.GetInt32(0), 
                        Name = reader.GetString(1),
                        Language = LanguageRepository.GetById(reader.GetInt32(2))
                    });
                }

                LOGGER.Log(Level.FINE, "Object returned",
                    new Param {Name = nameof(dialects), Value = string.Join("\n", dialects)});
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "The reader was null");
            }

            return dialects;
        }

        public void Persist(Dialect entity)
        {
            string persistEntity = new Query(Const.SCHEMA, Const.DIALECT_TABLE).Insert().
                Column(Const.DIALECT_NAME).Column(Const.DIALECT_LANGUAGE).
                Values().
                Value(entity.Name).Value(entity.Language.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(persistEntity);
        }

        public void Merge(Dialect entity)
        {
            string mergeEntity = new Query(Const.SCHEMA, Const.DIALECT_TABLE).Update().
                Column(Const.DIALECT_NAME).Value(entity.Name).
                Column(Const.DIALECT_LANGUAGE).Value(entity.Language.Id).
                Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(mergeEntity);
        }

        public void Delete(Dialect entity)
        {
            string deleteEntity = new Query(Const.SCHEMA, Const.DIALECT_TABLE).Delete()
                .Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(deleteEntity);
        }
    }
}