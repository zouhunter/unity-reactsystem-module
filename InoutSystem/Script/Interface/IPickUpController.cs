using UnityEngine.Events;
namespace FlowSystem
{
    public interface IPickUpController
    {
        bool PickUped { get; }
        bool TryPickUpObject(out PickUpAble pickedUpObj);
        bool TryStayPickUpedObject();
        bool PickDownPickedUpObject();
        void UpdatePickUpdObject();
    }
}