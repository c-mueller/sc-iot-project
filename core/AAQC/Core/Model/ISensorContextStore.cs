using Model.Model;

namespace Core.Model
{
    public interface ISensorContextStore
    {
        void StorePddlObjectState(PddlObjectState objectState);
        PddlObjectState GetLastPddlObjectState();
    }
}