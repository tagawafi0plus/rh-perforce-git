using System.Collections.Generic;
using UnityEngine;

namespace Live.Scripts.Action
{
    public enum UserActionState
    {
        TapStart,
        TapEnd,
        None
    }

    public class UserAction
    {
        private List<bool> flagItems;
        private List<bool> inputItems;

        public UserAction()
        {
            flagItems = new List<bool>
            {
                false,
                false,
                false,
                false,
                false,
            };

            inputItems = new List<bool>(flagItems);
        }

        public int GetCount()
        {
            return flagItems.Count;
        }

        public UserActionState GetInputState(int index)
        {
            var flag = flagItems[index];
            var input = inputItems[index];

            if (!flag && input)
            {
                flagItems[index] = true;
                return UserActionState.TapStart;
            }
            else if (flag && !input)
            {
                flagItems[index] = false;
                return UserActionState.TapEnd;
            }

            return UserActionState.None;
        }

        public void checkInput()
        {
            for (var i = 0; i < flagItems.Count; i++)
            {
                inputItems[i] = false;
            }

            if (!Input.anyKey)
            {
                return;
            }

            if (Input.GetKey("a"))
            {
                inputItems[0] = true;
            }

            if (Input.GetKey("s"))
            {
                inputItems[1] = true;
            }

            if (Input.GetKey("d"))
            {
                inputItems[2] = true;
            }

            if (Input.GetKey("f"))
            {
                inputItems[3] = true;
            }

            if (Input.GetKey("g"))
            {
                inputItems[4] = true;
            }
        }
    }
}