using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using FFMpegCore.Extend;
using System.Collections.Concurrent;
using Bitmap = System.Drawing.Bitmap;
using Rectangle = System.Drawing.Rectangle;
using BitmapData = System.Drawing.Imaging.BitmapData;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using ImageLockMode = System.Drawing.Imaging.ImageLockMode;

namespace aoc2022 {
    class FFWriter {
        BlockingCollection<IVideoFrame> _frames = new BlockingCollection<IVideoFrame>(30);

        int width, height;
        double framerate;
        string filename;

        public FFWriter(int w, int h, int fps, string title) {
            width = w;
            height = h;
            framerate = (double)fps;
            filename = title + ".mp4";
        }

        public unsafe void addRawImage(void* data) {        
            // Unfortunately, there seems to be no way to 'claim' the pointer, so we copy the data. again.
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Buffer.MemoryCopy(data, bmpData.Scan0.ToPointer(), width * height * 4, width * height * 4);
            bmp.UnlockBits(bmpData);
            _frames.Add(new BitmapVideoFrameWrapper(bmp));
        }

        public bool run() {
            var videoFramesSource = new RawVideoPipeSource(_frames.GetConsumingEnumerable()) {
                FrameRate = framerate
            };
            return FFMpegArguments
                .FromPipeInput(videoFramesSource)
                .OutputToFile(filename, true, options =>
                    options.WithVideoCodec(VideoCodec.LibX264)
                           .WithConstantRateFactor(18)
                           .WithSpeedPreset(Speed.Slow))
                .ProcessSynchronously();
        }

        public void finish() {
            _frames.CompleteAdding();
        }
    }
}
