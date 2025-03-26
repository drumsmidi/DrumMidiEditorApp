using LiteDB;

namespace DrumMidiLibrary.pIO.pDatabase.pRecord;

internal record struct RecordDmsFile
{
    /// <summary>
    /// 譜面ファイルパス
    /// </summary>
    [BsonId]
    public string FilePath { get; set; }

    public string BaseFolderPath { get; set; }

}
