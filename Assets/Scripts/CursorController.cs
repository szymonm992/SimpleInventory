using UnityEngine;

namespace SimpleInventory
{
    public class CursorController 
    {
        public void SetCursorLocked(CursorLockMode cursorLockMode)
        {
            Cursor.lockState = cursorLockMode;
        }

        public void SetCursorVisible(bool isVisible)
        {
            Cursor.visible = isVisible;
        }
    }
}
