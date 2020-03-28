namespace Doxy.Utils
{
    public static class Const
    {
        #region Folders

        private static string SQL_FOLDER = "SQL/";
        private static string LOGS_FOLDER = "Logs/";

        #endregion

        #region SQL

        public static string DATABASE_NAME = SQL_FOLDER + "doxydb.db";
        public static string CREATE_TABLES_SCRIPT = SQL_FOLDER + "createTables.txt";

        #endregion

        #region Logs

        public static string LOG_FILE_NAME = LOGS_FOLDER + "logs.txt";

        #endregion
    }
}
