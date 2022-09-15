using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eiko.Extract.Base64
{
    class Program
    {
        static void Main(string[] args)
        {
            //Informação
            Console.WriteLine("Extração de PDFs em Base64 de arquivo XMLs");
            Console.WriteLine("");

            //Obter diretório
            string dirPath = getDir();
            if (string.IsNullOrEmpty(dirPath))
            {
                Console.WriteLine("Diretório não informado");
                Console.WriteLine("A aplicação será finalizada");
                Console.ReadLine();
                return;
            }

            //Obtendo arquivos no diretório
            string[] files = Directory.GetFiles(dirPath, "*.xml");
            if (files.Count() == 0)
            {
                Console.WriteLine("Não foram localizados arquivos XMLs");
                Console.WriteLine("A aplicação será finalizada");
                Console.ReadLine();
                return;
            }

            //Processando arquivos
            int countXml = 0;
            int countPdf = 0;
            foreach (string item in files)
            {
                Console.WriteLine("Arquivo XML: " + item);
                countXml++;

                XmlDocument doc = new XmlDocument();
                doc.Load(item);
                XmlNodeList elemList = doc.GetElementsByTagName("NFSImagem");

                bool find = false;
                for (int i = 0; i < elemList.Count; i++)
                {
                    find = true;

                    string pdfCont = elemList[i].InnerXml;
                    string pdfNome = item.Replace(".xml", ".pdf");

                    byte[] bytes = Convert.FromBase64String(pdfCont);
                    
                    FileStream stream = new FileStream(pdfNome, FileMode.CreateNew);
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(bytes, 0, bytes.Length);
                    writer.Close();

                    Console.WriteLine("Arquivo PDF: " + item);
                    countPdf++;
                }

                if (find == false)
                    Console.WriteLine("Conteúdo PDF não localizado");

                Console.WriteLine("");

                //Directory.Move(file.FullName, filepath + "\\TextFiles\\" + file.Name);
            }

            //Resumo
            Console.WriteLine("Resumo de processamento");
            Console.WriteLine("Arquivos XML processados: " + countXml);
            Console.WriteLine("Arquivos PDF gerados: " + countPdf);

            //Finalização
            Console.ReadLine();
        }

        private static string getDir()
        {
            //Obter diretório
            Console.WriteLine("Informe o diretório para obter os arquivos");
            string dirTemp = Console.ReadLine();

            return dirTemp;
        }
    }
}
