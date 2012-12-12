using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGAI.State
{
    public class Cell<C>
    {
        private int x;
        private int y;
        private C content;

        public Cell(int x, int y, C content)
        {
            this.x = x;
            this.y = y;
            this.content = content;
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }

        public C getContent()
        {
            return content;
        }

        public void setContent(C content)
        {
            this.content = content;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Cell<C> c = obj as Cell<C>;
            if ((System.Object)c == null)
            {
                return false;
            }
            return x == c.x && y == c.y;
        }
        public override int GetHashCode()
        {
            return x + 23 + y + 31;
        }
    }
}
