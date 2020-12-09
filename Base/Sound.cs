////using System;
////using System.Collections.Generic;
////using System.ComponentModel;
////using System.Data;
////using System.Drawing;
////using System.Text;
////using System.Windows.Forms;

//////引用的命名空间
////using Microsoft.DirectX.DirectSound;

////namespace XP.Base
////{
////    public class Sound
////    {
////        //缓冲区对象
////        private SecondaryBuffer secBuffer;
////        //设备对象
////        private Device secDev;

////        public Sound()
////        {

////        }

////        private void button2_Click(object sender, EventArgs e)
////        {
////            if (textBox1.Text.Length > 0)
////            {
////                secDev = new Device();
////                secDev.SetCooperativeLevel(this, CooperativeLevel.Normal);//设置设备协作级别
////                secBuffer = new SecondaryBuffer(textBox1.Text, secDev);//创建辅助缓冲区
////                secBuffer.Play(0, BufferPlayFlags.Looping);//设置缓冲区为循环播放
////            }
////        }

////        private void button3_Click(object sender, EventArgs e)
////        {
////            if (textBox1.Text.Length > 0)
////            {
////                secDev = new Device();
////                secDev.SetCooperativeLevel(this, CooperativeLevel.Normal);//设置设备协作级别
////                BufferDescription buffDes = new BufferDescription();
////                buffDes.GlobalFocus = true;//设置缓冲区全局获取焦点
////                buffDes.ControlVolume = true;//指明缓冲区可以控制声音
////                buffDes.ControlPan = true;//指明缓冲区可以控制声道平衡
////                secBuffer = new SecondaryBuffer(textBox1.Text, buffDes, secDev);//创建辅助缓冲区
////                secBuffer.Play(0, BufferPlayFlags.Looping);//设置缓冲区为循环播放
////            }
////        }

////        private void button4_Click(object sender, EventArgs e)
////        {
////            if (secBuffer != null)
////            {
////                secBuffer.Stop();
////            }
////        }

////        private void trackBar1_Scroll(object sender, EventArgs e)
////        {
////            if (secBuffer != null)
////            {
////                secBuffer.Volume = -trackBar1.Value * 400;//音量为0表示最大的音量，因此设置时必须为负。
////            }
////        }

////        private void trackBar2_Scroll(object sender, EventArgs e)
////        {
////            if (secBuffer != null)
////            {
////                if (trackBar2.Value == 0)
////                {
////                    secBuffer.Pan = Convert.ToInt32(Pan.Left);//左声道
////                }
////                else if (trackBar2.Value == 2)
////                {
////                    secBuffer.Pan = Convert.ToInt32(Pan.Right);//右声道
////                }
////                else
////                {
////                    secBuffer.Pan = Convert.ToInt32(Pan.Center);
////                }
////            }
////        }

////    }
////}
//using System;
//using System.IO;
//using System.Diagnostics;
//using System.Text;
//using System.Collections;
//using System.Windows.Forms;
//using System.Threading;
//using Microsoft.DirectX.DirectSound;

//namespace TG.Sound
//{
//    #region Enumerations code
//    public enum OggSampleSize { EightBits, SixteenBits }
//    #endregion

//    public delegate void PlayOggFileEventHandler(object sender, PlayOggFileEventArgs e);

//    public delegate void StopOggFileEventHandler(object sender, PlayOggFileEventArgs e);

//    public sealed class PlayOggFileEventArgs : EventArgs
//    {
//        #region Members and events code
//        private bool success;
//        private string reasonForFailure; // if !Success then this is the explanation for the failure 
//        private int playId;	// the value of the playID parameter when PlayOggFile() was called 

//        //NOTE: The Ogg Vorbis decoder may have encountered errors while decoding the Ogg Vorbis data. 
//        //		These two error counts are for information purposes only, since if Success the 
//        //		created waveform data was played, but it may not have sounded as intended if either of  
//        //		these two counts are nonzero. 
//        public int ErrorHoleCount, // count of encountered OV_HOLE errors during decoding 
//            // indicates there was an interruption in the data. 
//                ErrorBadLinkCount; // count of encountered OV_EBADLINK errors during decoding 
//        // indicates that an invalid stream section was supplied to libvorbisfile,  
//        // or the requested link is corrupt. 
//        #endregion

//        #region Properties code
//        public bool Success
//        {
//            get { return success; }
//            set { success = value; }
//        }

