using System;

[Serializable]
public class MicrophoneData : IEquatable<MicrophoneData>
{
    const int ScaleFactor = 4;

    //public ushort[] Samples;
    public float[] Samples;

    public MicrophoneData()
    {

    }

    public MicrophoneData(float[] rawSamples, int channelCount)
    {
        Samples = rawSamples;
        /*Samples = new float[rawSamples.Length / channelCount / ScaleFactor];

        int index = 0;

        for (int i = 0; i < rawSamples.Length && index < Samples.Length; i += channelCount * ScaleFactor)
        {
            float sum = 0;
            for (int j = 0; j < ScaleFactor; j++)
            {
                //sum += ScaleFloatToShort(rawSamples[i + j * channelCount], -1, 1, ushort.MinValue, ushort.MaxValue);
                sum += rawSamples[i + j * channelCount];
            }
            Samples[index++] = sum / ScaleFactor;
        }*/
    }

    public float[] GetFloats()
    {
      /*  var result = new float[Samples.Length];

        for (int i = 0; i < Samples.Length; i++)
        {
            result[i] = ScaleShortToFloat(Samples[i], ushort.MinValue, ushort.MaxValue, -1, 1);
        }*/

        return Samples;
    }

    public bool Equals(MicrophoneData other)
    {
        if (other.Samples == Samples)
            return true;

        if (other.Samples.Length != Samples.Length)
            return false;

        for (int i = 0; i < other.Samples.Length; i++)
        {
            if (other.Samples[i] != Samples[i])
                return false;
        }

        return true;
    }

    public static ushort ScaleFloatToShort(float value, float minValue, float maxValue, ushort minTarget, ushort maxTarget)
    {
        int targetRange = maxTarget - minTarget; 
        float valueRange = maxValue - minValue;
        float valueRelative = value - minValue;
        return (ushort)(minTarget + (ushort)(valueRelative / valueRange * targetRange));
    }

    public static float ScaleShortToFloat(ushort value, ushort minValue, ushort maxValue, float minTarget, float maxTarget)
    {
        float targetRange = maxTarget - minTarget;
        ushort valueRange = (ushort)(maxValue - minValue);
        ushort valueRelative = (ushort)(value - minValue);
        return minTarget + (valueRelative / valueRange * targetRange);
    }
}
