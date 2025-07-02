public static class CommandIdGenerator
{
    private static int _currentId = 0;

    public static int  GenerateId()
    {
        if (_currentId < 100000) return _currentId++;
        return -1;
    }
}