//        public string ReasonForFailure
//        {
//            get { return reasonForFailure; }
//            set { reasonForFailure = value; }
//        }

//        public int PlayId
//        {
//            get { return playId; }
//            set { playId = value; }
//        }
//        #endregion

//        #region Initialization code
//        public PlayOggFileEventArgs(int Id)
//        {
//            this.PlayId = Id;
//        }

//        public PlayOggFileEventArgs(bool success, string reason, int Id, int errorHoleCount, int errorBadLinkCount)
//        {
//            this.Success = success;
//            this.ReasonForFailure = reason;
//            this.PlayId = Id;
//            this.ErrorBadLinkCount = errorBadLinkCount;
//            this.ErrorHoleCount = errorHoleCount;
//        }
//        #endregion
//    }

//    public class OggPlay : IDisposable
//    {
//        #region Members and events code
//        private const int WaitTimeout = 5000;
//        private Device directSoundDevice;
//        private OggSampleSize oggFileSampleSize;
//        private bool disposed;

//        public event PlayOggFileEventHandler PlayOggFileResult;
//        public event StopOggFileEventHandler StopOggFileNow;
//        #endregion

//        #region Properties code
//        public Device DirectSoundDevice
//        {
//            get { return directSoundDevice; }
//            set { directSoundDevice = value; }
//        }

//        public OggSampleSize OggFileSampleSize
//        {
//            get { return oggFileSampleSize; }
//            set { oggFileSampleSize = value; }
//        }
//        #endregion

//        #region Intialization code
//        public OggPlay(Control owner, OggSampleSize wantedOggSampleSize)
//        {
//            // set DirectSoundDevice 
//            DirectSoundDevice = new Device();

//            // NOTE: the DirectSound docs recommend CooperativeLevel.Priority for games 
//            DirectSoundDevice.SetCooperativeLevel(owner, CooperativeLevel.Priority);

//            // set OggSampleSize 
//            OggFileSampleSize = wantedOggSampleSize;
//        }
//        #endregion

//        #region IDispose implementation
//        protected virtual void Dispose(bool disposing)
//        {
//            lock (this)
//            {
//                // Do nothing if the object has already been disposed of 
//                if (disposed)
//                    return;

//                if (disposing)
//                {
//                    // Release disposable objects used by this instance here 

//                    // cleanup DirectSound Device 
//                    if (DirectSoundDevice != null)
//                    {
//                        DirectSoundDevice.Dispose();
//                        DirectSoundDevice = null;
//                    }
//                }

//                // Release unmanaged resource here. Don't access reference type fields 

//                // Remember that the object has been disposed of 
//                disposed = true;
//            }
//        }

//        public void Dispose()
//        {
//            Dispose(true);

//            // Unregister object for finalization 
//            GC.SuppressFinalize(this);
//        }

//        ~OggPlay()
//        {
//            Dispose(false);
//        }
//        #endregion

//        #region Ogg playback commands code
//        public void PlayOggFile(string fileName, int playId)
//        {
//            PlayOggFileEventArgs EventArgs = new PlayOggFileEventArgs(playId);

//            // decode the ogg file in a separate thread 
//            PlayOggFileThreadInfo pofInfo = new PlayOggFileThreadInfo(
//                EventArgs, fileName, OggFileSampleSize == OggSampleSize.EightBits ? 8 : 16,
//                DirectSoundDevice, this);

//            Thread PlaybackThread = new Thread(new ThreadStart(pofInfo.PlayOggFileThreadProc));
//            PlaybackThread.Start();
//            Thread.Sleep(0);  // yield the rest of timeslice so new thread can start right away 
//        }

//        public void StopOggFile(int playId)
//        {
//            PlayOggFileEventArgs EventArgs = new PlayOggFileEventArgs(playId);
//            StopOggFileNow(this, EventArgs);
//        }
//        #endregion

//        class PlayOggFileThreadInfo
//        {
//            #region Members and events code
//            private string fileName;
//            private int bitsPerSample;  // either 8 or 16 
//            private bool stopNow;
//            private PlayOggFileEventArgs eventArgs;
//            private Device directSoundDevice;
//            private OggPlay oggPlay;
//            #endregion

//            #region Properties code
//            public PlayOggFileEventArgs EventArgs
//            {
//                get { return eventArgs; }
//                set { eventArgs = value; }
//            }

