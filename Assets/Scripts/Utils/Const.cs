namespace Utils
{
    public static class Const
    {
        #region Folders

        public static string STREAMING_ASSETS;
        
        private const string SQL_FOLDER = "SQL/";
        private const string LOGS_FOLDER = "Logs/";

        #endregion

        #region SQL

        public const string DATABASE_NAME = SQL_FOLDER + "doxydb.db";
        public const string TEST_DATABASE_NAME = SQL_FOLDER + "testdoxydb.db";
        public const string CREATE_TABLES_SCRIPT = SQL_FOLDER + "createTables.txt";
        public const string ADD_TEST_DATA = SQL_FOLDER + "addTestData.txt";

        public const string SCHEMA = "main";
        public const string ID = "id";

        #endregion

        #region LANGUAGE

        public const string LANGUAGE_TABLE = "language";
        public const string LANGUAGE_NAME = "name";

        #endregion
        
        #region BASEWORD

        public const string BASEWORD_TABLE = "baseword";
        public const string BASEWORD_WORD = "word";
        public const string BASEWORD_DIALECT = "dialect";

        #endregion

        #region DIALECT

        public const string DIALECT_TABLE = "dialect";
        public const string DIALECT_NAME = "name";
        public const string DIALECT_LANGUAGE = "language";

        #endregion

        #region DICTIONARY

        public const string DICTIONARY_TABLE = "dictionary";
        public const string DICTIONARY_BASEWORD = "baseWord";
        public const string DICTIONARY_TRANSLATEDWORD = "translatedWord";
        
        #endregion

        #region Logs

        public const string LOG_FILE_NAME = LOGS_FOLDER + "logss.txt";
        public const string TEST_LOG_FILE_NAME = LOGS_FOLDER + "testlogs.txt";

        #endregion
    }
}
