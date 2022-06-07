using UnityEngine;
interface Tool{
    void ToolSelected(Tooltype type);

    void ToolActivated(bool isRightController);
    void ToolDeselected();
}