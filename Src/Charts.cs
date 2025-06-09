using Newtonsoft.Json;
using Raylib_cs;

public enum NoteKind
{
    Left  = 0,
    Down  = 1,
    Up    = 2,
    Right = 3,
}

public class ChartBoard
{
    public float Speed;
    public int BPM;
    public float CurrentY = 0;
    public List<(Note, Note)> Notes = new();
    public int BPS
    {
        get => this.BPM * 60;
    }

    public class Note
    {
        public float Left;
        public float Down;
        public float Up;
        public float Right;

        public Note(float left = 0, float down = 0, float up = 0, float right = 0)
        {
            this.Left = left;
            this.Down = down;
            this.Up = up;
            this.Right = right;
        }
    }

    public static ChartBoard LoadFromJSON(string path)
    {
        ChartBoard result = new();
        ChartsJson chartsJson;
        using (StreamReader reader = new(path)) {
            var string_result = reader.ReadToEnd();
            var jsonResult = JsonConvert.DeserializeObject<ChartsJson>(string_result);
            if (jsonResult == null) throw new Exception("");
            chartsJson = jsonResult;
        }
        result.BPM = chartsJson.bpm;
        result.Speed = chartsJson.speed;
        result.Notes = (
            from note in chartsJson.notes
            select (
                new Note(note.opponent[0], note.opponent[1], note.opponent[2], note.opponent[3]),
                new Note(note.player[0], note.player[1], note.player[2], note.player[3])
            )
        ).ToList();
        return result;
    }

    public List<(Note, Note)> GetIntersects(float noteHeight, float top, float down)
    {
        List<(Note, Note)> result = new();

        for (int i = 0; i < Notes.Count(); i++)
        {
            float noteY = i * noteHeight;
            if (noteY <= top + CurrentY && noteY <= down + CurrentY)
            {
                result.Add(Notes[i]);
            }
            else if (noteY >= down + CurrentY)
            {
                break;
            }
        }

        return result;
    }

    public (int, int) GetCurrentRange(float noteHeight, float min, float max)
    {
        if (Notes.Count() == 0)
            throw new Exception("Not enough notes.");

        int start = -1;
        int end = -1;

        for (int i = 0; i < Notes.Count(); i++)
        {
            float noteY = i * noteHeight;
            if (noteY >= min + CurrentY && noteY <= max + CurrentY)
            {
                if (start == -1)
                    start = i;
                end = i;
            }
            else if (noteY >= max + CurrentY)
            {
                break;
            }
        }

        return (start, end);
    }
}

public class ChartsJson {
    public float speed;
    public int bpm;
    public List<NoteLineJson> notes = new();

    public class NoteLineJson {
        public List<float> opponent = new();
        public List<float> player = new();
    }
}
