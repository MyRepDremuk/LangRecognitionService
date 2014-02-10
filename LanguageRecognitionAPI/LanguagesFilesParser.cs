using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Configuration;


namespace LanguageRecognitionAPI
{
    
    class LanguagesFilesParser : ILanguagesFilesParser
    {
        public const string XMLFILE = "xml";
        public const string TEXTFILE = "txt";

        private const string TAGNAME = "name";
        private const string TAGNUMBEROFCHARS = "numberOfChars";
        private const string TAGPATHUNIGRAM = "pathUnigram";
        private const string TAGPATHBIGRAM = "pathBigram";
        private const string TAGPATHTRIGRAM = "pathTrigram";
        private const string TAGLANGUAGE = "language";
        private const string TAGLANGUAGES = "languages";

        private const string TAGPAIR = "pair";
        private const string TAGCHARACTER = "character";
        private const string TAGFREQUENCY = "frequency";

        private char[] FILEPARSESEPARATORSARRAY;

        private string _configuratuonFilePath;

        public LanguagesFilesParser()
        {
            _configuratuonFilePath = ConfigurationManager.ConnectionStrings["connectionConfigXML"].ToString();
            FILEPARSESEPARATORSARRAY = new char[] { ' ', '\n', '\r', ';' };
        }

        // Display any warnings or errors. 
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);

        } 


        public List<Language> ParseLangugesFromConfig(string storageType, ref int maxBase)
        {
            List<Language> resultLanguageCollection = new List<Language>();
            
            string currentConfigFilePath = "";

            string currElement = "";

            string currName = "";
            int currNumberOfChars = 0;
            string currUnigramnFilePath = "";
            string currBigarmFilePath = "";
            string currTrigarmFilePath = "";

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // Create the XmlReader object.
            //using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)), settings)
            switch(storageType)
            {
                case TEXTFILE:
                    if (_configuratuonFilePath != string.Empty)
                    {
                        currentConfigFilePath = _configuratuonFilePath;
                    }
                    else
                    {
                        throw new FileNotFoundException();
                    }
                    break;

                case XMLFILE:
                    if (_configuratuonFilePath != string.Empty)
                    {
                        currentConfigFilePath = _configuratuonFilePath;
                    }
                    else
                    {
                        throw new FileNotFoundException();
                    }
                    break;
            }

            using (XmlReader reader = XmlReader.Create(currentConfigFilePath, settings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            currElement = reader.Name;
                            break;

                        case XmlNodeType.Text:
                            switch (currElement)
                            {

                                case TAGNAME:
                                    currName = reader.Value;
                                    break;

                                case TAGNUMBEROFCHARS:
                                    currNumberOfChars = Int32.Parse(reader.Value);
                                    if (currNumberOfChars > maxBase)
                                        maxBase = currNumberOfChars;
                                    break;

                                case TAGPATHUNIGRAM:
                                    currUnigramnFilePath = reader.Value;
                                    break;

                                case TAGPATHBIGRAM:
                                    currBigarmFilePath = reader.Value;
                                    break;
                                
                                case TAGPATHTRIGRAM:
                                    currTrigarmFilePath = reader.Value;
                                    break;
                            }
                            break;

                        case XmlNodeType.XmlDeclaration:
                        case XmlNodeType.ProcessingInstruction:

                            break;

                        case XmlNodeType.Comment:
                            break;

                        case XmlNodeType.EndElement:
                            if (reader.Name == TAGLANGUAGE)
                            {
                                resultLanguageCollection.Add(new Language(currName, currUnigramnFilePath, currBigarmFilePath,
                                    currTrigarmFilePath, currNumberOfChars, this.ParseCharLanguageFrequecy(currUnigramnFilePath, storageType),
                                    this.ParseBigramsLanguageFrequecy(currBigarmFilePath, storageType), this.ParseTrigramsLanguageFrequecy(currTrigarmFilePath, storageType)));
                            
                                
                                currName = "";
                                currNumberOfChars = 0;
                                currUnigramnFilePath = "";
                                currBigarmFilePath = "";
                                currTrigarmFilePath = "";
                            
                            }
                            break;
                    }
                }
            }
            return resultLanguageCollection;
        }


        private Dictionary<char, double> ParseCharLanguageFrequecy(string filePath, string storageType)
        {
            switch(storageType)
            {
                case XMLFILE:
                    return this.ParseLanguageFrequecyXML(filePath);
                
                case TEXTFILE:
                    return this.ParseLanguageFrequecyFile(filePath);
            }
            return null;
        }


        private Dictionary<string, double> ParseBigramsLanguageFrequecy(string filePath, string storageType)
        {
            return this.ParseLanguageBigramsFrequecyXML(filePath);
        }


        private Dictionary<string, double> ParseTrigramsLanguageFrequecy(string filePath, string storageType)
        {
            return this.ParseLanguageTrigramsFrequecyXML(filePath);
        }


        private Dictionary<char, double> ParseLanguageFrequecyFile(string filePath)
        {
            if (filePath == null || filePath == string.Empty)
                return new Dictionary<char, double>();

            Dictionary<char, double> currFrequencyCollection = new Dictionary<char, double>();

            using (StreamReader sr = new StreamReader(filePath))
            {

                string[] currCharsFrequencyValues = sr.ReadToEnd().Split(FILEPARSESEPARATORSARRAY, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < currCharsFrequencyValues.Length; j += 2)
                {
                    currFrequencyCollection.Add(currCharsFrequencyValues[j][0], Double.Parse(currCharsFrequencyValues[j + 1], new System.Globalization.CultureInfo("en-US")));
                }
            }

            return currFrequencyCollection;
        }


        private Dictionary<char, double> ParseLanguageFrequecyXML(string filePath)
        {
            if (filePath == null || filePath == string.Empty)
                return new Dictionary<char, double>();

            Dictionary<char, double> currFrequencyCollection = new Dictionary<char, double>();

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // Create the XmlReader object.
            //using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)), settings)
            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if (reader.Name == TAGPAIR)
                                currFrequencyCollection.Add(reader.GetAttribute(TAGCHARACTER)[0], Double.Parse(reader.GetAttribute(TAGFREQUENCY), new System.Globalization.CultureInfo("en-US")));
                            break;

                        case XmlNodeType.Text:
                            break;

                        case XmlNodeType.XmlDeclaration:
                        case XmlNodeType.ProcessingInstruction:
                            break;

                        case XmlNodeType.Comment:
                            break;

                        case XmlNodeType.EndElement:
                            break;
                    }
                }
            }

            return currFrequencyCollection;
        }


        private Dictionary<string, double> ParseLanguageBigramsFrequecyXML(string filePath)
        {

            if (filePath == null || filePath == string.Empty)
                return new Dictionary<string, double>();

            Dictionary<string, double> currFrequencyCollection = new Dictionary<string, double>();

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // Create the XmlReader object.
            //using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)), settings)
            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if (reader.Name == TAGPAIR)
                                currFrequencyCollection.Add(reader.GetAttribute(TAGCHARACTER), Double.Parse(reader.GetAttribute(TAGFREQUENCY), new System.Globalization.CultureInfo("en-US")));
                            break;

                        case XmlNodeType.Text:
                            break;

                        case XmlNodeType.XmlDeclaration:
                        case XmlNodeType.ProcessingInstruction:
                            break;

                        case XmlNodeType.Comment:
                            break;

                        case XmlNodeType.EndElement:
                            break;
                    }
                }
            }

            return currFrequencyCollection;
        }


        private Dictionary<string, double> ParseLanguageTrigramsFrequecyXML(string filePath)
        {

            if (filePath == null || filePath == string.Empty)
                return new Dictionary<string, double>();

            Dictionary<string, double> currFrequencyCollection = new Dictionary<string, double>();

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // Create the XmlReader object.
            //using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)), settings)
            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if (reader.Name == TAGPAIR)
                                currFrequencyCollection.Add(reader.GetAttribute(TAGCHARACTER), Double.Parse(reader.GetAttribute(TAGFREQUENCY), new System.Globalization.CultureInfo("en-US")));
                            break;

                        case XmlNodeType.Text:
                            break;

                        case XmlNodeType.XmlDeclaration:
                        case XmlNodeType.ProcessingInstruction:
                            break;

                        case XmlNodeType.Comment:
                            break;

                        case XmlNodeType.EndElement:
                            break;
                    }
                }
            }

            return currFrequencyCollection;
        }
    }
}

