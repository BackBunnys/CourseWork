namespace CourseWork.Common
{
    public static class Statistics
    {
        public static uint DrawCalls { get; set; }
        public static uint Vertices { get; set; }
        public static uint Indices { get; set; }

        public static void Clear()
        {
            DrawCalls = 0u;
            Vertices = 0u;
            Indices = 0u;
        }
    }
}