//            public string FileName
//            {
//                get { return fileName; }
//                set { fileName = value; }
//            }

//            public int BitsPerSample
//            {
//                get { return bitsPerSample; }
//                set { bitsPerSample = value; }
//            }

//            public Device DirectSoundDevice
//            {
//                get { return directSoundDevice; }
//                set { directSoundDevice = value; }
//            }

//            public OggPlay oplay
//            {
//                get { return oggPlay; }
//                set { oggPlay = value; }
//            }
//            #endregion

//            #region Initialization code
//            public PlayOggFileThreadInfo(PlayOggFileEventArgs eventArgs, string fileName, int bitsPerSample,
//                Device directSound, OggPlay oPlay)
//            {
//                this.EventArgs = eventArgs;
//                this.FileName = fileName;
//                this.BitsPerSample = bitsPerSample;
//                this.DirectSoundDevice = directSound;
//                this.oplay = oPlay;

//                // add the interrupt event handler 
//                oplay.StopOggFileNow += new StopOggFileEventHandler(InterruptOggFilePlayback);
//            }
//            #endregion

//            #region Interrupt thread handler code
//            private void InterruptOggFilePlayback(object sender, PlayOggFileEventArgs e)
//            {
//                if (e.PlayId == EventArgs.PlayId)
//                {
//                    stopNow = true;
//                }
//            }
//            #endregion

//            public void PlayOggFileThreadProc()
//            {
//                // call as needed the external C functions to decode the ogg file 
//                unsafe
//                {
//                    void* vf = (void*)0;
//                    SecondaryBuffer SecBuf = null;
//                    BufferDescription MyDescription = null;
//                    Notify MyNotify = null;

//                    try
//                    {
//                        // initialize 
//                        int ErrorCode = NativeMethods.init_for_ogg_decode(FileName, &vf);
//                        Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder successfully initialized.");

//                        if (ErrorCode != 0)
//                        {
//                            // NOTE: these ErrorCode values and comments are copied from init_for_ogg_decode() 
//                            const int
//                                ifod_err_open_failed = 1,              // open file failed 
//                                ifod_err_malloc_failed = 2,            // malloc() call failed; out of memory 
//                                ifod_err_read_failed = 3,              // A read from media returned an error      
//                                ifod_err_not_vorbis_data = 4,          // Bitstream is not Vorbis data        
//                                ifod_err_vorbis_version_mismatch = 5,  // Vorbis version mismatch 
//                                ifod_err_invalid_vorbis_header = 6,    // Invalid Vorbis bitstream header 
//                                ifod_err_internal_fault = 7,           // Internal logic fault; indicates a bug or heap/stack corruption 
//                                ifod_err_unspecified_error = 8;        // ov_open() returned an undocumented error 

//                            // prefix the reason 
//                            EventArgs.ReasonForFailure =
//                                "Ogg Vorbis decoder initialization for ogg file '" + FileName + "' failed: ";

//                            // suffix the reason 
//                            switch (ErrorCode)
//                            {
//                                case ifod_err_open_failed:
//                                    EventArgs.ReasonForFailure += "Unable to open the ogg file.";
//                                    break;

//                                case ifod_err_malloc_failed:
//                                    EventArgs.ReasonForFailure += "Out of memory.";
//                                    break;

//                                case ifod_err_read_failed:
//                                    EventArgs.ReasonForFailure += "A read from media returned an error.";
//                                    break;

//                                case ifod_err_not_vorbis_data:
//                                    EventArgs.ReasonForFailure += "Bitstream is not Vorbis data.";
//                                    break;

//                                case ifod_err_vorbis_version_mismatch:
//                                    EventArgs.ReasonForFailure += "Vorbis version mismatch.";
//                                    break;

//                                case ifod_err_invalid_vorbis_header:
//                                    EventArgs.ReasonForFailure += "Invalid Vorbis bitstream header.";
//                                    break;

//                                case ifod_err_internal_fault:
//                                    EventArgs.ReasonForFailure +=
//                                        "Internal logic fault; indicates a bug or heap/stack corruption.";
//                                    break;

//                                case ifod_err_unspecified_error:
//                                    EventArgs.ReasonForFailure += "Vorbis ov_open() returned an undocumented error.";
//                                    break;

//                                default:
//                                    Debug.Assert(false);
//                                    break;
//                            }

//                            oplay.PlayOggFileResult(this, EventArgs);
//                            return;
//                        }

