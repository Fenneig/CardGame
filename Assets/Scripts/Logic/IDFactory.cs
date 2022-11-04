namespace CardGame.Logic
{
    public static class IDFactory
    {
        private static int _count = 0;

        public static int GetUniqueID()
        {
            return _count++;
        }

        public static void ResetID()
        {
            _count = 0;
        }
    }
}