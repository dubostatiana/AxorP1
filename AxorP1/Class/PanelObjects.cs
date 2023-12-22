namespace AxorP1.Class
{
    public class PanelObject
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public int SizeX { get; set; } = 1;
        public int SizeY { get; set; } = 1;
        public int MinSizeX { get; set; } = 0;
        public int MinSizeY { get; set; } = 0;

        public System.Type ComponentType { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }
}
