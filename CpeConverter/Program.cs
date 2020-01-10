using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace CpeConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReaderSettings cpeDictionaryReaderSettings = new XmlReaderSettings();
            cpeDictionaryReaderSettings.IgnoreWhitespace = true;

            string csvFilePath = args[1]; // "C:\\temp\\cpeDictionary\\test-cpe-dictionary.csv";
            string cpeItemName, cpeItemTitle, cpe23ItemName, csvEntry;
            cpeItemName = cpeItemTitle = cpe23ItemName = csvEntry = String.Empty;

            using (StreamWriter fileStreamWriter = new StreamWriter(csvFilePath))
            {
                using (XmlReader cpeDictionaryReader = XmlReader.Create(args[0]/*"test-official-cpe-dictionary_v2.3.xml"*/, cpeDictionaryReaderSettings))
                {
                    while (cpeDictionaryReader.Read())
                    {
                        //                Console.Write(new string(' ', cpeDictionaryReader.Depth * 2));
                        //                Console.WriteLine(cpeDictionaryReader.NodeType);

                        Console.Write(cpeDictionaryReader.NodeType.ToString().PadRight(17, '-'));
                        Console.Write("> ".PadRight(cpeDictionaryReader.Depth * 3));

                        switch (cpeDictionaryReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                Console.WriteLine(cpeDictionaryReader.Name);
                                switch (cpeDictionaryReader.Name)
                                {
                                    case "cpe-item":
                                        cpeItemName = cpeDictionaryReader["name"];
                                        cpeDictionaryReader.Read();
                                        cpeDictionaryReader.ReadStartElement("title");
                                        cpeItemTitle = cpeDictionaryReader.Value; break;

                                    case "cpe-23:cpe23-item":
                                        cpe23ItemName = cpeDictionaryReader["name"];
                                        csvEntry = string.Format("{0},{1},{2}", cpeItemName, cpeItemTitle, cpe23ItemName);
                                        fileStreamWriter.WriteLine(csvEntry); break;

                                    default: break;
                                }
                                break;
                            case XmlNodeType.EndElement:
                                Console.WriteLine(cpeDictionaryReader.Name); break;

                            case XmlNodeType.Text:
                            case XmlNodeType.CDATA:
                            case XmlNodeType.Comment:
                            case XmlNodeType.XmlDeclaration:
                                Console.WriteLine(cpeDictionaryReader.Value); break;

                            case XmlNodeType.DocumentType:
                                Console.WriteLine(cpeDictionaryReader.Name + " - " + cpeDictionaryReader.Value); break;

                            default: break;
                        }
                    }
                }
            }
        }
    }
}
