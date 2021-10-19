using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Pdf;
using System.Text.Encodings;
using System.Data.SQLite;

namespace Diplom
{
    public partial class Doc : Form
    {
        private double a1;
        private double a2;
        private double a3;
        private int IdUser;
        string name;

        private SQLiteConnection conn = new SQLiteConnection("Data Source=dpbd.db;");

        public Doc( int id)
        {
            InitializeComponent();
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(-1);
            dateTimePicker2.MaxDate = DateTime.Now;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1254");
            IdUser = id;
        }

        void LoadT()
        {

                string cm = "SELECT SUM(Operations.sum) FROM Operations WHERE @date1 <= Operations.datetime <= @date2 AND Operations.type = 2";
                SQLiteCommand cmd = new SQLiteCommand(cm, conn);
                cmd.Parameters.Add("@date1", DbType.DateTime).Value = dateTimePicker1.Value;
                cmd.Parameters.Add("@date2", DbType.DateTime).Value = dateTimePicker2.Value;

                conn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    a1 = reader.GetDouble(0);
                }
                reader.Close();
                conn.Close();

                cm = "SELECT SUM(Operations.sum) FROM Operations WHERE @date1 <= Operations.datetime <= @date2 AND Operations.type = 3";
                cmd = new SQLiteCommand(cm, conn);
                cmd.Parameters.Add("@date1", DbType.DateTime).Value = dateTimePicker1.Value;
                cmd.Parameters.Add("@date2", DbType.DateTime).Value = dateTimePicker2.Value;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    a2 = reader.GetDouble(0);
                }
                reader.Close();
                conn.Close();

                cm = "SELECT SUM(Operations.sum) FROM Operations WHERE @date1 <= Operations.datetime <= @date2 AND Operations.type = 1";
                cmd = new SQLiteCommand(cm, conn);
                cmd.Parameters.Add("@date1", DbType.DateTime).Value = dateTimePicker1.Value;
                cmd.Parameters.Add("@date2", DbType.DateTime).Value = dateTimePicker2.Value;

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    a3 = reader.GetDouble(0);
                }
                reader.Close();
                conn.Close();

            cm = "SELECT Users.surname || ' ' || Users.name || ' ' || Users.patronymic AS 'ФИО' FROM Users WHERE id = @id";
            cmd = new SQLiteCommand(cm, conn);
            cmd.Parameters.Add("@id", DbType.Int32).Value = IdUser;

            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                name = reader.GetString(0);
            }
            reader.Close();
            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadT();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1254");
            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;//стандартный размер страницы
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Portrait;//ориентация
            section.PageSetup.BottomMargin = 10;//нижний отступ
            section.PageSetup.TopMargin = 10;//верхний отступ

            MigraDoc.DocumentObjectModel.Paragraph paragraph = section.AddParagraph();
            double fontHeight = 14;
            MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font("Times New Roman", fontHeight);

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText("ИП 'Мамедов'");

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText("Финансовый отчет");

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText("От: " + dateTimePicker1.Value.Date.ToString("dd:MM:yyyy"));

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText("До: " + dateTimePicker2.Value.Date.ToString("dd:MM:yyyy"));

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText(" ");


            Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            table.Rows.Alignment = RowAlignment.Center;

            table.Format.Alignment = (ParagraphAlignment)HorizontalAlignment.Center;
            table.Borders.Width = 2;
            table.Rows.HeightRule = RowHeightRule.Auto;
            Column column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Center;
            
            Row row = table.AddRow();
            row.Borders.Visible = true;
            row.Borders.Width = 2;
            fontHeight = 12;
            font = new MigraDoc.DocumentObjectModel.Font("Times New Roman", fontHeight);
            Cell cell = row.Cells[0];
            cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph("Закупки на сумму");
            cell = row.Cells[1];
            cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph("Списания на сумму");
            cell = row.Cells[2];
            cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph("Продажи на сумму");
            cell = row.Cells[3];
            cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph("Выручка");

            row = table.AddRow();
            row.Borders.Visible = true;
            row.Borders.Width = 2;
            fontHeight = 12;
            font = new MigraDoc.DocumentObjectModel.Font("Times New Roman", fontHeight);
            cell = row.Cells[0];
            cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph(a1.ToString());
            cell = row.Cells[1];
            cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph(a2.ToString());
            cell = row.Cells[2];
            cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph(a3.ToString());
            cell = row.Cells[3];
            cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.AddParagraph((a3 - a1).ToString());

            document.LastSection.Add(table);

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText(" ");

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText("Выполнил: " + name);

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText("Дата: " + DateTime.Now.Date.ToString("dd:MM:yyyy"));

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText(" ");

            paragraph = new Paragraph();
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.Format.Font.ApplyFont(font);
            section.Add(paragraph);
            paragraph.AddText("Подпись _____________");

            MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer = new MigraDoc.Rendering.PdfDocumentRenderer(true);
            pdfRenderer.Document = document;
            
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save("otchet.pdf");
            string fn = "otchet.pdf";
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = fn;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = dateTimePicker2.Value.AddDays(-1);
        }
    }
}
