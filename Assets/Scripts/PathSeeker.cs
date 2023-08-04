public class PathSeeker : PathFinding.PathSeeker
{
    void Awake()
    {
        PathRequestManager = GetComponentInParent<WorldInternalIO>().PathRequestManager;
    }
}
