using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit.DebugView
{
    public class TextQueue
    {
        private int maxCount = 30;
        private Text textElement;

        private Queue<string> stringQue;

        // Start is called before the first frame update
        public TextQueue(Text textElement)
        {
            stringQue = new Queue<string>();
            this.textElement = textElement;
        }

        public void AddLine(string msg)
        {
            if (textElement != null)
            {
                if (stringQue.Count < maxCount)
                {
                    stringQue.Enqueue(msg);
                }
                else
                {
                    stringQue.Dequeue();
                    stringQue.Enqueue(msg);
                }
                UpdateText();
            }
        }
        public void UpdateText()
        {
            string content = "";

            foreach (var str in stringQue)
            {
                content = str + "\n" + content;
            }

            if (this.textElement.IsActive())
            {
                this.textElement.text = content;
            }
        }
    }
}
