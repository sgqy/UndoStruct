using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UndoStruct
{
    class UndoPacker<Type>
    {
        Type[] _chain = null; // 环形列表

        int _volume = 0;
        int idx(int Index) // 求解实际下标
        {
            return Index % _volume;
        }

        public UndoPacker(int Count)
        {
            _idx = _volume = Count; // _volume 初始化后不会使实际下标成为 -1
            _chain = new Type[Count];
        }

        int _redo_count = 0;
        int _undo_count = 0;
        int _idx = 0; // 虚拟下表
        bool _undoing = false;

        public void push(Type item)
        {
            //MessageBox.Show(idx(_idx++).ToString());
            if (_undoing) // 多退少补: 指针向前移, undo_count 同进退
            {
                ++_idx;
                if (_undo_count < _volume - 1) ++_undo_count;
            }

            _chain[idx(_idx++)] = item;
            if (_undo_count < _volume - 1 && !_undoing) _undo_count++;

            _redo_count = 0;
            _undoing = false;
        }

        public Type undo()
        {
            _redo_count++;
            _undo_count--;

            if (!_undoing) // 多退少补: 指针向后移, undo_count 同进退
                _idx -= 2;
            else
                _idx--;

            _undoing = true;
            return _chain[idx(_idx)];
        }

        public Type redo()
        {
            _redo_count--;
            if (_undo_count < _volume - 1) _undo_count++;
            return _chain[idx(++_idx)];
        }

        public Type[] content()
        {
            return _chain;
        }

        public static int UndoAvaliable = 1;
        public static int RedoAvaliable = 2;
        public int state()
        {
            int ret = 0;

            if (_redo_count > 0) ret |= RedoAvaliable;
            if (_undo_count > 0) ret |= UndoAvaliable;
            if (_undo_count == _idx || _undo_count == _volume - 1) ret &= ~RedoAvaliable;
            //ret &= ~UndoAvaliable;

            return ret;
        }
    }
}
