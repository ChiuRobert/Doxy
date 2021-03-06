﻿using System.Collections.Generic;
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
    internal class DictionaryRepository : IDictionaryRepository
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(DictionaryRepository), FileService.GetLogPath());

        [UseProperty]
        private IBaseWordRepository baseWordRepository;

        private IBaseWordRepository BaseWordRepository =>
            baseWordRepository ??
            (baseWordRepository = RepositoryFactory.GetRepository<IBaseWordRepository>());

        public Dictionary GetById(int id)
        {
            Dictionary result = null;
            string selectById = new Query(Const.SCHEMA, Const.DICTIONARY_TABLE).Select().
                Where().
                Column(Const.ID).Equal().Value(id).
                Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectById);

            if (reader != null)
            {
                while (reader.Read())
                {
                    result = new Dictionary
                    {
                        Id = reader.GetInt32(0),
                        BaseWord = BaseWordRepository.GetById(reader.GetInt32(1)),
                        TranslatedWord = BaseWordRepository.GetById(reader.GetInt32(2))
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

        public Dictionary GetByBaseTranslated(BaseWord baseWord, BaseWord translatedWord)
        {
            Dictionary result = null;
            string selectByWords = new Query(Const.SCHEMA, Const.DICTIONARY_TABLE).Select().
                Where().
                Column(Const.DICTIONARY_BASEWORD).Equal().Value(baseWord.Id).
                And().
                Column(Const.DICTIONARY_TRANSLATEDWORD).Equal().Value(translatedWord.Id).
                Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectByWords);

            if (reader != null)
            {
                while (reader.Read())
                {
                    result = new Dictionary 
                    {
                        Id = reader.GetInt32(0), 
                        BaseWord = BaseWordRepository.GetById(reader.GetInt32(1)),
                        TranslatedWord = BaseWordRepository.GetById(reader.GetInt32(2))
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

        public List<Dictionary> GetAll()
        {
            List<Dictionary> dictionaries = new List<Dictionary>();
            string selectAll = new Query(Const.SCHEMA, Const.DICTIONARY_TABLE).Select().Execute();

            IDataReader reader = DbContext.INSTANCE.ExecuteCommand(selectAll);

            if (reader != null)
            {
                while (reader.Read())
                {
                    dictionaries.Add(new Dictionary
                    {
                        Id = reader.GetInt32(0), 
                        BaseWord = BaseWordRepository.GetById(reader.GetInt32(1)),
                        TranslatedWord = BaseWordRepository.GetById(reader.GetInt32(2))
                    });
                }

                LOGGER.Log(Level.FINE, "Object returned",
                    new Param {Name = nameof(dictionaries), Value = string.Join("\n", dictionaries)});
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "The reader was null");
            }

            return dictionaries;
        }

        public void Persist(Dictionary entity)
        {
            string persistEntity = new Query(Const.SCHEMA, Const.DICTIONARY_TABLE).Insert().
                Column(Const.DICTIONARY_BASEWORD).Column(Const.DICTIONARY_TRANSLATEDWORD).
                Values().
                Value(entity.BaseWord.Id).Value(entity.TranslatedWord.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(persistEntity);
        }

        public void Merge(Dictionary entity)
        {
            string mergeEntity = new Query(Const.SCHEMA, Const.DICTIONARY_TABLE).Update().
                Column(Const.DICTIONARY_BASEWORD).Value(entity.BaseWord.Id).
                Column(Const.DICTIONARY_TRANSLATEDWORD).Value(entity.TranslatedWord.Id).
                Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(mergeEntity);
        }

        public void Delete(Dictionary entity)
        {
            string deleteEntity = new Query(Const.SCHEMA, Const.DICTIONARY_TABLE).Delete()
                .Where().
                Column(Const.ID).Equal().Value(entity.Id).
                Execute();

            DbContext.INSTANCE.ExecuteCommand(deleteEntity);
        }
    }
}