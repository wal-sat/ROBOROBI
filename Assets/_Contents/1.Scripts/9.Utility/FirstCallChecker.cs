public class FirstCallChecker
{
    private bool _isFirstCall = true;

    public bool Check()
    {
        if (_isFirstCall)
        {
            _isFirstCall = false;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _isFirstCall = true;
    }
}
