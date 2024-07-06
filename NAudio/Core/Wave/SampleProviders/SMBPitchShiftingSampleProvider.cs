using System;
using NAudio.Core.Dsp;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave.SampleProviders;

/// <summary>
/// Author: Freefall
/// Date: 05.08.16
/// Based on: the port of Stephan M. Bernsee´s pitch shifting class
/// Port site: https://sites.google.com/site/mikescoderama/pitch-shifting
/// Test application and github site: https://github.com/Freefall63/NAudio-Pitchshifter
/// 
/// NOTE: I strongly advice to add a Limiter for post-processing.
/// For my needs the FastAttackCompressor1175 provides acceptable results:
/// https://github.com/Jiyuu/SkypeFX/blob/master/JSNet/FastAttackCompressor1175.cs
///
/// UPDATE: Added a simple Limiter based on the pydirac implementation.
/// https://github.com/echonest/remix/blob/master/external/pydirac225/source/Dirac_LE.cpp
/// 
///</summary>
public class SmbPitchShiftingSampleProvider : ISampleProvider
{
    //Shifter objects
    private readonly ISampleProvider sourceStream;
    private readonly int fftSize;
    private readonly long osamp;
    private readonly SmbPitchShifter shifterLeft = new();
    private readonly SmbPitchShifter shifterRight = new();

    //Limiter constants
    private const float LIM_THRESH = 0.95f;
    private const float LIM_RANGE = 1f - LIM_THRESH;
    private const float M_PI_2 = (float) (Math.PI/2);

    /// <summary>
    /// Creates a new SMB Pitch Shifting Sample Provider with default settings
    /// </summary>
    /// <param name="sourceProvider">Source provider</param>
    public SmbPitchShiftingSampleProvider( ISampleProvider sourceProvider )
        : this( sourceProvider, 4096, 4L, 1f )
    {
    }

    /// <summary>
    /// Creates a new SMB Pitch Shifting Sample Provider with custom settings
    /// </summary>
    /// <param name="sourceProvider">Source provider</param>
    /// <param name="fftSize">FFT Size (any power of two &lt;= 4096: 4096, 2048, 1024, 512, ...)</param>
    /// <param name="osamp">Oversampling (number of overlapping windows)</param>
    /// <param name="initialPitch">Initial pitch (0.5f = octave down, 1.0f = normal, 2.0f = octave up)</param>
    public SmbPitchShiftingSampleProvider( ISampleProvider sourceProvider, int fftSize, long osamp,
        float initialPitch )
    {
        sourceStream = sourceProvider;
        WaveFormat = sourceProvider.WaveFormat;
        this.fftSize = fftSize;
        this.osamp = osamp;
        PitchFactor = initialPitch;
    }

    /// <summary>
    /// Read from this sample provider
    /// </summary>
    public int Read( float [] buffer, int offset, int count )
    {
        var sampRead = sourceStream.Read(buffer, offset, count);
        if ( PitchFactor == 1f )
        {
            //Nothing to do.
            return sampRead;
        }
        if ( WaveFormat.Channels == 1 )
        {
            var mono = new float[sampRead];
            var index = 0;
            for ( var sample = offset; sample <= sampRead + offset - 1; sample++ )
            {
                mono [ index ] = buffer [ sample ];
                index += 1;
            }
            shifterLeft.PitchShift( PitchFactor, sampRead, fftSize, osamp, WaveFormat.SampleRate, mono );
            index = 0;
            for ( var sample = offset; sample <= sampRead + offset - 1; sample++ )
            {
                buffer [ sample ] = Limiter( mono [ index ] );
                index += 1;
            }
            return sampRead;
        }
        if ( WaveFormat.Channels == 2 )
        {
            var left = new float[(sampRead >> 1)];
            var right = new float[(sampRead >> 1)];
            var index = 0;
            for ( var sample = offset; sample <= sampRead + offset - 1; sample += 2 )
            {
                left [ index ] = buffer [ sample ];
                right [ index ] = buffer [ sample + 1 ];
                index += 1;
            }
            shifterLeft.PitchShift( PitchFactor, sampRead >> 1, fftSize, osamp, WaveFormat.SampleRate, left );
            shifterRight.PitchShift( PitchFactor, sampRead >> 1, fftSize, osamp, WaveFormat.SampleRate, right );
            index = 0;
            for ( var sample = offset; sample <= sampRead + offset - 1; sample += 2 )
            {
                buffer [ sample ] = Limiter( left [ index ] );
                buffer [ sample + 1 ] = Limiter( right [ index ] );
                index += 1;
            }
            return sampRead;
        }
        throw new Exception( "Shifting of more than 2 channels is currently not supported." );
    }

    /// <summary>
    /// WaveFormat
    /// </summary>
    public WaveFormat WaveFormat
    {
        get;
    }

    /// <summary>
    /// Pitch Factor (0.5f = octave down, 1.0f = normal, 2.0f = octave up)
    /// </summary>
    public float PitchFactor { get; set; } = 1f;

    private float Limiter( float sample )
    {
        float res;
        if ( LIM_THRESH < sample )
        {
            res = ( sample - LIM_THRESH ) / LIM_RANGE;
            res = (float)( ( Math.Atan( res ) / M_PI_2 * LIM_RANGE ) + LIM_THRESH );
        }
        else if ( sample < -LIM_THRESH )
        {
            res = -( sample + LIM_THRESH ) / LIM_RANGE;
            res = -(float)( ( Math.Atan( res ) / M_PI_2 * LIM_RANGE ) + LIM_THRESH );
        }
        else
        {
            res = sample;
        }
        return res;
    }
}
