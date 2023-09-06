using UnityEngine.UIElements;

public class ValidityIndicator : Image
{
    bool _valid = true;
    public bool Valid
    {
        get => _valid;
        set
        {
            _valid = value;
            if (_valid)
            {
                AddToClassList("success-image");
                RemoveFromClassList("failure-image");
            }
            else
            {
                RemoveFromClassList("success-image");
                AddToClassList("failure-image");
            }
        }
    }
}
