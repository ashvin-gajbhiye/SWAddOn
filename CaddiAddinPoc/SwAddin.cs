using SldWorks;
using SwConst;
using SWPublished;
using System;
using System.Runtime.InteropServices;

namespace CaddiAddinPoc
{
    [Guid("B7C1F7F2-9B2F-4C55-9B9E-123456789ABC")]
    [ComVisible(true)]
    public class SwAddin : ISwAddin
    {
        private SldWorks.SldWorks swApp;
        private ICommandManager cmdMgr;
        private int addinID;

        private TaskpaneView taskPaneView;
        private IssuePanel panelUI;

        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            swApp = (SldWorks.SldWorks)ThisSW;
            addinID = Cookie;

            swApp.SetAddinCallbackInfo2(0, this, addinID);
            cmdMgr = swApp.GetCommandManager(addinID);

            CreateCommandManager();

            return true;
        }

        public bool DisconnectFromSW()
        {
            if (cmdMgr != null)
                cmdMgr.RemoveCommandGroup(1);

            return true;
        }

        private void CreateCommandManager()
        {
            int errors = 0;

            ICommandGroup cmdGroup =
                cmdMgr.CreateCommandGroup2(
                    1,
                    "Caddi Tools",
                    "Caddi Design Review Add-in",
                    "",
                    -1,
                    false,
                    ref errors);

            cmdGroup.AddCommandItem2(
                "Open Review Panel",
                -1,
                "Open Caddi Issue Panel",
                "Caddi Panel",
                0,
                "OnOpenPanel",
                "",
                0,
                (int)(swCommandItemType_e.swToolbarItem |
                      swCommandItemType_e.swMenuItem));

            cmdGroup.HasToolbar = true;
            cmdGroup.HasMenu = true;
            cmdGroup.Activate();
        }

        public void OnOpenPanel()
        {
            ShowTaskPane();
        }

        private void ShowTaskPane()
        {
            if (taskPaneView == null)
            {
                taskPaneView =
                    swApp.CreateTaskpaneView2("", "Caddi Design Review");

                panelUI = new IssuePanel();

                taskPaneView.DisplayWindowFromHandle(panelUI.Handle.ToInt32());
            }
            else
            {
                taskPaneView.ShowView();
            }
        }
    }
}