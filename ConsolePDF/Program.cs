using detranLibraryNetCore.Model.habilitacao.pdfHabilitacao;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace ConsolePDF
{
    internal class Program
    {
        static string diretorio = "C:\\Users\\dudui\\OneDrive\\Área de Trabalho";
        string email = "dudui1998@gmail.com";
        static void Main(string[] args)
        {
            //var app = new Program();
            //app.GerarRelatorioSimples();
            var app = new PdfComprovanteMedico();
            app.GenerateInvoice(diretorio);
            Console.WriteLine("PDF criado com sucesso!");
            // MessageBox.Show("aaa");

        }
        private void GerarRelatorioSimples()
        {
            string caminho = string.Format("{0}/AprendendoItextSharp.pdf", diretorio);

            Document document = new Document(PageSize.A4, 20, 20, 10, 10);
            FileStream fs = new FileStream(caminho, FileMode.Create, FileAccess.Write, FileShare.None);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, fs);
            Paragraph Enter = new Paragraph("\n");
            document.Open();

            BaseColor azulClaro = new BaseColor(30, 144, 255);
            BaseColor azul = BaseColor.BLUE;
            BaseColor azulEscuro = new BaseColor(70, 130, 180);

            BaseColor vermelho = BaseColor.RED;

            var fontTimes = FontFactory.GetFont(FontFactory.TIMES, 16, azulClaro);
            var fontTimesBold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 16, azul);
            var fontTimesItalic = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 16, azulEscuro);

            var fontHelvetica = FontFactory.GetFont(FontFactory.HELVETICA, 16, vermelho);

            Paragraph p1 = new Paragraph(string.Format("Olá, Mundo!"), fontTimesBold);
            document.Add(p1);
            document.Add(Enter);

            Paragraph p2 = new Paragraph("Existem muitas variações disponíveis de passagens de Lorem Ipsum, mas a maioria sofreu algum tipo de alteração, seja por inserção de passagens com humor, ou palavras aleatórias que não parecem nem um pouco convincentes. Se você pretende usar uma passagem de Lorem Ipsum, precisa ter certeza de que não há algo embaraçoso escrito escondido no meio do texto. Todos os geradores de Lorem Ipsum na internet tendem a repetir pedaços predefinidos conforme necessário, fazendo deste o primeiro gerador de Lorem Ipsum autêntico da internet. Ele usa um dicionário com mais de 200 palavras em Latim combinado com um punhado de modelos de estrutura de frases para gerar um Lorem Ipsum com aparência razoável, livre de repetições, inserções de humor, palavras não características, etc.", fontTimes);
            document.Add(p2);
            document.Add(Enter);


            Paragraph p3 = new Paragraph(string.Format("Email: {0}", email), fontTimesItalic);
            document.Add(p3);
            document.Add(Enter);

            Paragraph p4 = new Paragraph("Existem muitas variações disponíveis de passagens de Lorem Ipsum, mas a maioria sofreu algum tipo de alteração, seja por inserção de passagens com humor, ou palavras aleatórias que não parecem nem um pouco convincentes. Se você pretende usar uma passagem de Lorem Ipsum, precisa ter certeza de que não há algo embaraçoso escrito escondido no meio do texto. Todos os geradores de Lorem Ipsum na internet tendem a repetir pedaços predefinidos conforme necessário, fazendo deste o primeiro gerador de Lorem Ipsum autêntico da internet. Ele usa um dicionário com mais de 200 palavras em Latim combinado com um punhado de modelos de estrutura de frases para gerar um Lorem Ipsum com aparência razoável, livre de repetições, inserções de humor, palavras não características, etc.", fontHelvetica);
            document.Add(p4);

            document.Close();
        }

    }
}
