using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

//http://social.msdn.microsoft.com/Forums/vstudio/en-US/7e7eeb4b-d393-4a59-bcd6-c2f9c81a02c1/draw-line-on-bitmap

namespace BitmapCalendar
{
    public class BitmapCalendarRenderer
    {
        private const int margin = 10;
        private const int textmargin = 5; 

        public Bitmap DrawThisMonthsCalendar(int width, int height, int fontSize, Color drawColour, Color alternateColor)
        {
            var image = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.Transparent);
                var pen = new Pen(drawColour) {Width = 3};

                //draw grid...
                int cellWidth = ((width - (margin * 2)) / 7);
                int cellHeight = ((height - (margin * 2)) / 7);

                //horizontal lines
                int maxWidth = margin + 7*cellWidth;
                for (int i = 0; i < 8; i++)
                {
                    int top = margin + i*cellHeight;
                    g.DrawLine(pen, new Point(margin, top), new Point(maxWidth, top));
                }

                //vertical lines
                int maxHeight = margin + 7*cellHeight;
                for (int i = 0; i < 8; i++)
                {
                    int left = margin + i*cellWidth;
                    g.DrawLine(pen, new Point(left, margin), new Point(left, maxHeight));
                }

                //highlight current day cell
                var now = DateTime.Now.Date;
                var coords = GetDateCoords(now, cellWidth, cellHeight);
                g.FillRectangle(new SolidBrush(drawColour), coords.X, coords.Y, cellWidth, cellHeight);

                //days of the week
                int count = 0;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                foreach (var dow in (DayOfWeek[]) Enum.GetValues(typeof (DayOfWeek)))
                {
                    int left = margin + textmargin + count*cellWidth;
                    g.DrawString(GetDayOfWeekAbbreviation(dow), new Font("Tahoma", fontSize), new SolidBrush(drawColour),
                                 left, margin + textmargin);
                    count++;
                }

                //date numbers
                for (int d = 1; d <= DateTime.DaysInMonth(now.Year, now.Month); d++)
                {
                    var drawDate = new DateTime(now.Year, now.Month, d); 
                    coords = GetDateCoords(drawDate, cellWidth, cellHeight);
                    var textColor = drawDate.Date == now.Date ? alternateColor : drawColour; 
                    g.DrawString(d.ToString(CultureInfo.InvariantCulture), new Font("Tahoma", fontSize), new SolidBrush(textColor),
                                 coords.X, coords.Y);
                }
            }

            return image; 
        }

        private Coordinates GetDateCoords(DateTime date, int cellWidth, int cellHeight)
        {
            //TODO:get correct cell coordinates for date
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1); 
            var cellOffset = firstDayOfMonth.DayOfWeek.GetHashCode() - 1;
            var dayCellNumber = date.Day + cellOffset;
            var cellX = dayCellNumber%7;
            var cellY = dayCellNumber/7 + 1; 

            return new Coordinates(margin + cellWidth * cellX, margin + cellHeight * cellY);
        }

        private string GetDayOfWeekAbbreviation(DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Sunday:
                    return "su";
                case DayOfWeek.Monday:
                    return "mo";
                case DayOfWeek.Tuesday:
                    return "tu";
                case DayOfWeek.Wednesday:
                    return "we";
                case DayOfWeek.Thursday:
                    return "th";
                case DayOfWeek.Friday:
                    return "fr";
                case DayOfWeek.Saturday:
                    return "sa";
                default:
                    return string.Empty;
            }
        }
    }

    internal class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