//                        byte[] PcmBuffer = new byte[4096]; // 4096 is the vorbis-recommended size 
//                        bool FirstTime, AtEOF, FormatChanged;
//                        int ChannelsCount = 0, SamplingRate = 0,
//                            PreviousChannelCount = 0, PreviousSamplingCount = 0,
//                            PcmBytes;
//                        int AverageBytesPerSecond = 0,
//                            BlockAlign = 0;

//                        WaveFormat MyWaveFormat = new WaveFormat();

//                        // NOTE: DirectSound documentation recommends from 1 to 2 seconds  
//                        //       for buffer size, so 1.2 is an arbitrary but good choice. 
//                        double SecBufHoldThisManySeconds = 1.2;
//                        int SecBufByteSize = 0,
//                            SecBufNextWritePosition = 0,
//                            SecBufPlayPositionWhenNextWritePositionSet = 0;
//                        AutoResetEvent
//                            SecBufNotifyAtBegin = new AutoResetEvent(false),
//                            SecBufNotifyAtOneThird = new AutoResetEvent(false),
//                            SecBufNotifyAtTwoThirds = new AutoResetEvent(false);
//                        bool SecBufInitialLoad = true;

//                        MemoryStream PcmStream = new MemoryStream();
//                        int PcmStreamNextConsumPcmPosition = 0;

//                        WaitHandle[] SecBufWaitHandles = {SecBufNotifyAtBegin, 
//                                                            SecBufNotifyAtOneThird, 
//                                                            SecBufNotifyAtTwoThirds};

//                        Debug.WriteLine("OggPlayer.cs: playing back ogg file.");

//                        // decode the ogg file into its PCM data 
//                        for (FirstTime = true; ; )
//                        {
//                            if (stopNow) // client has decided to stop playback! 
//                            {
//                                SecBuf.Stop();
//                                Debug.WriteLine("OggPlayer.cs: vorbis decoder playback interrupted.");
//                                break;
//                            }

//                            // get the next chunk of PCM data, pin these so GC can't relocate them 
//                            fixed (byte* buf = &PcmBuffer[0])
//                            {
//                                fixed (int* HoleCount = &EventArgs.ErrorHoleCount)
//                                {
//                                    fixed (int* BadLinkCount = &EventArgs.ErrorBadLinkCount)
//                                    {
//                                        // NOTE: the sample size of the returned PCM data -- either 8-bit  
//                                        //		 or 16-bit samples -- is set by BitsPerSample 
//                                        PcmBytes = NativeMethods.ogg_decode_at_most_one_vorbis_packet(
//                                            vf, buf, PcmBuffer.Length,
//                                            BitsPerSample,
//                                            &ChannelsCount, &SamplingRate,
//                                            HoleCount, BadLinkCount);
//                                    }
//                                }
//                            }

//                            // set AtEOF 
//                            if (PcmBytes == 0)
//                                AtEOF = true;
//                            else
//                                AtEOF = false;

//                            if (FirstTime && AtEOF)
//                            {
//                                EventArgs.ReasonForFailure =
//                                    "The Ogg Vorbis file '" + FileName + "' has no usable data.";
//                                oplay.PlayOggFileResult(this, EventArgs);
//                                return;
//                            }

//                            // we must be aware that multiple bitstream sections do not  
//                            // necessarily use the same number of channels or sampling rate							 
//                            if (!FirstTime &&
//                                (ChannelsCount != PreviousChannelCount
//                                || SamplingRate != PreviousSamplingCount))
//                                FormatChanged = true;
//                            else
//                            {
//                                FormatChanged = false;
//                            }

//                            // compute format items 
//                            if (FirstTime || FormatChanged)
//                            {
//                                BlockAlign = ChannelsCount * (BitsPerSample / 8);
//                                AverageBytesPerSecond = SamplingRate * BlockAlign;
//                            }

//                            // use the PCM data 
//                            if (FirstTime)
//                            {
//                                int HoldThisManySamples =
//                                    (int)(SamplingRate * SecBufHoldThisManySeconds);

//                                // set the format 
//                                MyWaveFormat.AverageBytesPerSecond = AverageBytesPerSecond;
//                                MyWaveFormat.BitsPerSample = (short)BitsPerSample;
//                                MyWaveFormat.BlockAlign = (short)BlockAlign;
//                                MyWaveFormat.Channels = (short)ChannelsCount;
//                                MyWaveFormat.SamplesPerSecond = SamplingRate;
//                                MyWaveFormat.FormatTag = WaveFormatTag.Pcm;

