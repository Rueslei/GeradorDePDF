using iTextSharp.text; // Extensão 1 - Text
using iTextSharp.text.pdf; // Extensão 2 - PDF
using System;
using System.Collections.Generic;
using System.IO;

namespace detranLibraryNetCore.Model.habilitacao.pdfHabilitacao
{
    public class PdfComprovanteMedico
    {
        public static string tipoServico = "p";

        public void GenerateInvoice(string diretorio)
        {
            string caminho = string.Format("{0}/ComprovanteExameBeta.pdf", diretorio);

            Document document = new Document(PageSize.A4, 20, 20, 10, 10);
            FileStream fs = new FileStream(caminho, FileMode.Create, FileAccess.Write, FileShare.None);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, fs);
            WriteDocument(document, pdfWriter);

        }

        public byte[] GenerateInvoice2()
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            Document document = new Document(PageSize.A4, 2F, 2F, 25F, 10F);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            //FieldsGenerate data = new FieldsGenerate();

            WriteDocument(document, writer);

            byte[] bytes = memoryStream.ToArray();

            //var docData = Convert.ToBase64String(bytes);

            return bytes;
        }

        public static string GetObservacoes(string tipoServico)
        {
            if(tipoServico.ToUpper() == "M")
            {
                return "\nANTES DA REALIZAÇÃO DO EXAME É NECESSÁRIO IDENTIFICAR-SE BIOMETRICAMENTE NAS UNIDADES DE ATENDIMENTO DO DETRAN. NA DATA AGENDADA, APRESENTAR-SE MUNIDO DE DOCUMENTO DE IDENTIFICAÇÃO, DO QUESTIONÁRIO PREENCHIDO E, CASO SE TRATE DE CATEGORIA \"C\", \"D\" OU \"E\", DO EXAME TOXICOLÓGICO ORIGINAL (VER OBSERVAÇÕES ABAIXO), BEM COMO, PARA SUA SEGURANÇA, ESTE COMPROVANTE DE AGENDAMENTO.\r\n";
            }
            else
            {
                return "\nANTES DA REALIZAÇÃO DO EXAME É NECESSÁRIO IDENTIFICAR-SE BIOMETRICAMENTE NAS UNIDADES DE ATENDIMENTO DO DETRAN. NA DATA AGENDADA, APRESENTAR-SE MUNIDO DE DOCUMENTO DE IDENTIFICAÇÃO.\r\n";
            }

        }

        public static void WriteDocument(Document doc, PdfWriter writer)
        {
            doc.Open();
            Rectangle page = doc.PageSize;
            Font Titulo = FontFactory.GetFont("Verdana", 11F, Font.BOLD, BaseColor.BLACK);
            Font Subtitulo = FontFactory.GetFont("Verdana", 8F, Font.BOLD, BaseColor.BLACK);
            Font FontePadrao = FontFactory.GetFont("Verdana", 8F, Font.NORMAL, BaseColor.BLACK);
            Font Cabecalho = FontFactory.GetFont("Verdana", 7F, Font.NORMAL, BaseColor.BLACK);
            Paragraph parag = new Paragraph(new Phrase("\n"));

            //string pathImageDetran = Path.Combine(Environment.CurrentDirectory, @"Assets/Docs", "detran.jpeg");
            string pathImageGov = "https://www.detran.se.gov.br/portal/img/governo/logo_detran.png";
            string pathImageBanese = "https://www.detran.se.gov.br/portal/images/banese.jpg";

            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(pathImageGov);
            iTextSharp.text.Image imageBanese = iTextSharp.text.Image.GetInstance(pathImageBanese);

            doc.Add(Header());
            doc.Add(tableGOV(writer, Cabecalho, image, Subtitulo));
            doc.Add(parag);
            doc.Add(tableTitulo(Titulo));
            doc.Add(parag);
            //  doc.Add(tableCodigoAutorizacao(FontePadrao, page, image, Subtitulo));
            // doc.Add(parag);
            //  doc.Add(tableTermo(FontePadrao, page, image, Subtitulo));
            //  doc.Add(tableAviso(FontePadrao, page, image, Subtitulo));
            doc.Add(tableDadosRequerente(FontePadrao, page, image, Subtitulo));
            doc.Add(tableDadosPerito(FontePadrao, page, image, Subtitulo));
            doc.Add(tableDadosAgendamento(FontePadrao, page, image, Subtitulo));
            doc.Add(tableCursos(FontePadrao, page, image, Subtitulo));
            // doc.Add(tableObservacoes(Cabecalho, page, image, Subtitulo, FontePadrao));
            //  doc.Add(tableAviso(Cabecalho, page, image, Subtitulo, FontePadrao));

            //doc.Add(tableAutorizacao(FontePadrao, page, image,  Subtitulo));


            doc.Close();

        }
        public static Paragraph Header()
        {
            DateTime thisDay = DateTime.Now;
            string dados = "DETRAN/SE - Portal de Serviços - Doc gerado eletronicamente em " + thisDay.ToString("dd/MM/yyyy") + " às " + thisDay.ToString("HH:mm:ss");
            Paragraph cabecalho = new Paragraph(dados, new Font(Font.NORMAL, 9));
            cabecalho.Alignment = Element.ALIGN_CENTER;
            return cabecalho;
        }

        public static PdfPTable tableGOV(PdfWriter writer, Font Cabecalho, iTextSharp.text.Image image, Font Subtitulo)
        {
            PdfPTable tableGOV = new PdfPTable(1);

            PdfPCell cell1 = new PdfPCell();
            AddImageInCell(cell1, image, 50f, 50f, 1);
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.Border = 1;
            //cell1.Padding = 4f;
            cell1.Rowspan = 4;
            tableGOV.AddCell(cell1);

            /*float[] widths = new float[] { 10f, 10f, 10f, 10f, 10f };
            tableGOV.SetWidths(widths);*/
            DateTime thisDay = DateTime.Now;

            PdfPCell cell0 = new PdfPCell(new Phrase("Governo de Sergipe \n Secretaria de Estado da Segurança Pública \n Departamento Estadual de Trânsito DETRAN/SE \n" + thisDay.ToString("dd / MM / yyyy") + " às " + thisDay.ToString("HH: mm:ss"), Cabecalho));
            cell0.HorizontalAlignment = Element.ALIGN_CENTER;
            cell0.Border = 0;
            tableGOV.AddCell(cell0);

            return tableGOV;
        }
        public static PdfPTable tableTitulo(Font Subtitulo)
        {
            PdfPTable tableTitulo = new PdfPTable(1);
            PdfPCell cell0 = default;
            if (tipoServico.ToUpper() == "M")
            {
                cell0 = new PdfPCell(new Phrase("COMPROVANTE DE AGENDAMENTO DO EXAME DE APTIDÃO FISICA E MENTAL", Subtitulo));
            }
            else if (tipoServico.ToUpper() == "P")
            {
                cell0 = new PdfPCell(new Phrase("COMPROVANTE DE AGENDAMENTO DA AVALIAÇÃO PSICOLÓGICA", Subtitulo));
            }
            cell0.HorizontalAlignment = 1;
            cell0.Border = 0;
            tableTitulo.AddCell(cell0);
            return tableTitulo;
        }
        //public static PdfPTable tableCodigoAutorizacao(Font FontePadrao, Rectangle page, iTextSharp.text.Image image, Font Subtitulo)
        //{

        //    PdfPTable table1 = new PdfPTable(3);

        //    //----------------
        //    Paragraph pargCodigoAutorizacao1 = new Paragraph();
        //    Phrase desc1 = new Phrase("Código Autorização Detran: ", Subtitulo);
        //    Phrase desc2 = new Phrase("abc", FontePadrao);
        //    pargCodigoAutorizacao1.Add(desc1);
        //    pargCodigoAutorizacao1.Add(desc2);
        //    PdfPCell cell1 = new PdfPCell(pargCodigoAutorizacao1);
        //    cell1.HorizontalAlignment = 0;
        //    cell1.Colspan = 2;
        //    cell1.Border = 0;
        //    table1.AddCell(cell1);

        //    Paragraph pargCodigoAutorizacao2 = new Paragraph();
        //    Phrase desc3 = new Phrase("Credenciamento: ", Subtitulo);
        //    Phrase desc4 = new Phrase("abc", FontePadrao);
        //    pargCodigoAutorizacao2.Add(desc3);
        //    pargCodigoAutorizacao2.Add(desc4);
        //    PdfPCell cell3 = new PdfPCell(pargCodigoAutorizacao2);
        //    cell3.HorizontalAlignment = 0;
        //    cell3.Colspan = 2;
        //    cell3.Border = 0;
        //    table1.AddCell(cell3);

        //    return table1;
        //}

        //public static PdfPTable tableTermo(Font FontePadrao, Rectangle page, iTextSharp.text.Image image, Font Subtitulo)
        //{
        //    PdfPTable table2 = new PdfPTable(1);
        //    table2.SpacingAfter = 10f;

        //    PdfPCell cell0 = new PdfPCell(new Phrase("DE ACORDO COM TERMO DE AUDIENCIA EXTRAJUDICIAL DO MINISTERIO PUBLICO ESTADUAL E CONFORME AUTORIZACAO DA PROCURADORIA JURIDICA DO ORGAO (COMUNICACAO INTERNA 70/2017), ESTA AUTORIZACAO DEVE SER APRESENTADA A QUALQUER UMA CREDENCIADAS AO DETRAN DE SERGIPE PARA REALIZACAO DE SERVICOS DE ESTAMPAGENS. ESCOLHA UMA DELAS NA LISTA DISPONIVEL EM TODOS OS SETORES DE ATENDIMENTO DO ORGAO OU ACESSE O SITE DO DETRAN (WWW.DETRAN.SE.GOV.BR) E NO MENU CLIQUE EM 'CREDENCIADOS'.", Subtitulo));
        //    cell0.Colspan = 2;
        //    cell0.Border = 0;
        //    cell0.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //    table2.AddCell(cell0);

        //    return table2;
        //}

        //public static PdfPTable tableAviso(Font FontePadrao, Rectangle page, iTextSharp.text.Image image, Font Subtitulo)
        //{
        //    PdfPTable table3 = new PdfPTable(1);


        //    PdfPCell cell0 = new PdfPCell(new Phrase("AVISO IMPORTANTE PARA O CLIENTE\n ", Subtitulo));
        //    cell0.Colspan = 2;
        //    cell0.Border = 0;
        //    cell0.HorizontalAlignment = Element.ALIGN_CENTER;
        //    table3.AddCell(cell0);


        //    PdfPCell cell1 = new PdfPCell(new Phrase("Em função da Portaria Detran 349 e suas alterações," +
        //        " todos os processos iniciados a partir de 19 de outubro 2020, deverão seguir o procedimento abaixo: \n ", FontePadrao));
        //    cell1.HorizontalAlignment = 0;
        //    cell1.Colspan = 2;
        //    cell1.Border = 0;
        //    cell1.Border = 0;
        //    table3.AddCell(cell1);

        //    PdfPCell cell2 = new PdfPCell(new Phrase(" Após a finalização do processo de veículo com a emissão do documento, seja Certificado de Registro de Veículo - CRV ou Certificado de Licenciamento Anual - CRLV, " +
        //        "escolha uma das Empresas Estampadoras de Placas de Identificação Veicular de sua preferência (https://www.detran.se.gov.br/?pg=credenciado/lojasdeplacas#gsc.tab=0) para realização do ciclo de " +
        //        "estampagem (pagamento, agendamento, emissão da nota fiscal e colocação da(s) placa(s)). \n   ", FontePadrao));
        //    cell2.HorizontalAlignment = 0;
        //    cell2.Colspan = 2;
        //    cell2.Border = 0;
        //    cell2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //    table3.AddCell(cell2);

        //    PdfPCell cell3 = new PdfPCell(new Phrase(" Observação: É da inteira responsabilidade do cliente a escolha da empresa que realizará o ciclo completo de estampagem. \n", FontePadrao));
        //    cell3.HorizontalAlignment = 0;
        //    cell3.Colspan = 2;
        //    cell3.Border = 0;
        //    table3.AddCell(cell3);

        //    return table3;
        //}

        public static PdfPTable tableDadosRequerente(Font FontePadrao, Rectangle page, iTextSharp.text.Image image, Font Subtitulo)
        {
            PdfPTable table1 = new PdfPTable(3);

            //table1.SpacingAfter = 10f;


            PdfPCell cell0 = new PdfPCell(new Phrase("DADOS DO REQUERENTE", Subtitulo));
            cell0.Colspan = 6;
            cell0.Border = 0;
            cell0.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(cell0);

            //---------------- ADICIONA LINHA NO TEXTO
            PdfPCell cellLine = new PdfPCell(new Phrase("————————————————————————————————————\r\n"));
            cellLine.Colspan = 6;
            cellLine.Border = 0;
            cellLine.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            table1.AddCell(cellLine);

            //-------------
            Phrase p1 = new Phrase("CLIENTE: ", Subtitulo);
            Phrase p2 = new Phrase("STATICPAULO", FontePadrao);
            Paragraph paraProp1 = new Paragraph();
            paraProp1.Add(p1);
            paraProp1.Add(p2);
            PdfPCell cell1 = new PdfPCell(paraProp1);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);
            //------------
            Paragraph paraProp2 = new Paragraph();
            Phrase p3 = new Phrase("DATA DE NASCIMENTO: ", Subtitulo);
            Phrase p4 = new Phrase("00/00/0000", FontePadrao);
            paraProp2.Add(p3);
            paraProp2.Add(p4);
            cell1 = new PdfPCell(paraProp2);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);
            //-------------
            Paragraph paraProp3 = new Paragraph();
            Phrase p5 = new Phrase("IDENTIDADE: ", Subtitulo);
            Phrase p6 = new Phrase("STATIC000000", FontePadrao);
            paraProp3.Add(p5);
            paraProp3.Add(p6);
            cell1 = new PdfPCell(paraProp3);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            //-------------
            Paragraph paraProp4 = new Paragraph();
            Phrase p7 = new Phrase("CPF: ", Subtitulo);
            Phrase p8 = new Phrase("STATIC000000", FontePadrao);
            paraProp4.Add(p7);
            paraProp4.Add(p8);
            PdfPCell cell2 = new PdfPCell(paraProp4);
            cell2.HorizontalAlignment = 0;
            cell2.Colspan = 2;
            cell2.Border = 0;
            table1.AddCell(cell2);

            //-------------
            Paragraph paraProp5 = new Paragraph();
            Phrase p9 = new Phrase("SEXO: ", Subtitulo);
            Phrase p10 = new Phrase("STATICSEXO", FontePadrao);
            paraProp5.Add(p9);
            paraProp5.Add(p10);
            cell2 = new PdfPCell(paraProp5);
            cell2.HorizontalAlignment = 0;
            cell2.Colspan = 2;
            cell2.Border = 0;
            table1.AddCell(cell2);

            return table1;
        }

        public static PdfPTable tableDadosPerito(Font FontePadrao, Rectangle page, iTextSharp.text.Image image, Font Subtitulo)
        {
            PdfPTable table1 = new PdfPTable(3);

            PdfPCell cell0 = new PdfPCell(new Phrase("\n\nDADOS DO PERITO", Subtitulo));
            cell0.Colspan = 6;
            cell0.Border = 0;
            cell0.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(cell0);

            //---------------- ADICIONA LINHA NO TEXTO
            PdfPCell cellLine = new PdfPCell(new Phrase("————————————————————————————————————\r\n"));
            cellLine.Colspan = 6;
            cellLine.Border = 0;
            cellLine.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            table1.AddCell(cellLine);

            //----------------
            Paragraph pargNomePerito = new Paragraph();
            Phrase p1 = new Phrase("PERITO: ", Subtitulo);
            Phrase p2 = new Phrase("staticNomePerito", FontePadrao);
            pargNomePerito.Add(p1);
            pargNomePerito.Add(p2);
            PdfPCell cell1 = new PdfPCell(pargNomePerito);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            //----------------
            Paragraph pargTipoPerito = new Paragraph();
            Phrase p3 = new Phrase("TIPO: ", Subtitulo);
            Phrase p4 = new Phrase("staticoTipo", FontePadrao);
            pargTipoPerito.Add(p3);
            pargTipoPerito.Add(p4);
            cell1 = new PdfPCell(pargTipoPerito);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            ////----------------
            //Paragraph pargVeiculo2 = new Paragraph();
            //Phrase p5 = new Phrase("Categoria: ", Subtitulo);
            //Phrase p6 = new Phrase("abc", FontePadrao);
            //pargVeiculo2.Add(p5);
            //pargVeiculo2.Add(p6);
            //cell1 = new PdfPCell(pargVeiculo2);
            //cell1.HorizontalAlignment = 0;
            //cell1.Colspan = 2;
            //cell1.Border = 0;
            //table1.AddCell(cell1);

            ////2
            ////----------------
            //Paragraph pargVeiculo3 = new Paragraph();
            //Phrase p7 = new Phrase("Município: ", Subtitulo);
            //Phrase p8 = new Phrase("abc", FontePadrao);
            //pargVeiculo3.Add(p7);
            //pargVeiculo3.Add(p8);
            //PdfPCell cell2 = new PdfPCell(pargVeiculo3);
            //cell2.HorizontalAlignment = 0;
            //cell2.Colspan = 2;
            //cell2.Border = 0;
            //table1.AddCell(cell2);

            ////----------------
            //Paragraph pargVeiculo4 = new Paragraph();
            //Phrase p9 = new Phrase("Chassi: ", Subtitulo);
            //Phrase p10 = new Phrase("abc", FontePadrao);
            //pargVeiculo4.Add(p9);
            //pargVeiculo4.Add(p10);
            //cell2 = new PdfPCell(pargVeiculo4);
            //cell2.HorizontalAlignment = 0;
            //cell2.Colspan = 2;
            //cell2.Border = 0;
            //table1.AddCell(cell2);

            ////----------------
            //Paragraph pargVeiculo5 = new Paragraph();
            //Phrase p11 = new Phrase("Marca: ", Subtitulo);
            //Phrase p12 = new Phrase("abc", FontePadrao);
            //pargVeiculo5.Add(p11);
            //pargVeiculo5.Add(p12);
            //cell2 = new PdfPCell(pargVeiculo5);
            //cell2.HorizontalAlignment = 0;
            //cell2.Colspan = 2;
            //cell2.Border = 0;
            //table1.AddCell(cell2);

            ////3
            ////----------------
            //Paragraph pargVeiculo6 = new Paragraph();
            //Phrase p13 = new Phrase("Cor: ", Subtitulo);
            //Phrase p14 = new Phrase("abc", FontePadrao);
            //pargVeiculo6.Add(p13);
            //pargVeiculo6.Add(p14);
            //PdfPCell cell3 = new PdfPCell(pargVeiculo6);
            //cell3.HorizontalAlignment = 0;
            //cell3.Colspan = 2;
            //cell3.Border = 0;
            //table1.AddCell(cell3);

            ////----------------
            //Paragraph pargVeiculo7 = new Paragraph();
            //Phrase p15 = new Phrase("Tipo: ", Subtitulo);
            //Phrase p16 = new Phrase("", FontePadrao);
            //pargVeiculo7.Add(p15);
            //pargVeiculo7.Add(p16);
            //cell3 = new PdfPCell(pargVeiculo7);
            //cell3.HorizontalAlignment = 0;
            //cell3.Colspan = 2;
            //cell3.Border = 0;
            //table1.AddCell(cell3);

            ////----------------
            //Paragraph pargVeiculo8 = new Paragraph();
            //Phrase p17 = new Phrase("Espécie: ", Subtitulo);
            //Phrase p18 = new Phrase("", FontePadrao);
            //pargVeiculo8.Add(p17);
            //pargVeiculo8.Add(p18);
            //cell3 = new PdfPCell(pargVeiculo8);
            //cell3.HorizontalAlignment = 0;
            //cell3.Colspan = 2;
            //cell3.Border = 0;
            //table1.AddCell(cell3);

            //cell3 = new PdfPCell(new Phrase(" ", FontePadrao));
            //cell3.HorizontalAlignment = 0;
            //cell3.Colspan = 2;
            //cell3.Border = 0;
            //table1.AddCell(cell3);

            return table1;
        }
        public static PdfPTable tableDadosAgendamento(Font FontePadrao, Rectangle page, iTextSharp.text.Image image, Font Subtitulo)
        {
            PdfPTable table1 = new PdfPTable(3);

            PdfPCell cell0 = new PdfPCell(new Phrase("\n\nDADOS DO AGENDAMENTO", Subtitulo));
            cell0.Colspan = 6;
            cell0.Border = 0;
            cell0.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(cell0);

            //---------------- ADICIONA LINHA NO TEXTO
            PdfPCell cellLine = new PdfPCell(new Phrase("————————————————————————————————————\r\n"));
            cellLine.Colspan = 6;
            cellLine.Border = 0;
            cellLine.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            table1.AddCell(cellLine);

            //----------------
            Paragraph pargDataAgendamento = new Paragraph();
            Phrase p1 = new Phrase("DATA: ", Subtitulo);
            Phrase p2 = new Phrase("STATIC DATA", FontePadrao);
            pargDataAgendamento.Add(p1);
            pargDataAgendamento.Add(p2);
            PdfPCell cell1 = new PdfPCell(pargDataAgendamento);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            //----------------
            Paragraph pargHoraAgendamento = new Paragraph();
            Phrase p3 = new Phrase("HORA: ", Subtitulo);
            Phrase p4 = new Phrase("StaticHora", FontePadrao);
            pargHoraAgendamento.Add(p3);
            pargHoraAgendamento.Add(p4);
            cell1 = new PdfPCell(pargHoraAgendamento);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            //----------------
            Paragraph pargDadosAgendamento = new Paragraph();
            Phrase p5 = new Phrase("POSTO DE ATENDIMENTO: ", Subtitulo);
            Phrase p6 = new Phrase("STATICVALUE", FontePadrao);
            pargDadosAgendamento.Add(p5);
            pargDadosAgendamento.Add(p6);
            cell1 = new PdfPCell(pargDadosAgendamento);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            //2
            //----------------
            Paragraph pargSituacao = new Paragraph();
            Phrase p7 = new Phrase("SITUAÇÃO: ", Subtitulo);
            Phrase p8 = new Phrase("STATICVALUE", FontePadrao);
            pargSituacao.Add(p7);
            pargSituacao.Add(p8);
            PdfPCell cell2 = new PdfPCell(pargSituacao);
            cell2.HorizontalAlignment = 0;
            cell2.Colspan = 2;
            cell2.Border = 0;
            table1.AddCell(cell2);

            //----------------
            Paragraph pargClinica = new Paragraph();
            Phrase p9 = new Phrase("CLÍNICA: ", Subtitulo);
            Phrase p10 = new Phrase("STATICVALUE", FontePadrao);
            pargClinica.Add(p9);
            pargClinica.Add(p10);
            cell2 = new PdfPCell(pargClinica);
            cell2.HorizontalAlignment = 0;
            cell2.Colspan = 6;
            cell2.Border = 0;
            table1.AddCell(cell2);

            //----------------
            Paragraph pargEndereco = new Paragraph();
            Phrase p11 = new Phrase("ENDEREÇO: ", Subtitulo);
            Phrase p12 = new Phrase("STATICVALUE", FontePadrao);
            pargEndereco.Add(p11);
            pargEndereco.Add(p12);
            cell2 = new PdfPCell(pargEndereco);
            cell2.HorizontalAlignment = 0;
            cell2.Colspan = 6;
            cell2.Border = 0;
            table1.AddCell(cell2);

            //3
            //----------------
            Paragraph pargObservacaoAgendamento = new Paragraph();
            Phrase p13 = new Phrase(GetObservacoes(tipoServico), FontePadrao);            
            pargObservacaoAgendamento.Add(p13);           
            PdfPCell cell3 = new PdfPCell(pargObservacaoAgendamento);
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.Colspan = 6;
            cell3.Border = 0;
            table1.AddCell(cell3);
           

            return table1;
        } public static PdfPTable tableCursos(Font FontePadrao, Rectangle page, iTextSharp.text.Image image, Font Subtitulo)
        {
            PdfPTable table1 = new PdfPTable(3);

            PdfPCell cell0 = new PdfPCell(new Phrase("\n\nCURSOS", Subtitulo));
            cell0.Colspan = 6;
            cell0.Border = 0;
            cell0.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.AddCell(cell0);

            //---------------- ADICIONA LINHA NO TEXTO
            PdfPCell cellLine = new PdfPCell(new Phrase("————————————————————————————————————\r\n"));
            cellLine.Colspan = 6;
            cellLine.Border = 0;
            cellLine.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            table1.AddCell(cellLine);

            //----------------
            Paragraph pargDataAgendamento = new Paragraph();
            Phrase p1 = new Phrase("VERIFICAR RETORNO DA ROTINA: ", Subtitulo);
            Phrase p2 = new Phrase("STATIC DATA", FontePadrao);
            pargDataAgendamento.Add(p1);
            pargDataAgendamento.Add(p2);
            PdfPCell cell1 = new PdfPCell(pargDataAgendamento);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            //----------------
            Paragraph pargHoraAgendamento = new Paragraph();
            Phrase p3 = new Phrase("HORA: ", Subtitulo);
            Phrase p4 = new Phrase("StaticHora", FontePadrao);
            pargHoraAgendamento.Add(p3);
            pargHoraAgendamento.Add(p4);
            cell1 = new PdfPCell(pargHoraAgendamento);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            //----------------
            Paragraph pargDadosAgendamento = new Paragraph();
            Phrase p5 = new Phrase("POSTO DE ATENDIMENTO: ", Subtitulo);
            Phrase p6 = new Phrase("STATICVALUE", FontePadrao);
            pargDadosAgendamento.Add(p5);
            pargDadosAgendamento.Add(p6);
            cell1 = new PdfPCell(pargDadosAgendamento);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table1.AddCell(cell1);

            //2
            //----------------
            Paragraph pargSituacao = new Paragraph();
            Phrase p7 = new Phrase("SITUAÇÃO: ", Subtitulo);
            Phrase p8 = new Phrase("STATICVALUE", FontePadrao);
            pargSituacao.Add(p7);
            pargSituacao.Add(p8);
            PdfPCell cell2 = new PdfPCell(pargSituacao);
            cell2.HorizontalAlignment = 0;
            cell2.Colspan = 2;
            cell2.Border = 0;
            table1.AddCell(cell2);

            //----------------
            Paragraph pargClinica = new Paragraph();
            Phrase p9 = new Phrase("CLÍNICA: ", Subtitulo);
            Phrase p10 = new Phrase("STATICVALUE", FontePadrao);
            pargClinica.Add(p9);
            pargClinica.Add(p10);
            cell2 = new PdfPCell(pargClinica);
            cell2.HorizontalAlignment = 0;
            cell2.Colspan = 6;
            cell2.Border = 0;
            table1.AddCell(cell2);

            //----------------
            Paragraph pargEndereco = new Paragraph();
            Phrase p11 = new Phrase("ENDEREÇO: ", Subtitulo);
            Phrase p12 = new Phrase("STATICVALUE", FontePadrao);
            pargEndereco.Add(p11);
            pargEndereco.Add(p12);
            cell2 = new PdfPCell(pargEndereco);
            cell2.HorizontalAlignment = 0;
            cell2.Colspan = 6;
            cell2.Border = 0;
            table1.AddCell(cell2);

            //3
            //----------------
            Paragraph pargObservacaoAgendamento = new Paragraph();
            Phrase p13 = new Phrase(GetObservacoes(tipoServico), FontePadrao);            
            pargObservacaoAgendamento.Add(p13);           
            PdfPCell cell3 = new PdfPCell(pargObservacaoAgendamento);
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.Colspan = 6;
            cell3.Border = 0;
            table1.AddCell(cell3);

            //----------------
            Paragraph pargVeiculo7 = new Paragraph();
            Phrase p15 = new Phrase("Tipo: ", Subtitulo);
            Phrase p16 = new Phrase("", FontePadrao);
            pargVeiculo7.Add(p15);
            pargVeiculo7.Add(p16);
            cell3 = new PdfPCell(pargVeiculo7);
            cell3.HorizontalAlignment = 0;
            cell3.Colspan = 2;
            cell3.Border = 0;
            table1.AddCell(cell3);

            //----------------
            Paragraph pargVeiculo8 = new Paragraph();
            Phrase p17 = new Phrase("Espécie: ", Subtitulo);
            Phrase p18 = new Phrase("", FontePadrao);
            pargVeiculo8.Add(p17);
            pargVeiculo8.Add(p18);
            cell3 = new PdfPCell(pargVeiculo8);
            cell3.HorizontalAlignment = 0;
            cell3.Colspan = 2;
            cell3.Border = 0;
            table1.AddCell(cell3);

            cell3 = new PdfPCell(new Phrase(" ", FontePadrao));
            cell3.HorizontalAlignment = 0;
            cell3.Colspan = 2;
            cell3.Border = 0;
            table1.AddCell(cell3);

            return table1;
        }

        public static PdfPTable tableObservacoes(Font Cabecalho, Rectangle page, iTextSharp.text.Image image, Font Subtitulo, Font FontePadrao)
        {

            PdfPTable table6 = new PdfPTable(1);

            PdfPCell cell0 = new PdfPCell(new Phrase("\n Observações \n", Subtitulo));
            cell0.Colspan = 6;
            cell0.Border = 0;
            cell0.HorizontalAlignment = Element.ALIGN_CENTER;
            table6.AddCell(cell0);

            //----------------
            Paragraph paragObservacoes1 = new Paragraph();
            Phrase p1 = new Phrase("Serviço: ", Subtitulo);
            Phrase p2 = new Phrase("", FontePadrao);
            paragObservacoes1.Add(p1);
            paragObservacoes1.Add(p2);
            PdfPCell cell1 = new PdfPCell(paragObservacoes1);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table6.AddCell(cell1);

            //----------------
            Paragraph paragObservacoes2 = new Paragraph();
            Phrase p3 = new Phrase("Valor do Serviço: ", Subtitulo);
            Phrase p4 = new Phrase("Consultar os sites das estampadoras credenciadas para saber os valores", FontePadrao);
            paragObservacoes2.Add(p3);
            paragObservacoes2.Add(p4);
            cell1 = new PdfPCell(paragObservacoes2);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table6.AddCell(cell1);

            Paragraph paragObservacoes3 = new Paragraph();
            Phrase p5 = new Phrase("Número de protocolo utilizado no serviço: ", Subtitulo);
            Phrase p6 = new Phrase("", FontePadrao);
            paragObservacoes3.Add(p5);
            paragObservacoes3.Add(p6);
            cell1 = new PdfPCell(paragObservacoes3);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table6.AddCell(cell1);

            Paragraph paragObservacoes4 = new Paragraph();
            Phrase p7 = new Phrase("Documento de arrecadação utilizado no serviço de veículo do Detran: ", Subtitulo);
            Phrase p8 = new Phrase("", FontePadrao);
            paragObservacoes4.Add(p7);
            paragObservacoes4.Add(p8);
            cell1 = new PdfPCell(paragObservacoes4);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table6.AddCell(cell1);

            Paragraph paragObservacoes5 = new Paragraph();
            Phrase p9 = new Phrase("Estampadora selecionada na digitação do serviço: ", Subtitulo);
            Phrase p10 = new Phrase("" + " \n", FontePadrao);
            paragObservacoes5.Add(p9);
            paragObservacoes5.Add(p10);
            cell1 = new PdfPCell(paragObservacoes5);
            cell1.HorizontalAlignment = 0;
            cell1.Colspan = 2;
            cell1.Border = 0;
            table6.AddCell(cell1);

            return table6;
        }
        public static PdfPTable tableAviso(Font Cabecalho, Rectangle page, iTextSharp.text.Image image, Font Subtitulo, Font FontePadrao)
        {
            PdfPTable table7 = new PdfPTable(1);

            PdfPCell cell0 = new PdfPCell(new Phrase("\n AVISO IMPORTANTE PARA O ESTAMPADOR \n", Subtitulo));
            cell0.Colspan = 6;
            cell0.Border = 0;
            cell0.HorizontalAlignment = Element.ALIGN_CENTER;
            table7.AddCell(cell0);

            Paragraph paragrafo = new Paragraph();

            Phrase p1 = new Phrase("\nO estampador deverá registrar a estampagem da placa no ", FontFactory.GetFont("Verdana", 8F, Font.NORMAL, BaseColor.BLACK));
            Phrase p2 = new Phrase("sistema do fabricante de placa ", FontFactory.GetFont("Verdana", 8F, Font.BOLD, BaseColor.BLACK));
            Phrase p3 = new Phrase(" e a informação de que a placa foi afixada no veiculo.Enquanto estes duas etapas não forem registradas, o ", FontFactory.GetFont("Verdana", 8F, Font.NORMAL, BaseColor.BLACK));
            Phrase p4 = new Phrase("VEÍCULO FICARÁ BLOQUEADO ", FontFactory.GetFont("Times New Roman", 8F, Font.BOLD, BaseColor.BLACK));
            Phrase p5 = new Phrase("para novos serviços, até que sejam realizadas. \n ", FontFactory.GetFont("Verdana", 8F, Font.NORMAL, BaseColor.BLACK));

            paragrafo.Add(p1);
            paragrafo.Add(p2);
            paragrafo.Add(p3);
            paragrafo.Add(p4);
            paragrafo.Add(p5);

            PdfPCell cell1 = new PdfPCell(paragrafo);
            table7.AddCell(cell1);

            return table7;
        }

        private static void AddImageInCell(PdfPCell cell, iTextSharp.text.Image image, float fitWidth, float fitHight, int Alignment)
        {
            image.ScaleToFit(166, 55);
            image.Alignment = Alignment;
            cell.AddElement(image);
        }
    }

}

