using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PDF_PageSizes
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || !File.Exists(args[0].ToString()))
            {
                InfoMessage();
            }
            else
            {
                string argument = args[0];
                string file = argument.ToString();
                string extension;

                extension = Path.GetExtension(file);

                if (extension == ".pdf")
                {
                    checkHeader(file);
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("ADVERTENCIA: El archivo no tiene extensión PDF, tiene extensión " + extension);
                    InfoMessage();
                }
            }
        }

        static void GetPDFInfo(string pdf_filename)
        {
            PdfReader reader = new PdfReader(pdf_filename);
            Console.WriteLine("Tamaño de página en unidades Postscript...");

            float n = reader.NumberOfPages;

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                Rectangle psize = reader.GetPageSize(i);
                float width = psize.Width;
                float height = psize.Height;
                string strPageRotation = null;

                if (height >= width)
                {
                    strPageRotation = "Vertical";
                }
                else
                {
                    strPageRotation = "Horizontal";
                }

                Console.WriteLine("Tamaño y rotación de la página {0} de {1}, {2} × {3}, {4}", i, reader.NumberOfPages, width, height, strPageRotation);
            }

            Console.WriteLine("Total de páginas PDF que contiene el documento, {0}", n);

        }

        static void checkHeader(string filename_to_check)
        {
            byte[] binaryHeader = new byte[4];

            using (BinaryReader reader = new BinaryReader(new FileStream(filename_to_check, FileMode.Open)))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                reader.Read(binaryHeader, 0, 4);
            }

            var strHeader = Encoding.Default.GetString(binaryHeader);

            if (strHeader == "%PDF")
            {
                GetPDFInfo(filename_to_check);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("ADVERTENCIA: El archivo no tiene cabecera del tipo de documento PDF");
                Console.WriteLine("ADVERTENCIA: Se esperaba cabecera %PDF, pero la cabecera encontrada en el archivo es \"" + strHeader + "\"");
                InfoMessage();

            }
        }

        static void InfoMessage()
        {
            Console.WriteLine("");
            Console.WriteLine("No se encontró archivo como documento PDF a procesar.");
            Console.WriteLine("");
            Console.WriteLine("El uso de la aplicación es: " + Process.GetCurrentProcess().ProcessName + " nombre_archivo.pdf"); // Check for null array
            Console.WriteLine("");
            Console.WriteLine("Se puede declarar la ruta completa del archivo PDF.");
            Console.WriteLine("Ej: " + Process.GetCurrentProcess().ProcessName + " C:\\users\\usuario1\\desktop\\archivo.pdf");
            Console.WriteLine("");
        }
    }
}