//                                // set BufferDescription 
//                                MyDescription = new BufferDescription();

//                                MyDescription.Format = MyWaveFormat;
//                                MyDescription.BufferBytes =
//                                    SecBufByteSize = HoldThisManySamples * BlockAlign;
//                                MyDescription.CanGetCurrentPosition = true;
//                                MyDescription.ControlPositionNotify = true;

//                                // create the buffer 
//                                SecBuf = new SecondaryBuffer(MyDescription, DirectSoundDevice);

//                                // set 3 notification points, at 0, 1/3, and 2/3 SecBuf size 
//                                MyNotify = new Notify(SecBuf);

//                                BufferPositionNotify[] MyBufferPositions = new BufferPositionNotify[3];

//                                MyBufferPositions[0].Offset = 0;
//                                MyBufferPositions[0].EventNotifyHandle = SecBufNotifyAtBegin.Handle;
//                                MyBufferPositions[1].Offset = (HoldThisManySamples / 3) * BlockAlign;
//                                MyBufferPositions[1].EventNotifyHandle = SecBufNotifyAtOneThird.Handle;
//                                MyBufferPositions[2].Offset = ((HoldThisManySamples * 2) / 3) * BlockAlign;
//                                MyBufferPositions[2].EventNotifyHandle = SecBufNotifyAtTwoThirds.Handle;

//                                MyNotify.SetNotificationPositions(MyBufferPositions);

//                                // prepare for next iteration 
//                                FirstTime = false;
//                                PreviousChannelCount = ChannelsCount;
//                                PreviousSamplingCount = SamplingRate;
//                            }
//                            else if (FormatChanged)
//                            {
//                                SecBuf.Stop();

//                                EventArgs.ReasonForFailure =
//                                    "The Ogg Vorbis file '" + FileName + "' has a format change (DirectSound can't handle this).";
//                                oplay.PlayOggFileResult(this, EventArgs);
//                                return;
//                            }
//                            else if (AtEOF)
//                            {
//                                Debug.WriteLine("OggPlayer.cs: vorbis decoder playback at end of file.");

//                                Debug.Assert(SecBufPlayPositionWhenNextWritePositionSet >= 0
//                                    && SecBufPlayPositionWhenNextWritePositionSet < SecBufByteSize
//                                    && SecBufNextWritePosition >= 0
//                                    && SecBufNextWritePosition < SecBufByteSize);

//                                // start playback if there wasn't enough PCM data to fill SecBuf the first time 
//                                if (SecBufInitialLoad)
//                                {
//                                    Debug.Assert(SecBufPlayPositionWhenNextWritePositionSet == 0
//                                        && SecBufNextWritePosition > 0);

//                                    // NOTE: Play() does the playing in its own thread 
//                                    SecBuf.Play(0, BufferPlayFlags.Looping);
//                                    Debug.WriteLine("OggPlayer.cs: vorbis decoder starting playback.");
//                                }

//                                // poll for end of current playback 
//                                int LoopbackCount = 0,
//                                    PlayPosition,
//                                    PreviousPlayPosition = SecBufPlayPositionWhenNextWritePositionSet;

//                                for (; ; PreviousPlayPosition = PlayPosition)
//                                {
//                                    Thread.Sleep(10);  // 10 milliseconds is an arbitrary but good choice 

//                                    PlayPosition = SecBuf.PlayPosition;

//                                    if (PlayPosition < PreviousPlayPosition)
//                                        ++LoopbackCount;

//                                    if (SecBufPlayPositionWhenNextWritePositionSet <= SecBufNextWritePosition)
//                                    {
//                                        if (PlayPosition >= SecBufNextWritePosition || LoopbackCount > 0)
//                                            break;
//                                    }
//                                    else
//                                    {
//                                        if ((PlayPosition < SecBufPlayPositionWhenNextWritePositionSet
//                                            && PlayPosition >= SecBufNextWritePosition) || LoopbackCount > 1)
//                                            break;
//                                    }
//                                }

//                                // done playing 
//                                Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder finished playback.");
//                                SecBuf.Stop();
//                                break;
//                            }

//                            // copy the new PCM data into PCM memory stream 
//                            PcmStream.SetLength(0);
//                            PcmStream.Write(PcmBuffer, 0, PcmBytes);
//                            PcmStream.Position = 0;
//                            PcmStreamNextConsumPcmPosition = 0;

