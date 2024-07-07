// based on Cookbook formulae for audio EQ biquad filter coefficients
// http://www.musicdsp.org/files/Audio-EQ-Cookbook.txt
// by Robert Bristow-Johnson  <rbj@audioimagination.com>

//    alpha = sin(w0)/(2*Q)                                       (case: Q)
//          = sin(w0)*sinh( ln(2)/2 * BW * w0/sin(w0) )           (case: BW)
//          = sin(w0)/2 * sqrt( (A + 1/A)*(1/S - 1) + 2 )         (case: S)
// Q: (the EE kind of definition, except for peakingEQ in which A*Q is
// the classic EE Q.  That adjustment in definition was made so that
// a boost of N dB followed by a cut of N dB for identical Q and
// f0/Fs results in a precisely flat unity gain filter or "wire".)
//
// BW: the bandwidth in octaves (between -3 dB frequencies for BPF
// and notch or between midpoint (dBgain/2) gain frequencies for
// peaking EQ)
//
// S: a "shelf slope" parameter (for shelving EQ only).  When S = 1,
// the shelf slope is as steep as it can be and remain monotonically
// increasing or decreasing gain with frequency.  The shelf slope, in
// dB/octave, remains proportional to S for all other values for a
// fixed f0/Fs and dBgain.

using System;

namespace NAudio.Core.Dsp;

/// <summary>
/// BiQuad filter
/// </summary>
public class BiQuadFilter
{
    // coefficients
    private double a0;
    private double a1;
    private double a2;
    private double a3;
    private double a4;

    // state
    private float x1;
    private float x2;
    private float y1;
    private float y2;

    /// <summary>
    /// Passes a single sample through the filter
    /// </summary>
    /// <param name="inSample">Input sample</param>
    /// <returns>Output sample</returns>
    public float Transform( float inSample )
    {
        // compute result
        var result = (a0 * inSample) + (a1 * x1) + (a2 * x2) - (a3 * y1) - (a4 * y2);

        // shift x1 to x2, sample to x1 
        x2 = x1;
        x1 = inSample;

        // shift y1 to y2, result to y1 
        y2 = y1;
        y1 = (float)result;

        return y1;
    }

    private void SetCoefficients( double aa0, double aa1, double aa2, double b0, double b1, double b2 )
    {
        // precompute the coefficients
        a0 = b0 / aa0;
        a1 = b1 / aa0;
        a2 = b2 / aa0;
        a3 = aa1 / aa0;
        a4 = aa2 / aa0;
    }

    /// <summary>
    /// Set this up as a low pass filter
    /// </summary>
    /// <param name="sampleRate">Sample Rate</param>
    /// <param name="cutoffFrequency">Cut-off Frequency</param>
    /// <param name="q">Bandwidth</param>
    public void SetLowPassFilter( float sampleRate, float cutoffFrequency, float q )
    {
        // H(s) = 1 / (s^2 + s/Q + 1)
        var w0 = 2 * Math.PI * cutoffFrequency / sampleRate;
        var cosw0 = Math.Cos(w0);
        var alpha = Math.Sin(w0) / (2 * q);

        var b0 = (1 - cosw0) / 2;
        var b1 = 1 - cosw0;
        var b2 = (1 - cosw0) / 2;
        var aa0 = 1 + alpha;
        var aa1 = -2 * cosw0;
        var aa2 = 1 - alpha;
        SetCoefficients( aa0, aa1, aa2, b0, b1, b2 );
    }

    /// <summary>
    /// Set this up as a peaking EQ
    /// </summary>
    /// <param name="sampleRate">Sample Rate</param>
    /// <param name="centreFrequency">Centre Frequency</param>
    /// <param name="q">Bandwidth (Q)</param>
    /// <param name="dbGain">Gain in decibels</param>
    public void SetPeakingEq( float sampleRate, float centreFrequency, float q, float dbGain )
    {
        // H(s) = (s^2 + s*(A/Q) + 1) / (s^2 + s/(A*Q) + 1)
        var w0 = 2 * Math.PI * centreFrequency / sampleRate;
        var cosw0 = Math.Cos(w0);
        var sinw0 = Math.Sin(w0);
        var alpha = sinw0 / (2 * q);
        var a = Math.Pow(10, dbGain / 40);     // TODO: should we square root this value?

        var b0 = 1 + (alpha * a);
        var b1 = -2 * cosw0;
        var b2 = 1 - (alpha * a);
        var aa0 = 1 + (alpha / a);
        var aa1 = -2 * cosw0;
        var aa2 = 1 - (alpha / a);
        SetCoefficients( aa0, aa1, aa2, b0, b1, b2 );
    }

    /// <summary>
    /// Create a Peaking EQ
    /// </summary>
    public static BiQuadFilter PeakingEQ( float sampleRate, float centreFrequency, float q, float dbGain )
    {
        var filter = new BiQuadFilter();
        filter.SetPeakingEq( sampleRate, centreFrequency, q, dbGain );
        return filter;
    }

    private BiQuadFilter()
    {
        // zero initial samples
        x1 = x2 = 0;
        y1 = y2 = 0;
    }
}
