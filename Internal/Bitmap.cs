using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
    using System.IO;

namespace XP.Internal
{
    // Bitmap
    // 位图的类。所谓位图即表示图像其本身。
    // 为了在画面中显示位图必需要使用精灵（Sprite）等。
    // 父类Object 
    // 注意：！！！内部图像采用BGRA的格式
    public class Bitmap : Base.GameObject
    {
        private string filename = null;
        // 获取此对象对应的文件名
        public string FileName
        {
            get
            {
                return this.filename;
            }
        }

        // 实际上的位图对象，保证它可读写比较方便
        private WriteableBitmap bitmap = null;

        /// <summary>
        /// 定义到BitmapSource的隐式转换
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static implicit operator BitmapSource(Bitmap bitmap)
        {
            return bitmap.bitmap;
        }

        //类方法new Bitmap(filename) 
        //读取 filename 指定的图像文件，生成 Bitmap 对象。
        //包含在 RGSS-RTP 和加密档案文件内的文件也会自动搜索。可以省略其扩展名。
        public Bitmap(string filename)
        {
            var path = System.IO.Path.GetDirectoryName(filename);
            var name = System.IO.Path.GetFileName(filename) + ".*";
            var ext = System.IO.Path.GetExtension(filename);

            var file = string.IsNullOrEmpty(ext) ? System.IO.Directory.GetFiles(path, name)[0] : filename;

            this.filename = filename;
            var bitmap = new BitmapImage(new Uri(file, UriKind.Relative));
            this.Width = bitmap.PixelWidth;
            this.Height = bitmap.PixelHeight;
            AddImage(bitmap, 0, 0);
        }

        private void AddImage(ImageSource image, double x, double y)
        {
            DrawingVisual visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawImage(image, new System.Windows.Rect(x, y, this.Width, this.Height));
            }
            this.AddVisual(visual);
        }

        //new Bitmap(width, height) 
        //生成指定尺寸的 Bitmap 对象。
        public Bitmap(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public Bitmap(Visual visual, double width, double height)
        {
            this.Width = width;
            this.Height = height;
            //this.AddVisual(visual);

            DrawingVisual v = new DrawingVisual();
            using(var dc = v.RenderOpen())
            {
                var brush = new VisualBrush(visual);
                dc.DrawRectangle(brush, null, new Rect(0, 0, width, height));
            }
            this.AddVisual(v);
        }

        bool _is_disposed = false;
        //方法dispose 
        //释放位图。如果已经释放的话则什么也不做。
        // 位图是否已经释放，实际上不需要释放，等待垃圾回收即可
        public virtual void dispose()
        {
            //this.bitmap = null;
            _is_disposed = true;
            // this.Clear();
        }

        //disposed? 
        //位图已经释放的话则返回真。
        public bool is_disposed
        {
            get
            {
                // return this.bitmap == null;
                // return _is_disposed;
                return false;
            }
        }

        //width 
        //取得位图的宽。
        public new int width
        {
            get
            {
                //return this.bitmap.PixelWidth;
               return (int)this.Width;
            }
        }

        //height 
        //取得位图的高。
        public new int height
        {
            get
            {
                //return this.bitmap.PixelHeight;
                return (int)this.Height;
            }
        }

        //rect 
        //取得位图的矩形（Rect）。
        public Rect rect
        {
            get
            {
                return new Rect(0, 0, this.width, this.height);
            }
        }

        //blt(x, y, src_bitmap, src_rect[, opacity]) 
        //传送 src_bitmap 的矩形 src_rect（Rect）到该位图的座标（x，y）。
        //opacity 指定其不透明度，范围为 0 ～ 255。
        public void blt(int x, int y, Bitmap src_bitmap, Rect src_rect, double opacity = 255)
        {
            DrawingVisual visual = new DrawingVisual();
            VisualBrush brush = new VisualBrush(src_bitmap);
            brush.Opacity = opacity / 255.0;
            brush.ViewboxUnits = BrushMappingMode.Absolute;
            brush.Viewbox = src_rect;

            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(brush, null, new Rect(x, y, src_rect.Width, src_rect.Height));
            }
            this.AddVisual(visual);
        }