//                            Debug.Assert(PcmStream.Length == PcmBytes);

//                            // initial load of secondary buffer 
//                            if (SecBufInitialLoad)
//                            {
//                                int WriteCount = (int)Math.Min(
//                                    PcmStream.Length,
//                                    SecBufByteSize - SecBufNextWritePosition);

//                                Debug.Assert(WriteCount >= 0);

//                                if (WriteCount > 0)
//                                {
//                                    Debug.Assert(PcmStream.Position == 0);

//                                    SecBuf.Write(
//                                        SecBufNextWritePosition,
//                                        PcmStream,
//                                        WriteCount,
//                                        LockFlag.None);

//                                    SecBufNextWritePosition += WriteCount;
//                                    PcmStreamNextConsumPcmPosition += WriteCount;
//                                }

//                                if (SecBufByteSize == SecBufNextWritePosition)
//                                {
//                                    // done filling the buffer 
//                                    SecBufInitialLoad = false;
//                                    SecBufNextWritePosition = 0;

//                                    // so start the playback 
//                                    // NOTE: Play() does the playing in its own thread 
//                                    SecBuf.Play(0, BufferPlayFlags.Looping);
//                                    Thread.Sleep(0);  //yield rest of timeslice so playback can start right away 
//                                }
//                                else
//                                {
//                                    continue;  // get more PCM data 
//                                }
//                            }

//                            // exhaust the current PCM data, writing the data into SecBuf 
//                            for (; PcmStreamNextConsumPcmPosition < PcmStream.Length; )
//                            {
//                                int WriteCount,
//                                    PlayPosition = SecBuf.PlayPosition,
//                                    WritePosition = SecBuf.WritePosition;

//                                // set WriteCount  
//                                WriteCount = 0;

//                                if (SecBufNextWritePosition < PlayPosition
//                                    && (WritePosition >= PlayPosition || WritePosition < SecBufNextWritePosition))
//                                    WriteCount = PlayPosition - SecBufNextWritePosition;
//                                else if (SecBufNextWritePosition > WritePosition
//                                    && WritePosition >= PlayPosition)
//                                    WriteCount = (SecBufByteSize - SecBufNextWritePosition) + PlayPosition;

//                                Debug.Assert(WriteCount >= 0 && WriteCount <= SecBufByteSize);

//                                // write 
//                                if (WriteCount > 0)
//                                {
//                                    WriteCount = (int)Math.Min(
//                                        WriteCount,
//                                        PcmStream.Length - PcmStreamNextConsumPcmPosition);

//                                    PcmStream.Position = PcmStreamNextConsumPcmPosition;
//                                    SecBuf.Write(
//                                        SecBufNextWritePosition,
//                                        PcmStream,
//                                        WriteCount,
//                                        LockFlag.None);

//                                    SecBufNextWritePosition =
//                                        (SecBufNextWritePosition + WriteCount) % SecBufByteSize;
//                                    SecBufPlayPositionWhenNextWritePositionSet = PlayPosition;
//                                    PcmStreamNextConsumPcmPosition += WriteCount;
//                                }
//                                else
//                                {
//                                    WaitHandle.WaitAny(SecBufWaitHandles);
//                                }
//                            }
//                        }

//                        // finito 
//                        EventArgs.Success = true;
//                        oplay.PlayOggFileResult(this, EventArgs);
//                    }
//                    catch (System.Exception ex)
//                    {
//                        EventArgs.ReasonForFailure = ex.Message;
//                        oplay.PlayOggFileResult(this, EventArgs);
//                        return;
//                    }
//                    finally
//                    {
//                        // cleanup vorbis decoder 
//                        int ReturnValue = NativeMethods.final_ogg_cleanup(vf);
//                        Debug.Assert(ReturnValue == 0);

//                        // cleanup DirectSound stuff 
//                        if (SecBuf != null)
//                        {
//                            SecBuf.Dispose();
//                            SecBuf = null;
//                        }

//                        if (MyDescription != null)
//                        {
//                            MyDescription.Dispose();
//                            MyDescription = null;
//                        }

//                        if (MyNotify != null)
//                        {
//                            MyNotify.Dispose();
//                            MyNotify = null;
//                        }

//                        Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder objects cleaned up.");
//                    }
//                }  // end unsafe   
//            }
//        }
//    }
//}
