﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.ViewModels.MainWindow._3DGraphics
{
    public class Viewport3DViewModel : ViewModelBase
    {
        #region Field

        

        #endregion

        #region PrivateProperty

        

        #endregion

        #region ReactiveCommand

        

        #endregion

        #region ReactiveProperty



        #endregion

        #region Request



        #endregion

        #region Events



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void CloseWindow()
        {
            FinishInteraction?.Invoke();
        }

        #endregion
    }
}
