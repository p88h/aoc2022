using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using System.Collections.Concurrent;
using FFMpegCore.Extensions.SkiaSharp;
using Bitmap = SkiaSharp.SKBitmap;

namespace aoc2022 {
    class FFWriter {
        BlockingCollection<IVideoFrame> _frames = new BlockingCollection<IVideoFrame>(300);

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
            Bitmap bmp = new Bitmap(width, height, SkiaSharp.SKColorType.Rgb888x, SkiaSharp.SKAlphaType.Unknown);
            void* ptr = (void*)bmp.GetPixels();
            Buffer.MemoryCopy(data, ptr, width * height * 4, width * height * 4);
            bmp.NotifyPixelsChanged();
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
