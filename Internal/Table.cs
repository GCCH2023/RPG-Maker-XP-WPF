using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //        Table
    //多维数组的类。各单元为带符号 2 字节型，即是从 -32,768 到 32,767 范围的整数。
    //在处理大量数据时，Ruby 的 Array 类运行效率会很差，这样就要使用这个类。
    public class Table
    {
        public Table()
        {

        }
        /// <summary>
        /// 必须定义属性，不然无法从xml序列化
        /// </summary>
        public List<int> data
        {
            get;
            set;
        }
        //父类Object 
        //类方法new Table(xsize[, ysize[, zsize]]) 
        //生成 Table 对象。指定多维数组各维的尺寸。能生成 1 ～ 3 维的数组。生成单元数为 0 的数组也是可能的。
        public Table(int xsize, int ysize = 1, int zsize = 1)
        {
            this._xsize = xsize;
            this._ysize = ysize;
            this._zsize = zsize;
            var count = xsize * ysize * zsize;
            data = new List<int>(count);
            for (int i = 0; i < count; i++)
                data.Add(0);
        }
        //方法resize(xsize[, ysize[, zsize]]) 
        //更改数组的尺寸。更改前的数据被保留。
        public void resize(int xsize, int ysize = 1, int zsize = 1)
        {
            this._xsize = xsize;
            this._ysize = ysize;
            this._zsize = zsize;

            var count = xsize * ysize * zsize;
            data = new List<int>(count);
            for (int i = 0; i < count; i++)
                data.Add(0);
        }
        //xsize 
        int _xsize;

        public int xsize
        {
            get { return _xsize; }
            set 
            {
                _xsize = value;
            }
        }
        //ysize 
        int _ysize;

        public int ysize
        {
            get { return _ysize; }
            set { _ysize = value;}
        }
        //zsize 
        int _zsize;

        public int zsize
        {
            get { return _zsize; }
            set { _zsize = value;}
        }

        //取得数组各维的尺寸。


        //属性this[x] 
        public int this[int x] 
        {
            get
            {
                if (x >= 0 && x < xsize)
                    return this.data[x];
                return 0;
            }
            set
            {
                this.data[x] = value;
            }
        }
        //this[x, y] 
        public int this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < xsize && y >= 0 && y <= ysize)
                    return this.data[y * xsize + x];
                return 0;
            }
            set
            {
                this.data[y * xsize + x] = value;
            }
        }
        //this[x, y, z] 
        public int this[int x, int y, int z]
        {
            get
            {
                if (x >= 0 && x < xsize && y >= 0 && y <= ysize && z >= 0 && z < zsize)
                    return this.data[z * (xsize * ysize) + y * xsize + x];
                return 0;
            }
            set
            {
                this.data[z * (xsize * ysize) + y * xsize + x] = value;
            }
        }
        //访问数组的单元。采用与生成数组相同维数的参数。当指定的单元不存在时返回 null。


        internal static unsafe Table Load(byte* p, int length)
        {
            int xsize = *(int *)(p + 4);
            int ysize = *(int *)(p + 8);
            int zsize = *(int *)(p + 12);
            short* data = (short*)(p + 20);

            Table table = new Table(xsize, ysize, zsize);

            for (int z = 0; z < zsize;z++)
            {
                for (int y = 0; y < ysize;y++)
                {
                    for (int x = 0; x < xsize; x++)
                    {
                        table.data[z * (xsize * ysize) + y * xsize + x] = *(data + x + y * xsize + z * ysize * xsize);
                    }
                }
            }
            return table;
        }
    }
}
