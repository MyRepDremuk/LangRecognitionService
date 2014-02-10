using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageRecognitionAPI;
using System.Data.SQLite;
using System.Data;
using System.Configuration;

namespace LanguageRecognitionServiceDAO
{
    public class MemorizationSQLiteDAO : IMemorizationDAO
    {

        private const int IDNOTFOUND = -1;

        public MemorizationSQLiteDAO()
        {
        }


        public bool IsWordsRecognitionResultExist(string currentWord)
        {
            int resultCount = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["connectionSQLite"].ToString();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT COUNT(*) FROM (LangWord inner join Words ON Words.Id = LangWord.word_id) WHERE Words.word = @currWordParam;", connection))
                {
                    SQLiteParameter currWordParam = command.CreateParameter();
                    currWordParam.ParameterName = "currWordParam";
                    currWordParam.Value = currentWord;
                    command.Parameters.Add(currWordParam);

                    resultCount = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return (resultCount > 0);
        }


        public RecognitionResult LoadMemorizedRecognitionResult(string currentString)
        {
            RecognitionResult result = new RecognitionResult(currentString);

            string connectionString = ConfigurationManager.ConnectionStrings["connectionSQLite"].ToString();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT language, probability "

                    + "FROM (LangWord INNER JOIN Languages ON Languages.Id = LangWord.lang_id) "
                    + "WHERE word_id = (SELECT Id FROM Words WHERE word=@currWordParam);", connection))
                {

                    SQLiteParameter currWordParam = command.CreateParameter();
                    currWordParam.ParameterName = "currWordParam";
                    currWordParam.Value = currentString;
                    command.Parameters.Add(currWordParam);

                    DataTable dt = new DataTable();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            result.LangProbability.Add((string)row["language"], Convert.ToDouble(row["probability"], new System.Globalization.CultureInfo("en-US")));
                        }
                    }
                }
            }
            return result;
        }


        public void MemorizeRecognitionResult(RecognitionResult currentResult)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionSQLite"].ToString();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var commandWord = new SQLiteCommand("INSERT INTO Words(word) VALUES(@currWordParam);", connection))
                {
                    SQLiteParameter currWordParam = commandWord.CreateParameter();
                    currWordParam.ParameterName = "currWordParam";
                    currWordParam.Value = currentResult.Word;
                    commandWord.Parameters.Add(currWordParam);

                    commandWord.ExecuteNonQuery();
                }

                foreach (var item in currentResult.LangProbability)
                {
                    using (var commandLang = new SQLiteCommand("SELECT Id FROM Languages WHERE language=@currLangParam;", connection))
                    {
                        int langId = -1;

                        SQLiteParameter currLangParam = commandLang.CreateParameter();
                        currLangParam.ParameterName = "currLangParam";
                        currLangParam.Value = item.Key;
                        commandLang.Parameters.Add(currLangParam);
                        var reader = commandLang.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            langId = Convert.ToInt32(row["Id"]);
                        }

                        reader.Close();

                        if (langId != IDNOTFOUND)
                        {

                            using (var command = new SQLiteCommand("INSERT INTO LangWord(word_id, lang_id, probability) "

                                + "VALUES((SELECT Words.Id FROM Words WHERE word=@currword), "
                                    + "(SELECT Languages.Id FROM Languages WHERE language=@currlang), @currprob);",
                                connection))
                            {

                                var currLang = command.CreateParameter();
                                currLang.ParameterName = "currlang";
                                currLang.Value = item.Key;
                                command.Parameters.Add(currLang);

                                SQLiteParameter currWord = commandLang.CreateParameter();
                                currWord.ParameterName = "currword";
                                currWord.Value = currentResult.Word;
                                command.Parameters.Add(currWord);

                                SQLiteParameter currProb = commandLang.CreateParameter();
                                currProb.ParameterName = "currprob";
                                currProb.Value = item.Value;
                                command.Parameters.Add(currProb);

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
}
