﻿using System;

namespace NAudio.Core.Dsp;

// C# ADSR based on work by Nigel Redmon, EarLevel Engineering: earlevel.com
// http://www.earlevel.com/main/2013/06/03/envelope-generators-adsr-code/
/// <summary>
/// Envelope generator (ADSR)
/// </summary>
public class EnvelopeGenerator
{
    private float output;
    private float attackRate;
    private float decayRate;
    private float releaseRate;
    private float attackCoef;
    private float decayCoef;
    private float releaseCoef;
    private float sustainLevel;
    private float targetRatioAttack;
    private float targetRatioDecayRelease;
    private float attackBase;
    private float decayBase;
    private float releaseBase;

    /// <summary>
    /// Envelope State
    /// </summary>
    public enum EnvelopeState
    {
        /// <summary>
        /// Idle
        /// </summary>
        Idle = 0,
        /// <summary>
        /// Attack
        /// </summary>
        Attack,
        /// <summary>
        /// Decay
        /// </summary>
        Decay,
        /// <summary>
        /// Sustain
        /// </summary>
        Sustain,
        /// <summary>
        /// Release
        /// </summary>
        Release
    };

    /// <summary>
    /// Creates and Initializes an Envelope Generator
    /// </summary>
    public EnvelopeGenerator()
    {
        Reset();
        AttackRate = 0;
        DecayRate = 0;
        ReleaseRate = 0;
        SustainLevel = 1.0f;
        SetTargetRatioAttack( 0.3f );
        SetTargetRatioDecayRelease( 0.0001f );
    }

    /// <summary>
    /// Attack Rate (seconds * SamplesPerSecond)
    /// </summary>
    public float AttackRate
    {
        get => attackRate;
        set
        {
            attackRate = value;
            attackCoef = CalcCoef( value, targetRatioAttack );
            attackBase = ( 1.0f + targetRatioAttack ) * ( 1.0f - attackCoef );
        }
    }

    /// <summary>
    /// Decay Rate (seconds * SamplesPerSecond)
    /// </summary>
    public float DecayRate
    {
        get => decayRate;
        set
        {
            decayRate = value;
            decayCoef = CalcCoef( value, targetRatioDecayRelease );
            decayBase = ( sustainLevel - targetRatioDecayRelease ) * ( 1.0f - decayCoef );
        }
    }

    /// <summary>
    /// Release Rate (seconds * SamplesPerSecond)
    /// </summary>
    public float ReleaseRate
    {
        get => releaseRate;
        set
        {
            releaseRate = value;
            releaseCoef = CalcCoef( value, targetRatioDecayRelease );
            releaseBase = -targetRatioDecayRelease * ( 1.0f - releaseCoef );
        }
    }

    private static float CalcCoef( float rate, float targetRatio ) => (float)Math.Exp( -Math.Log( ( 1.0f + targetRatio ) / targetRatio ) / rate );

    /// <summary>
    /// Sustain Level (1 = 100%)
    /// </summary>
    public float SustainLevel
    {
        get => sustainLevel;
        set
        {
            sustainLevel = value;
            decayBase = ( sustainLevel - targetRatioDecayRelease ) * ( 1.0f - decayCoef );
        }
    }

    /// <summary>
    /// Sets the attack curve
    /// </summary>
    private void SetTargetRatioAttack( float targetRatio )
    {
        if ( targetRatio < 0.000000001f )
        {
            targetRatio = 0.000000001f;  // -180 dB
        }

        targetRatioAttack = targetRatio;
        attackBase = ( 1.0f + targetRatioAttack ) * ( 1.0f - attackCoef );
    }

    /// <summary>
    /// Sets the decay release curve
    /// </summary>
    private void SetTargetRatioDecayRelease( float targetRatio )
    {
        if ( targetRatio < 0.000000001f )
        {
            targetRatio = 0.000000001f;  // -180 dB
        }

        targetRatioDecayRelease = targetRatio;
        decayBase = ( sustainLevel - targetRatioDecayRelease ) * ( 1.0f - decayCoef );
        releaseBase = -targetRatioDecayRelease * ( 1.0f - releaseCoef );
    }

    /// <summary>
    /// Read the next volume multiplier from the envelope generator
    /// </summary>
    /// <returns>A volume multiplier</returns>
    public float Process()
    {
        switch ( State )
        {
            case EnvelopeState.Idle:
                break;
            case EnvelopeState.Attack:
                output = attackBase + ( output * attackCoef );
                if ( output >= 1.0f )
                {
                    output = 1.0f;
                    State = EnvelopeState.Decay;
                }
                break;
            case EnvelopeState.Decay:
                output = decayBase + ( output * decayCoef );
                if ( output <= sustainLevel )
                {
                    output = sustainLevel;
                    State = EnvelopeState.Sustain;
                }
                break;
            case EnvelopeState.Sustain:
                break;
            case EnvelopeState.Release:
                output = releaseBase + ( output * releaseCoef );
                if ( output <= 0.0 )
                {
                    output = 0.0f;
                    State = EnvelopeState.Idle;
                }
                break;
        }
        return output;
    }

    /// <summary>
    /// Trigger the gate
    /// </summary>
    /// <param name="gate">If true, enter attack phase, if false enter release phase (unless already idle)</param>
    public void Gate( bool gate )
    {
        if ( gate )
        {
            State = EnvelopeState.Attack;
        }
        else if ( State != EnvelopeState.Idle )
        {
            State = EnvelopeState.Release;
        }
    }

    /// <summary>
    /// Current envelope state
    /// </summary>
    public EnvelopeState State
    {
        get;
        private set;
    }

    /// <summary>
    /// Reset to idle state
    /// </summary>
    public void Reset()
    {
        State = EnvelopeState.Idle;
        output = 0.0f;
    }

    /// <summary>
    /// Get the current output level
    /// </summary>
    public float GetOutput() => output;
}
