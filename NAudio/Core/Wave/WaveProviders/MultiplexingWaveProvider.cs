﻿using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Core.Utils;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveOutputs;

// ReSharper disable once CheckNamespace
namespace NAudio.Core.Wave.WaveProviders;

/// <summary>
/// Allows any number of inputs to be patched to outputs
/// Uses could include swapping left and right channels, turning mono into stereo,
/// feeding different input sources to different soundcard outputs etc
/// </summary>
public class MultiplexingWaveProvider : IWaveProvider
{
    private readonly IList<IWaveProvider> inputs;
    private readonly List<int> mappings;
    private readonly int bytesPerSample;

    /// <summary>
    /// Creates a multiplexing wave provider, allowing re-patching of input channels to different
    /// output channels. Number of outputs is equal to total number of channels in inputs
    /// </summary>
    /// <param name="inputs">Input wave providers. Must all be of the same format, but can have any number of channels</param>
    public MultiplexingWaveProvider( IEnumerable<IWaveProvider> inputs ) : this( inputs, -1 )
    {

    }

    /// <summary>
    /// Creates a multiplexing wave provider, allowing re-patching of input channels to different
    /// output channels
    /// </summary>
    /// <param name="inputs">Input wave providers. Must all be of the same format, but can have any number of channels</param>
    /// <param name="numberOfOutputChannels">Desired number of output channels. (-1 means use total number of input channels)</param>
    public MultiplexingWaveProvider( IEnumerable<IWaveProvider> inputs, int numberOfOutputChannels )
    {
        this.inputs = new List<IWaveProvider>( inputs );

        OutputChannelCount = numberOfOutputChannels == -1 ? this.inputs.Sum( i => i.WaveFormat.Channels ) : numberOfOutputChannels;

        if ( this.inputs.Count == 0 )
        {
            throw new ArgumentException( "You must provide at least one input" );
        }
        if ( OutputChannelCount < 1 )
        {
            throw new ArgumentException( "You must provide at least one output" );
        }
        foreach ( var input in this.inputs )
        {
            if ( WaveFormat == null )
            {
                WaveFormat = input.WaveFormat.Encoding == WaveFormatEncoding.Pcm
                    ? new WaveFormat( input.WaveFormat.SampleRate, input.WaveFormat.BitsPerSample, OutputChannelCount )
                    : input.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat
                        ? WaveFormat.CreateIeeeFloatWaveFormat( input.WaveFormat.SampleRate, OutputChannelCount )
                        : throw new ArgumentException( "Only PCM and 32 bit float are supported" );
            }
            else
            {
                if ( input.WaveFormat.BitsPerSample != WaveFormat.BitsPerSample )
                {
                    throw new ArgumentException( "All inputs must have the same bit depth" );
                }
                if ( input.WaveFormat.SampleRate != WaveFormat.SampleRate )
                {
                    throw new ArgumentException( "All inputs must have the same sample rate" );
                }
            }
            InputChannelCount += input.WaveFormat.Channels;
        }
        bytesPerSample = WaveFormat.BitsPerSample / 8;

        mappings = [];
        for ( var n = 0; n < OutputChannelCount; n++ )
        {
            mappings.Add( n % InputChannelCount );
        }
    }

    /// <summary>
    /// persistent temporary buffer to prevent creating work for garbage collector
    /// </summary>
    private byte[] inputBuffer;

    /// <summary>
    /// Reads data from this WaveProvider
    /// </summary>
    /// <param name="buffer">Buffer to be filled with sample data</param>
    /// <param name="offset">Offset to write to within buffer, usually 0</param>
    /// <param name="count">Number of bytes required</param>
    /// <returns>Number of bytes read</returns>
    public int Read( byte [] buffer, int offset, int count )
    {
        var outputBytesPerFrame = bytesPerSample * OutputChannelCount;
        var sampleFramesRequested = count / outputBytesPerFrame;
        var inputOffset = 0;
        var sampleFramesRead = 0;
        // now we must read from all inputs, even if we don't need their data, so they stay in sync
        foreach ( var input in inputs )
        {
            var inputBytesPerFrame = bytesPerSample * input.WaveFormat.Channels;
            var bytesRequired = sampleFramesRequested * inputBytesPerFrame;
            inputBuffer = BufferHelpers.Ensure( inputBuffer, bytesRequired );
            var bytesRead = input.Read(inputBuffer, 0, bytesRequired);
            sampleFramesRead = Math.Max( sampleFramesRead, bytesRead / inputBytesPerFrame );

            for ( var n = 0; n < input.WaveFormat.Channels; n++ )
            {
                var inputIndex = inputOffset + n;
                for ( var outputIndex = 0; outputIndex < OutputChannelCount; outputIndex++ )
                {
                    if ( mappings [ outputIndex ] == inputIndex )
                    {
                        var inputBufferOffset = n * bytesPerSample;
                        var outputBufferOffset = offset + (outputIndex * bytesPerSample);
                        var sample = 0;
                        while ( sample < sampleFramesRequested && inputBufferOffset < bytesRead )
                        {
                            Array.Copy( inputBuffer, inputBufferOffset, buffer, outputBufferOffset, bytesPerSample );
                            outputBufferOffset += outputBytesPerFrame;
                            inputBufferOffset += inputBytesPerFrame;
                            sample++;
                        }
                        // clear the end
                        while ( sample < sampleFramesRequested )
                        {
                            Array.Clear( buffer, outputBufferOffset, bytesPerSample );
                            outputBufferOffset += outputBytesPerFrame;
                            sample++;
                        }
                    }
                }
            }
            inputOffset += input.WaveFormat.Channels;
        }

        return sampleFramesRead * outputBytesPerFrame;
    }

    /// <summary>
    /// The WaveFormat of this WaveProvider
    /// </summary>
    public WaveFormat WaveFormat
    {
        get;
    }

    /// <summary>
    /// Connects a specified input channel to an output channel
    /// </summary>
    /// <param name="inputChannel">Input Channel index (zero based). Must be less than InputChannelCount</param>
    /// <param name="outputChannel">Output Channel index (zero based). Must be less than OutputChannelCount</param>
    public void ConnectInputToOutput( int inputChannel, int outputChannel )
    {
        if ( inputChannel < 0 || inputChannel >= InputChannelCount )
        {
            throw new ArgumentException( "Invalid input channel" );
        }
        if ( outputChannel < 0 || outputChannel >= OutputChannelCount )
        {
            throw new ArgumentException( "Invalid output channel" );
        }
        mappings [ outputChannel ] = inputChannel;
    }

    /// <summary>
    /// The number of input channels. Note that this is not the same as the number of input wave providers. If you pass in
    /// one stereo and one mono input provider, the number of input channels is three.
    /// </summary>
    public int InputChannelCount
    {
        get;
    }

    /// <summary>
    /// The number of output channels, as specified in the constructor.
    /// </summary>
    public int OutputChannelCount
    {
        get;
    }
}
