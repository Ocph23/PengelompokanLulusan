﻿using MainApp.DataAccess.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows;

namespace MainApp.Reports
{
    public class HelperPrint : IDisposable
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;

        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name,
          string fileNameExtension, Encoding encoding,
          string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        internal static List<ReportModel> CreateReportModel(List<Mahasiswa> source)
        {
            List<ReportModel> dataSource = new List<ReportModel>();
           foreach (var item in source)
            {
                dataSource.Add(new ReportModel { Clustering=$"Kluster {item.Clustering}", Gelombang=item.GelombangModel.Nama,
                 HasilTest=item.HasilTest, Nama=item.Nama, IPK=item.IPK, IsCentroid=item.IsCentroid, Jurusan=item.Jurusan.Name, MasaStudi=item.MasaStudi,
                 Nomor=item.Nomor, NPM=item.NPM, Suku=item.SukuModel.Nama});
            }
            return dataSource;
        }

        private void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }
        // Create a local report for Report.rdlc, load the data,
        //    export the report to an .emf file, and print it.

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        internal void PrintDocument(List<ReportDataSource> sources, string layout, ReportParameter[] parameters)
        {
            LocalReport report = new LocalReport();
            report.ReportEmbeddedResource = layout;

            if (parameters != null)
                report.SetParameters(parameters);
            foreach (var item in sources)
            {
                report.DataSources.Add(item);
            }
            Export(report);
            Print();
        }

        public DataTable ToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }


        internal static void PrintWithFormActionTwoSource(string title, ReportDataSource dataHeader, ReportDataSource items, string layout, ReportParameter[] parameters)
        {
            var content = new ReportContent(dataHeader, items, layout, parameters);
            var dlg = new Window
            {
                Content = content,
                AllowsTransparency = false,
                Title = "",
                ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip,
                WindowState = WindowState.Maximized,
            };

            content.WindowClose = dlg.Close;
            var vm = new BaseNotify();
            vm.MyTitle = title;
            dlg.DataContext = vm;
            dlg.ShowDialog();
        }

        internal static void PrintPreviewWithFormAction(string title, ReportDataSource source, string layout, ReportParameter[] parameters)
        {
            //TrireksaApp.Reports.Layouts.Nota.rdlc"
            var content = new ReportContent(source, layout, parameters);
            var dlg = new Window
            {
                AllowsTransparency = false,
                Content = content,
                Title = "",
                ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip,
                WindowState = WindowState.Maximized,
            };

            content.WindowClose = dlg.Close;
            var vm = new BaseNotify();
            vm.MyTitle = title;
            dlg.DataContext = vm;
            dlg.ShowDialog();
        }
    }
}
