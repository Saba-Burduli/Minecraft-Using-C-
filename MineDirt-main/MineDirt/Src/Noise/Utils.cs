namespace MineDirt.Src.Noise;
public static class Utils
{
    public static float ScaleNoise(float value, float min, float max) => 
        min + (value + 1) / 2 * (max - min);
}
