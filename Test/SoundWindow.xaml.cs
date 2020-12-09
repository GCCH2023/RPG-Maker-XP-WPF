using Microsoft.DirectX.DirectSound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XP.Test
{
    /// <summary>
    /// SoundWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SoundWindow : Window
    {
        ////缓冲区对象
        //private SecondaryBuffer secBuffer;
        ////设备对象
        //private Device secDev;
        public SoundWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var fileName = @"016-Theme05.mid";
            //DevicesCollection sound_devices = new DevicesCollection();
            //Device sound_device_output = new Device(sound_devices[0].DriverGuid);
            ////设置设备协作级别
            //sound_device_output.SetCooperativeLevel(new WindowInteropHelper(this).Handle, CooperativeLevel.Normal);
            ////创建辅助缓冲区
            //SecondaryBuffer secondary_buffer = new SecondaryBuffer(fileName, sound_device_output);
            ////设置缓冲区为循环播放
            //secondary_buffer.Play(0, BufferPlayFlags.Looping);
        }
    }
}
