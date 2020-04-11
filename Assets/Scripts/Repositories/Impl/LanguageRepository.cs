﻿using System.Data;
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

            while (reader.Read())
            {
                result = new Language {Id = reader.GetInt32(0), Name = reader.GetString(1)};
            }
            
            LOGGER.Log(Level.FINE, "Object returned", new Param {Name = nameof(result), Value = result});

            return result;
        }

        public void Persist(Language entity)
        {
            string insertEntity = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Insert().
                Column(Const.LANGUAGE_NAME).
                Values().
                Value(entity.Name).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(insertEntity);
        }

        public void Merge(Language entity)
        {
            string updateEntity = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Update().
                Column(Const.LANGUAGE_NAME).Value(entity.Name).
                Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(updateEntity);
        }

        public void Delete(Language entity)
        {
            string deleteEntity = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Delete().
                Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(deleteEntity);
        }
    }
}
