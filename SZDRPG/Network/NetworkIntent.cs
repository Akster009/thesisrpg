using System.Text;
using SFML.System;

namespace SZDRPG.Network
{
    public class NetworkIntent
    {
        public int IntentNum = -1;
        public Vector2f IntentPosition = new Vector2f();
        public int Length;

        public byte[] GetMessage()
        {
            byte[] ret = Encoding.ASCII.GetBytes(IntentNum + "|" + (int)IntentPosition.X  + "|" + (int)IntentPosition.Y);
            Length = ret.Length;
            return ret;
        }
    }
}