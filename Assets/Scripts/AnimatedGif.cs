using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

public class AnimatedGif
{
    readonly PropertyItem mTimes;

    public List<AnimatedGifFrame> Images { get; } = new List<AnimatedGifFrame>();

    public AnimatedGif(string path)
    {
        Image img = Image.FromFile(path);
        int frames = img.GetFrameCount(FrameDimension.Time);

        if (frames <= 1) return;

        byte[] times = img.GetPropertyItem(0x5100).Value;
        int frame = 0;
        for (; ; )
        {
            int dur = BitConverter.ToInt32(times, 4 * frame);
            Images.Add(new AnimatedGifFrame(new Bitmap(img), dur));
            if (++frame >= frames) break;
            img.SelectActiveFrame(FrameDimension.Time, frame);
        }
        img.Dispose();
    }

    public AnimatedGif(Image image)
    {
        int frames = image.GetFrameCount(FrameDimension.Time);

        if (frames <= 1) return;

        byte[] times = image.GetPropertyItem(0x5100).Value;
        int frame = 0;
        for (; ; )
        {
            int dur = BitConverter.ToInt32(times, 4 * frame);
            Images.Add(new AnimatedGifFrame(new Bitmap(image), dur));
            if (++frame >= frames) break;
            image.SelectActiveFrame(FrameDimension.Time, frame);
        }
    }
}

public class AnimatedGifFrame
{
    internal AnimatedGifFrame(Image img, int duration)
    {
        Image = img; Duration = duration;
    }
    public Image Image { get; }
    public int Duration { get; }
}