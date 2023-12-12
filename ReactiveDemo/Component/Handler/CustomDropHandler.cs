using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReactiveDemo.Component.Handler
{
    public class CustomDropHandler : DefaultDropHandler
    {
        public override void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
        }

        public override void Drop(IDropInfo dropInfo)
        {
            
        }
    }
}