        //stretch_blt(dest_rect, src_bitmap, src_rect[, opacity]) 
        //传送 src_bitmap 的矩形 src_rect（Rect）到该位图的矩形 dest_rect（Rect）。
        //opacity 指定其不透明度，范围为 0 ～ 255。
        public void stretch_blt(Rect dest_rect, Bitmap src_bitmap, Rect src_rect, int opacity = 255)
        {
            DrawingVisual visual = new DrawingVisual();
            VisualBrush brush = new VisualBrush(src_bitmap);
            brush.Opacity = opacity / 255.0;
            brush.ViewboxUnits = BrushMappingMode.Absolute;
            brush.Viewbox = src_rect;

            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(brush, null, new Rect(dest_rect.X, dest_rect.Y, src_rect.Width, src_rect.Height));
            }
            this.AddVisual(visual);
        }

        //fill_rect(x, y, width, height, color) 
        //fill_rect(rect, color) 
        //以 color（Color）颜色填充该位图的矩形（x，y，width，height）或 rect（Rect）。
        public void fill_rect(int x, int y, int width, int height, Color color)
        {
            fill_rect(new Rect(x, y, width, height), color);
        }

        public void fill_rect(Rect rect, Color color)
        {
            DrawingVisual visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(new SolidColorBrush(color), null, rect);
            }
            this.AddVisual(visual);
        }
        //clear 
        //清除位图全体。
        public void clear()
        {
            this.Clear();
        }

        //get_pixel(x, y) 
        //取得点（x，y）的颜色（Color）。
        public Color get_pixel(int x, int y)
        {
            var bmp = new RenderTargetBitmap(this.width, this.height, 96.0, 96.0, System.Windows.Media.PixelFormats.Bgra32);
            bmp.Render(this);

            //定义切割矩形
            var cut = new Int32Rect(x, y, 1, 1);
            //计算Stride
            var stride = bitmap.Format.BitsPerPixel * cut.Width / 8;
            //声明字节数组
            byte[] bytes = new byte[cut.Height * stride];
            //调用CopyPixels
            bitmap.CopyPixels(cut, bytes, stride, 0);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }

        //set_pixel(x, y, color) 
        //设定点（x，y）的颜色为 color（Color）。
        public void set_pixel(int x, int y, Color color)
        {
            fill_rect(x, y, 1, 1, color);
        }

        //hue_change(hue) 
        //转换色相。hue 指定色相（360 度）的位移。
        //这个处理会花费一些时间。而且，因为转换误差，多次反复转换会丢失颜色。
        public void hue_change(double hue)
        {
            var bmp = new RenderTargetBitmap(this.width, this.height, 96.0, 96.0, System.Windows.Media.PixelFormats.Bgra32);
            bmp.Render(this);

            var writableBitmap = new WriteableBitmap(bmp);

            //Utility.ColorUtilities.ConvertRgbToHsv()
            var bytePerPixel = writableBitmap.Format.BitsPerPixel / 8;
            var stride = bytePerPixel * (int)writableBitmap.PixelWidth;

            var width = writableBitmap.PixelWidth;
            var height = writableBitmap.PixelHeight;
            // 需要处理透明度
            unsafe
            {
                var p = (byte*)writableBitmap.BackBuffer.ToPointer();
                writableBitmap.Lock();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        var index = i * stride + j * bytePerPixel;
                        var hsv = Utility.ColorUtilities.ConvertRgbToHsv(p[index], p[index + 1], p[index + 2]);
                        var color = Utility.ColorUtilities.ConvertHsvToRgb(hue, hsv.S, hsv.V);
                        p[index] = color.R;
                        p[index + 1] = color.G;
                        p[index + 2] = color.B;
                    }
                }
                writableBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
                writableBitmap.Unlock();
            }

            this.Clear();
            AddImage(writableBitmap, 0, 0);
        }
        //draw_text(x, y, width, height, str[, align]) 
        //draw_text(rect, str[, align]) 
        //在该位图的矩形（x，y，width，height）或 rect（Rect）中描绘字符串 str。
        //如果文本的长度超过矩形的宽度的话，则自动缩放为 60% 再描绘。
        //水平方向默认为左对齐，但是当 align 指定为 1 时为居中对齐，指定为 2 时为右对齐。垂直方向则总为居中对齐。
        //这个处理需要花费时间，尽量不要在每 1 帧中重描绘字符串。
        public void draw_text(double x, double y, double width, double height, string str, int align = 0)
        {
            int w, h;
            var fontSize = (float)this.font.size;
            FormattedText formattedText = null;
            var typeface = new Typeface("Verdana");
            var brush = new SolidColorBrush(this.font.color);
            // 求出合适的字体大小
            do
            {
                if (formattedText != null)
                    fontSize *= 0.6f;

                formattedText = new FormattedText(
                    str,
                    System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    typeface,
                    fontSize,
                    brush);

                w = (int)Math.Ceiling(formattedText.Width);
                h = (int)Math.Ceiling(formattedText.Height);
            } while (w > width || h > height);
            // 计算文本位置

            double realY = y + (height - h) / 2;
            double realX = x;
            switch (align)
            {
                case 1: realX = x + (width - w) / 2; break;
                case 2: realX = x + width - w; break;
            }

            //text.TextAlignment = TextAlignment.Center;
            DrawingVisual visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                dc.DrawText(formattedText, new Point(realX, realY));
            }
            this.AddVisual(visual);
        }
        public void draw_text(Rect rect, string str, int align = 0)
        {
            draw_text(rect.X, rect.Y, rect.Width, rect.Height, str, align);
        }

        public Rect text_size(string str, double fontSize)
        {
            var typeface = new Typeface("Verdana");
            var brush = new SolidColorBrush(this.font.color);

            var formattedText = new FormattedText(
                    str,
                    System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    typeface,
                    fontSize,
                    brush);
            var w = (int)Math.Ceiling(formattedText.Width);
            var h = (int)Math.Ceiling(formattedText.Height);
            return new Rect(0, 0, w, h);
        }

        //text_size(str) 
        //取得以 draw_text 方法描绘字符串 str 的矩形（Rect）。但是，不包含斜体的部分。
        public Rect text_size(string str)
        {
            return text_size(str, this.font.size);
        }

        public Bitmap clone()
        {
            Bitmap bitmap = new Bitmap(this.width, this.height);

            DrawingVisual visual = new DrawingVisual();
            VisualBrush brush = new VisualBrush(this);

            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(brush, null, new System.Windows.Rect(0, 0, this.width, this.height));
            }

            bitmap.AddVisual(visual);
            return bitmap;
        }

        //属性font 
        //是用 draw_text 方法描绘字符串时使用的字体（Font）。
        Font _font = new Font();
        public Font font
        {
            get { return _font; }
            set { _font = value; }
        }

        // 设置Bitmap的源是否，要显示哪个区域

        public Rect Viewbox
        {
            set
            {
                this.OpacityMask = new DrawingBrush()
                {
                    ViewportUnits = BrushMappingMode.Absolute,
                    Viewport = value,
                    Drawing = Global.ViewportDrawing
                };
            }
        }
    }
}
