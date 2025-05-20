using Newtonsoft.Json;

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
    public int CurrentNote = 0;
    public List<Note> Notes = new();

    public class Note
    {
        public float Time;
        public NoteKind Kind;
        public float Hold;

        public Note(float time, NoteKind kind, float hold)
        {
            this.Time = time;
            this.Kind = kind;
            this.Hold = hold;
        }

        public Note((float, int, float) t)
        {
            this.Time = t.Item1;
            this.Kind = KindFromInt(t.Item2);
            this.Hold = t.Item3;
        }

        private static NoteKind KindFromInt(int i)
        {
            switch (i) {
            case 0:
                return NoteKind.Left;
            case 1:
                return NoteKind.Down;
            case 2:
                return NoteKind.Up;
            case 3:
                return NoteKind.Right;
            default:
                throw new Exception("");
            }
        }
    }
}

class ChartsLoader
{
    public static ChartBoard LoadFromJSON(string path)
    {
        ChartBoard result = new();
        ChartsJson j;
        using (StreamReader reader = new(path))
        {
            var jsonContent = reader.ReadToEnd();
            var jj = JsonConvert.DeserializeObject<ChartsJson>(jsonContent);
            if (jj == null) throw new Exception("");
            j = jj;
        }

        result.BPM = j.song.bpm;
        result.Speed = j.song.speed;
        foreach (var note in j.song.notes)
        {
            foreach (var n in note.sectionNotes)
            {
                result.Notes.Add(new(n));
            }
        }
        return result;
    }

}

public class ChartsJson
{
    public SongJson song = new();

    public class SongJson
    {
        public float speed;
        public int bpm;
        public List<NoteJson> notes = new();
    }

    public class NoteJson
    {
        public List<(float, int, float)> sectionNotes = new();
    }
}
