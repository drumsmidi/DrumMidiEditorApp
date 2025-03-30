using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pIO.pScore.pDms;

/// <summary>
/// Score入出力
/// </summary>
internal abstract class ScoreStreamBase : IScoreReader, IScoreWriter
{
    #region Reader

    public bool Validation( GeneralPath aGeneralPath )
    {
        var errorFlag = false;

        try
        {
            var xsdMarkup = GetValidation();

            using var xsd_reader = new XmlTextReader( new StringReader( xsdMarkup ) );

            var setting = new XmlReaderSettings();
            _ = setting.Schemas.Add( string.Empty, xsd_reader );

            setting.ValidationType = ValidationType.Schema;
            setting.ValidationEventHandler += new ValidationEventHandler
                (
                    ( sender, e ) =>
                    {
                        errorFlag = true;
                        Log.Warning( $"Validation Error:{e.Message}" );
                    }
                );

            using var xml_reader = XmlReader.Create( aGeneralPath.AbsoulteFilePath, setting );

            while ( xml_reader.Read() )
            {
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            errorFlag = true;
        }

        return !errorFlag;
    }

    /// <summary>
    /// XML妥当性チェック用の文字列取得
    /// </summary>
    /// <returns></returns>
    protected virtual string GetValidation() => string.Empty;

    public void Read( GeneralPath aGeneralPath, out Score aScore )
    {
        using var reader = XmlReader.Create( aGeneralPath.AbsoulteFilePath );

        OnReadScore( reader, out aScore );
    }

    public void Read( GeneralPath aGeneralPath, out MidiMapSet aMidiMapSet )
    {
        using var reader = XmlReader.Create( aGeneralPath.AbsoulteFilePath );

        OnReadScore( reader, out var score );

        aMidiMapSet = score.GetFirstMidiMapSet()?.Clone() ?? throw new Exception( $"Can't find MidiMapSet" );
    }

    /// <summary>
    /// スコア読取
    /// </summary>
    /// <param name="aReader"></param>
    /// <param name="aScore"></param>
    protected virtual void OnReadScore( XmlReader aReader, out Score aScore )
    {
        aScore = new();
    }

    #endregion

    #region Writer

    public void Write( GeneralPath aGeneralPath, Score aScore )
    {
        using var writer = new XmlTextWriter( aGeneralPath.AbsoulteFilePath, Encoding.UTF8 );

        writer.Formatting = Formatting.Indented;

        OnWriteScore( writer, aScore );
    }

    /// <summary>
    /// スコア出力
    /// </summary>
    /// <param name="aWriter"></param>
    /// <param name="aScore"></param>
    protected virtual void OnWriteScore( XmlTextWriter aWriter, Score aScore )
    {
    }

    public void Write( GeneralPath aGeneralPath, MidiMapSet aMidiMapSet )
    {
        using var writer = new XmlTextWriter( aGeneralPath.AbsoulteFilePath, Encoding.UTF8 );

        writer.Formatting = Formatting.Indented;

        OnWriteMidiMapSet( writer, aMidiMapSet );
    }

    /// <summary>
    /// MidiMapセット出力
    /// </summary>
    /// <param name="aWriter">XMLライター</param>
    /// <param name="aMidiMapSet">保存MidiMapSet</param>
    protected virtual void OnWriteMidiMapSet( XmlTextWriter aWriter, MidiMapSet aMidiMapSet )
    {
    }

    #endregion
}
