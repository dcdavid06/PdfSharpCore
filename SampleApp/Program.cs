
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp;
using System;

namespace SampleApp 
{


    public class Program
    {
        

        private static string GetOutFilePath(string name)
        {
            string OutputDirName = @".";
            return System.IO.Path.Combine(OutputDirName, name);
        }


        private static void SaveDocument(PdfSharpCore.Pdf.PdfDocument document, string name)
        {
            string outFilePath = GetOutFilePath(name);
            string? dir = System.IO.Path.GetDirectoryName(outFilePath);
            if (dir != null && !System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            document.Save(outFilePath);
        }


        public static void Main(string[] args)
        {
            System.Console.WriteLine("Starting...");

            const string outName = "test1.pdf";

            PdfSharpCore.Pdf.PdfDocument? document = new PdfSharpCore.Pdf.PdfDocument();

            PdfSharpCore.Pdf.PdfPage? pageNewRenderer = document.AddPage();

            PdfSharpCore.Drawing.XGraphics? gfx = PdfSharpCore.Drawing.XGraphics.FromPdfPage(pageNewRenderer);

            XRect rect = new XRect(new XPoint(), gfx.PageSize);
            rect.Inflate(-10, -15);
            XFont font = new XFont("Verdana", 14, XFontStyle.Bold);
            //gfx.DrawString("Test Title", font, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);

            rect.Offset(0, 5);
            font = new XFont("Verdana", 8, XFontStyle.Italic);
            XStringFormat format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Far;
            gfx.DrawString("Created with " + "David", font, XBrushes.DarkOrchid, rect, format);

            font = new XFont("Verdana", 8);
            format.Alignment = XStringAlignment.Center;
            gfx.DrawString(document.PageCount.ToString(), font, XBrushes.DarkOrchid, rect, format);


            BeginBox(gfx, 1, "Accounts by Status Report");
            int x1 = 12;
            for (int i = 0; i < 10; i++)
            {

                gfx.DrawString($"Item {i}", new XFont("Arial", 6)
                , XBrushes.Black
                , new XPoint(12, x1 - 2));
                gfx.DrawLine(XPens.Black, 0, x1 , 250, x1);
                x1 += 12;
            }
            EndBox(gfx);


            gfx.DrawString(
                  $"Accounts by Status Report as of {DateTime.UtcNow.ToLongDateString()}"
                , new PdfSharpCore.Drawing.XFont("Arial", 8)
                , PdfSharpCore.Drawing.XBrushes.Black
                , new PdfSharpCore.Drawing.XPoint(12, 12)
            );

            SaveDocument(document, outName);

            System.Console.WriteLine("Done!");
        } // End Sub Main 

        protected static XColor BackColor;
        protected static XColor BackColor2;
        protected static XColor ShadowColor;
        protected static double BorderWidth;
        protected static XPen BorderPen;

        public static void BeginBox(XGraphics gfx, int number, string title)
        {
            BackColor = XColors.Ivory;
            BackColor2 = XColors.WhiteSmoke;

            BackColor = XColor.FromArgb(212, 224, 240);
            BackColor2 = XColor.FromArgb(253, 254, 254);

            ShadowColor = XColors.Gainsboro;
            BorderWidth = .5;
            BorderPen = new XPen(XColor.FromArgb(94, 118, 151), BorderWidth);

            const int dEllipse = 15;
            var rect = new XRect(0, 20, 300, 200);
            if (number % 2 == 0)
                rect.X = 300 - 5;
            rect.Y = 40 + ((number - 1) / 2) * (200 - 5);
            rect.Inflate(-10, -10);
            var rect2 = rect;
            rect2.Offset(BorderWidth, BorderWidth);
            gfx.DrawRoundedRectangle(new XSolidBrush(ShadowColor), rect2, new XSize(dEllipse + 8, dEllipse + 8));
            var brush = new XLinearGradientBrush(rect, BackColor, BackColor2, XLinearGradientMode.Vertical);
            gfx.DrawRoundedRectangle(BorderPen, brush, rect, new XSize(dEllipse, dEllipse));
            rect.Inflate(-5, -5);

            var font = new XFont("Verdana", 12, XFontStyle.Regular);
            gfx.DrawString(title, font, XBrushes.Navy, rect, XStringFormats.TopCenter);

            rect.Inflate(-10, -5);
            rect.Y += 20;
            rect.Height -= 20;

            _state = gfx.Save();
            gfx.TranslateTransform(rect.X, rect.Y);
        }

        public static void EndBox(XGraphics gfx)
        {
            gfx.Restore(_state);
        }

        static XGraphicsState _state;

    } // End Class Program 


} // End Namespace SampleApp 
