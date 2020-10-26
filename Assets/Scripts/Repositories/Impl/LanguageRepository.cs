using System.Collections.Generic;
using System.Data;
using DataBase;
using Entities;
using SbLogger;
using SbLogger.Levels;
using ScotchBoardSQL;
using Utils;

namespace Repositories.Impl
{
    internal class LanguageRepository : ILanguageRepository
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(LanguageRepository), FileService.GetLogPath());

        public Language GetById(int id)
        {
            Language result = null;
            string selectById = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Select().
                Where().
                Column(Const.ID).Equal().Value(id).
                Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectById);

            if (reader != null)
            {
                while (reader.Read())
                {
                    result = new Language {Id = reader.GetInt32(0), Name = reader.GetString(1)};
                }

                LOGGER.Log(Level.FINE, "Object returned", new Param {Name = nameof(result), Value = result});
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "The reader was null");
            }

            return result;
        }
        
        public Language GetByName(string name)
        {
            Language result = null;
            string selectByName = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Select().
                Where().
                Column(Const.LANGUAGE_NAME).Equal().Value(name).
                Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectByName);

            if (reader != null)
            {
                while (reader.Read())
                {
                    result = new Language {Id = reader.GetInt32(0), Name = reader.GetString(1)};
                }

                LOGGER.Log(Level.FINE, "Object returned", new Param {Name = nameof(result), Value = result});
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "The reader was null");
            }

            return result;
        }

        public List<Language> GetAll()
        {
            List<Language> languages = new List<Language>();
            string selectAll = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Select().Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectAll);

            if (reader != null)
            {
                while (reader.Read())
                {
                    languages.Add(new Language {Id = reader.GetInt32(0), Name = reader.GetString(1)});
                }

                LOGGER.Log(Level.FINE, "Object returned",
                    new Param {Name = nameof(languages), Value = string.Join("\n", languages)});
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "The reader was null");
            }

            return languages;
        }

        public void Persist(Language entity)
        {
            LOGGER.Log(Level.INFO, "Creating language", new Param {Name = nameof(entity), Value = entity});
            
            string insertEntity = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Insert().
                Column(Const.LANGUAGE_NAME).
                Values().
                Value(entity.Name).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(insertEntity);
        }

        public void Merge(Language entity)
        {
            LOGGER.Log(Level.INFO, "Updating language", new Param {Name = nameof(entity), Value = entity});
            
            string updateEntity = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Update().
                Column(Const.LANGUAGE_NAME).Value(entity.Name).
                Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(updateEntity);
        }

        public void Delete(Language entity)
        {
            LOGGER.Log(Level.INFO, "Deleting language", new Param {Name = nameof(entity), Value = entity});
            
            string deleteEntity = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Delete().
                Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(deleteEntity);
        }
    }
}
