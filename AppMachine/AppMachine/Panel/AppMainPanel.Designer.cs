namespace AppMachine.Panel
{
    partial class AppMainPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.kNavigatorMain = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            this.kProductionPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kGroupOperation = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.kAllSetupPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kNavigatorMainSetup = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            this.kRecipeManagerPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kGeneralSetupPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kSpecPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kMachineSetupPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kCommonParamPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kUsersManagerPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kComponentPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.componentBrowser = new MCore.Controls.ComponentBrowser();
            this.kStateMachinePage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kNavigatorStateMachine = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            this.KSemiStateMachinePage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kNavigatorSemiStateMachine = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            this.kUtilityPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kGroupUtility = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.kHitoryPage = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kGroupHistory = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.kRichTbHistory = new ComponentFactory.Krypton.Toolkit.KryptonRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.kNavigatorMain)).BeginInit();
            this.kNavigatorMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kProductionPage)).BeginInit();
            this.kProductionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kGroupOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroupOperation.Panel)).BeginInit();
            this.kGroupOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kAllSetupPage)).BeginInit();
            this.kAllSetupPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kNavigatorMainSetup)).BeginInit();
            this.kNavigatorMainSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kRecipeManagerPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGeneralSetupPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kSpecPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kMachineSetupPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kCommonParamPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kUsersManagerPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kComponentPage)).BeginInit();
            this.kComponentPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kStateMachinePage)).BeginInit();
            this.kStateMachinePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kNavigatorStateMachine)).BeginInit();
            this.kNavigatorStateMachine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KSemiStateMachinePage)).BeginInit();
            this.KSemiStateMachinePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kNavigatorSemiStateMachine)).BeginInit();
            this.kNavigatorSemiStateMachine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kUtilityPage)).BeginInit();
            this.kUtilityPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kGroupUtility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroupUtility.Panel)).BeginInit();
            this.kGroupUtility.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kHitoryPage)).BeginInit();
            this.kHitoryPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kGroupHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroupHistory.Panel)).BeginInit();
            this.kGroupHistory.Panel.SuspendLayout();
            this.kGroupHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // kNavigatorMain
            // 
            this.kNavigatorMain.AllowPageReorder = false;
            this.kNavigatorMain.Bar.TabBorderStyle = ComponentFactory.Krypton.Toolkit.TabBorderStyle.OneNote;
            this.kNavigatorMain.Bar.TabStyle = ComponentFactory.Krypton.Toolkit.TabStyle.OneNote;
            this.kNavigatorMain.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kNavigatorMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kNavigatorMain.Group.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.FormMain;
            this.kNavigatorMain.Location = new System.Drawing.Point(0, 0);
            this.kNavigatorMain.Name = "kNavigatorMain";
            this.kNavigatorMain.Pages.AddRange(new ComponentFactory.Krypton.Navigator.KryptonPage[] {
            this.kProductionPage,
            this.kAllSetupPage,
            this.kComponentPage,
            this.kStateMachinePage,
            this.KSemiStateMachinePage,
            this.kUtilityPage,
            this.kHitoryPage});
            this.kNavigatorMain.SelectedIndex = 5;
            this.kNavigatorMain.Size = new System.Drawing.Size(1264, 932);
            this.kNavigatorMain.StateCommon.Panel.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            this.kNavigatorMain.TabIndex = 2;
            this.kNavigatorMain.Text = "kNavigatorMain";
            // 
            // kProductionPage
            // 
            this.kProductionPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kProductionPage.Controls.Add(this.kGroupOperation);
            this.kProductionPage.Flags = 65534;
            this.kProductionPage.LastVisibleSet = true;
            this.kProductionPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kProductionPage.Name = "kProductionPage";
            this.kProductionPage.Padding = new System.Windows.Forms.Padding(3);
            this.kProductionPage.Size = new System.Drawing.Size(1258, 894);
            this.kProductionPage.StateCommon.Page.Color1 = System.Drawing.Color.Green;
            this.kProductionPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.Green;
            this.kProductionPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.Green;
            this.kProductionPage.StateCommon.Tab.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kProductionPage.StateCommon.Tab.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kProductionPage.Text = "Operation";
            this.kProductionPage.ToolTipTitle = "Page ToolTip";
            this.kProductionPage.UniqueName = "7DC3C6C63634424980ABA033FF98CCA4";
            // 
            // kGroupOperation
            // 
            this.kGroupOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kGroupOperation.Location = new System.Drawing.Point(3, 3);
            this.kGroupOperation.Name = "kGroupOperation";
            this.kGroupOperation.Size = new System.Drawing.Size(1252, 888);
            this.kGroupOperation.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kGroupOperation.StateCommon.Border.Rounding = 7;
            this.kGroupOperation.TabIndex = 1;
            // 
            // kAllSetupPage
            // 
            this.kAllSetupPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kAllSetupPage.Controls.Add(this.kNavigatorMainSetup);
            this.kAllSetupPage.Flags = 65534;
            this.kAllSetupPage.LastVisibleSet = true;
            this.kAllSetupPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kAllSetupPage.Name = "kAllSetupPage";
            this.kAllSetupPage.Padding = new System.Windows.Forms.Padding(3);
            this.kAllSetupPage.Size = new System.Drawing.Size(1258, 894);
            this.kAllSetupPage.StateCommon.Page.Color1 = System.Drawing.Color.Maroon;
            this.kAllSetupPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.Maroon;
            this.kAllSetupPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.Maroon;
            this.kAllSetupPage.StateCommon.Tab.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kAllSetupPage.StateCommon.Tab.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kAllSetupPage.Text = "All Setup";
            this.kAllSetupPage.ToolTipTitle = "Page ToolTip";
            this.kAllSetupPage.UniqueName = "75E5B7E97E3D4201038861916CC4C39A";
            // 
            // kNavigatorMainSetup
            // 
            this.kNavigatorMainSetup.AllowPageReorder = false;
            this.kNavigatorMainSetup.Bar.BarOrientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Left;
            this.kNavigatorMainSetup.Bar.ItemOrientation = ComponentFactory.Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kNavigatorMainSetup.Bar.TabBorderStyle = ComponentFactory.Krypton.Toolkit.TabBorderStyle.RoundedEqualSmall;
            this.kNavigatorMainSetup.Bar.TabStyle = ComponentFactory.Krypton.Toolkit.TabStyle.OneNote;
            this.kNavigatorMainSetup.Button.ButtonDisplayLogic = ComponentFactory.Krypton.Navigator.ButtonDisplayLogic.None;
            this.kNavigatorMainSetup.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kNavigatorMainSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kNavigatorMainSetup.Group.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.FormMain;
            this.kNavigatorMainSetup.Location = new System.Drawing.Point(3, 3);
            this.kNavigatorMainSetup.Name = "kNavigatorMainSetup";
            this.kNavigatorMainSetup.Pages.AddRange(new ComponentFactory.Krypton.Navigator.KryptonPage[] {
            this.kRecipeManagerPage,
            this.kGeneralSetupPage,
            this.kSpecPage,
            this.kMachineSetupPage,
            this.kCommonParamPage,
            this.kUsersManagerPage});
            this.kNavigatorMainSetup.Panel.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kNavigatorMainSetup.SelectedIndex = 5;
            this.kNavigatorMainSetup.Size = new System.Drawing.Size(1252, 888);
            this.kNavigatorMainSetup.StateCommon.Panel.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            this.kNavigatorMainSetup.TabIndex = 2;
            this.kNavigatorMainSetup.Text = "kryptonNavigator2";
            this.kNavigatorMainSetup.SelectedPageChanged += new System.EventHandler(this.kNavigatorMainSetup_SelectedPageChanged);
            // 
            // kRecipeManagerPage
            // 
            this.kRecipeManagerPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kRecipeManagerPage.Flags = 65534;
            this.kRecipeManagerPage.LastVisibleSet = true;
            this.kRecipeManagerPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kRecipeManagerPage.Name = "kRecipeManagerPage";
            this.kRecipeManagerPage.Size = new System.Drawing.Size(1135, 882);
            this.kRecipeManagerPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.White;
            this.kRecipeManagerPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kRecipeManagerPage.Text = "Recipes Manager";
            this.kRecipeManagerPage.ToolTipTitle = "Page ToolTip";
            this.kRecipeManagerPage.UniqueName = "CCF5D54C0D834BC9A097618B62D7413A";
            // 
            // kGeneralSetupPage
            // 
            this.kGeneralSetupPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kGeneralSetupPage.Flags = 65534;
            this.kGeneralSetupPage.LastVisibleSet = true;
            this.kGeneralSetupPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kGeneralSetupPage.Name = "kGeneralSetupPage";
            this.kGeneralSetupPage.Size = new System.Drawing.Size(1141, 882);
            this.kGeneralSetupPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.White;
            this.kGeneralSetupPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kGeneralSetupPage.Text = "General";
            this.kGeneralSetupPage.ToolTipTitle = "Page ToolTip";
            this.kGeneralSetupPage.UniqueName = "8B0882EB5ADE46DDD9A38B83F3B086A9";
            // 
            // kSpecPage
            // 
            this.kSpecPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kSpecPage.Flags = 65534;
            this.kSpecPage.LastVisibleSet = true;
            this.kSpecPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kSpecPage.Name = "kSpecPage";
            this.kSpecPage.Size = new System.Drawing.Size(1141, 882);
            this.kSpecPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.White;
            this.kSpecPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kSpecPage.Text = "Specification";
            this.kSpecPage.ToolTipTitle = "Page ToolTip";
            this.kSpecPage.UniqueName = "6F4926C4F7C9453FA5BDE37C8097EDC3";
            // 
            // kMachineSetupPage
            // 
            this.kMachineSetupPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kMachineSetupPage.Flags = 65534;
            this.kMachineSetupPage.LastVisibleSet = true;
            this.kMachineSetupPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kMachineSetupPage.Name = "kMachineSetupPage";
            this.kMachineSetupPage.Size = new System.Drawing.Size(1141, 882);
            this.kMachineSetupPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.White;
            this.kMachineSetupPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kMachineSetupPage.Text = "Machine Setup";
            this.kMachineSetupPage.ToolTipTitle = "Page ToolTip";
            this.kMachineSetupPage.UniqueName = "5B4E58DAC25747996D92DE9CF5FC8693";
            // 
            // kCommonParamPage
            // 
            this.kCommonParamPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kCommonParamPage.Flags = 65534;
            this.kCommonParamPage.LastVisibleSet = true;
            this.kCommonParamPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kCommonParamPage.Name = "kCommonParamPage";
            this.kCommonParamPage.Size = new System.Drawing.Size(1141, 882);
            this.kCommonParamPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.White;
            this.kCommonParamPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kCommonParamPage.Text = "CommonParam";
            this.kCommonParamPage.ToolTipTitle = "Page ToolTip";
            this.kCommonParamPage.UniqueName = "7037C41C18BF44E1BB9B8F1228F8D104";
            // 
            // kUsersManagerPage
            // 
            this.kUsersManagerPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kUsersManagerPage.Flags = 65534;
            this.kUsersManagerPage.LastVisibleSet = true;
            this.kUsersManagerPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kUsersManagerPage.Name = "kUsersManagerPage";
            this.kUsersManagerPage.Size = new System.Drawing.Size(1141, 882);
            this.kUsersManagerPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.White;
            this.kUsersManagerPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kUsersManagerPage.Text = "Users Manager";
            this.kUsersManagerPage.ToolTipTitle = "Page ToolTip";
            this.kUsersManagerPage.UniqueName = "B693C829F370443780B31E9AD14ED981";
            // 
            // kComponentPage
            // 
            this.kComponentPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kComponentPage.Controls.Add(this.componentBrowser);
            this.kComponentPage.Flags = 65534;
            this.kComponentPage.LastVisibleSet = true;
            this.kComponentPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kComponentPage.Name = "kComponentPage";
            this.kComponentPage.Padding = new System.Windows.Forms.Padding(3);
            this.kComponentPage.Size = new System.Drawing.Size(1258, 894);
            this.kComponentPage.StateCommon.Page.Color1 = System.Drawing.Color.Olive;
            this.kComponentPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.Olive;
            this.kComponentPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.Olive;
            this.kComponentPage.StateCommon.Tab.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kComponentPage.StateCommon.Tab.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kComponentPage.Text = "Component";
            this.kComponentPage.ToolTipTitle = "Page ToolTip";
            this.kComponentPage.UniqueName = "DADAFFDF684D48A820A5F2F763FF8594";
            // 
            // componentBrowser
            // 
            this.componentBrowser.BackColor = System.Drawing.SystemColors.Control;
            this.componentBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.componentBrowser.Location = new System.Drawing.Point(3, 3);
            this.componentBrowser.Name = "componentBrowser";
            this.componentBrowser.Size = new System.Drawing.Size(1252, 888);
            this.componentBrowser.TabIndex = 2;
            // 
            // kStateMachinePage
            // 
            this.kStateMachinePage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kStateMachinePage.Controls.Add(this.kNavigatorStateMachine);
            this.kStateMachinePage.Flags = 65534;
            this.kStateMachinePage.LastVisibleSet = true;
            this.kStateMachinePage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kStateMachinePage.Name = "kStateMachinePage";
            this.kStateMachinePage.Padding = new System.Windows.Forms.Padding(3);
            this.kStateMachinePage.Size = new System.Drawing.Size(1258, 894);
            this.kStateMachinePage.StateCommon.Page.Color1 = System.Drawing.Color.Navy;
            this.kStateMachinePage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.Navy;
            this.kStateMachinePage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.Navy;
            this.kStateMachinePage.StateCommon.Tab.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kStateMachinePage.StateCommon.Tab.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kStateMachinePage.Text = "State Machine";
            this.kStateMachinePage.ToolTipTitle = "Page ToolTip";
            this.kStateMachinePage.UniqueName = "3A6DEC46C4984BAEFD879A222EBC769C";
            // 
            // kNavigatorStateMachine
            // 
            this.kNavigatorStateMachine.AllowPageReorder = false;
            this.kNavigatorStateMachine.Bar.BarOrientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Left;
            this.kNavigatorStateMachine.Bar.ItemOrientation = ComponentFactory.Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kNavigatorStateMachine.Bar.TabBorderStyle = ComponentFactory.Krypton.Toolkit.TabBorderStyle.RoundedEqualSmall;
            this.kNavigatorStateMachine.Bar.TabStyle = ComponentFactory.Krypton.Toolkit.TabStyle.OneNote;
            this.kNavigatorStateMachine.Button.ButtonDisplayLogic = ComponentFactory.Krypton.Navigator.ButtonDisplayLogic.None;
            this.kNavigatorStateMachine.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kNavigatorStateMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kNavigatorStateMachine.Group.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.FormMain;
            this.kNavigatorStateMachine.Location = new System.Drawing.Point(3, 3);
            this.kNavigatorStateMachine.Name = "kNavigatorStateMachine";
            this.kNavigatorStateMachine.Panel.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kNavigatorStateMachine.Size = new System.Drawing.Size(1252, 888);
            this.kNavigatorStateMachine.StateCommon.Panel.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            this.kNavigatorStateMachine.TabIndex = 3;
            this.kNavigatorStateMachine.Text = "kryptonNavigator2";
            // 
            // KSemiStateMachinePage
            // 
            this.KSemiStateMachinePage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.KSemiStateMachinePage.Controls.Add(this.kNavigatorSemiStateMachine);
            this.KSemiStateMachinePage.Flags = 65534;
            this.KSemiStateMachinePage.LastVisibleSet = true;
            this.KSemiStateMachinePage.MinimumSize = new System.Drawing.Size(50, 50);
            this.KSemiStateMachinePage.Name = "KSemiStateMachinePage";
            this.KSemiStateMachinePage.Padding = new System.Windows.Forms.Padding(3);
            this.KSemiStateMachinePage.Size = new System.Drawing.Size(1258, 894);
            this.KSemiStateMachinePage.StateCommon.Page.Color1 = System.Drawing.Color.Teal;
            this.KSemiStateMachinePage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.Teal;
            this.KSemiStateMachinePage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.Teal;
            this.KSemiStateMachinePage.StateCommon.Tab.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.KSemiStateMachinePage.StateCommon.Tab.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.KSemiStateMachinePage.Text = "Semi State Machne";
            this.KSemiStateMachinePage.ToolTipTitle = "Page ToolTip";
            this.KSemiStateMachinePage.UniqueName = "C0272E5C60774223EA85152FF19853DE";
            // 
            // kNavigatorSemiStateMachine
            // 
            this.kNavigatorSemiStateMachine.AllowPageReorder = false;
            this.kNavigatorSemiStateMachine.Bar.BarOrientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Left;
            this.kNavigatorSemiStateMachine.Bar.ItemOrientation = ComponentFactory.Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kNavigatorSemiStateMachine.Bar.TabBorderStyle = ComponentFactory.Krypton.Toolkit.TabBorderStyle.RoundedEqualSmall;
            this.kNavigatorSemiStateMachine.Bar.TabStyle = ComponentFactory.Krypton.Toolkit.TabStyle.OneNote;
            this.kNavigatorSemiStateMachine.Button.ButtonDisplayLogic = ComponentFactory.Krypton.Navigator.ButtonDisplayLogic.None;
            this.kNavigatorSemiStateMachine.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kNavigatorSemiStateMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kNavigatorSemiStateMachine.Group.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.FormMain;
            this.kNavigatorSemiStateMachine.Location = new System.Drawing.Point(3, 3);
            this.kNavigatorSemiStateMachine.Name = "kNavigatorSemiStateMachine";
            this.kNavigatorSemiStateMachine.Panel.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kNavigatorSemiStateMachine.Size = new System.Drawing.Size(1252, 888);
            this.kNavigatorSemiStateMachine.StateCommon.Panel.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            this.kNavigatorSemiStateMachine.TabIndex = 4;
            this.kNavigatorSemiStateMachine.Text = "kryptonNavigator2";
            // 
            // kUtilityPage
            // 
            this.kUtilityPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kUtilityPage.Controls.Add(this.kGroupUtility);
            this.kUtilityPage.Flags = 65534;
            this.kUtilityPage.LastVisibleSet = true;
            this.kUtilityPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kUtilityPage.Name = "kUtilityPage";
            this.kUtilityPage.Padding = new System.Windows.Forms.Padding(3);
            this.kUtilityPage.Size = new System.Drawing.Size(1258, 894);
            this.kUtilityPage.StateCommon.Page.Color1 = System.Drawing.Color.Purple;
            this.kUtilityPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.Purple;
            this.kUtilityPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.Purple;
            this.kUtilityPage.StateCommon.Tab.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kUtilityPage.StateCommon.Tab.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kUtilityPage.Text = "Utility";
            this.kUtilityPage.ToolTipTitle = "Page ToolTip";
            this.kUtilityPage.UniqueName = "AA471BAD9FFA4E368D8B77A6F57973E6";
            // 
            // kGroupUtility
            // 
            this.kGroupUtility.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kGroupUtility.Location = new System.Drawing.Point(3, 3);
            this.kGroupUtility.Name = "kGroupUtility";
            this.kGroupUtility.Size = new System.Drawing.Size(1252, 888);
            this.kGroupUtility.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kGroupUtility.StateCommon.Border.Rounding = 7;
            this.kGroupUtility.TabIndex = 2;
            // 
            // kHitoryPage
            // 
            this.kHitoryPage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kHitoryPage.Controls.Add(this.kGroupHistory);
            this.kHitoryPage.Flags = 65534;
            this.kHitoryPage.LastVisibleSet = true;
            this.kHitoryPage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kHitoryPage.Name = "kHitoryPage";
            this.kHitoryPage.Padding = new System.Windows.Forms.Padding(3);
            this.kHitoryPage.Size = new System.Drawing.Size(1258, 894);
            this.kHitoryPage.StateCommon.Page.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.kHitoryPage.StateCommon.Tab.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.kHitoryPage.StateCommon.Tab.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.kHitoryPage.StateCommon.Tab.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.kHitoryPage.StateCommon.Tab.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kHitoryPage.Text = "History";
            this.kHitoryPage.ToolTipTitle = "Page ToolTip";
            this.kHitoryPage.UniqueName = "3A4D3169C452490C4BB06A542932FE1C";
            // 
            // kGroupHistory
            // 
            this.kGroupHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kGroupHistory.Location = new System.Drawing.Point(3, 3);
            this.kGroupHistory.Name = "kGroupHistory";
            // 
            // kGroupHistory.Panel
            // 
            this.kGroupHistory.Panel.Controls.Add(this.kRichTbHistory);
            this.kGroupHistory.Size = new System.Drawing.Size(1252, 888);
            this.kGroupHistory.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kGroupHistory.StateCommon.Border.Rounding = 7;
            this.kGroupHistory.TabIndex = 3;
            // 
            // kRichTbHistory
            // 
            this.kRichTbHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kRichTbHistory.Location = new System.Drawing.Point(0, 0);
            this.kRichTbHistory.Name = "kRichTbHistory";
            this.kRichTbHistory.ReadOnly = true;
            this.kRichTbHistory.Size = new System.Drawing.Size(1246, 882);
            this.kRichTbHistory.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.kRichTbHistory.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.kRichTbHistory.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kRichTbHistory.StateCommon.Border.Rounding = 7;
            this.kRichTbHistory.StateCommon.Border.Width = 2;
            this.kRichTbHistory.StateCommon.Content.Color1 = System.Drawing.Color.White;
            this.kRichTbHistory.StateCommon.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.kRichTbHistory.TabIndex = 0;
            this.kRichTbHistory.Text = "Version 1.0.0.0\n- Initial Prototype";
            // 
            // AppMainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kNavigatorMain);
            this.Name = "AppMainPanel";
            this.Size = new System.Drawing.Size(1264, 932);
            ((System.ComponentModel.ISupportInitialize)(this.kNavigatorMain)).EndInit();
            this.kNavigatorMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kProductionPage)).EndInit();
            this.kProductionPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kGroupOperation.Panel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroupOperation)).EndInit();
            this.kGroupOperation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kAllSetupPage)).EndInit();
            this.kAllSetupPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kNavigatorMainSetup)).EndInit();
            this.kNavigatorMainSetup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kRecipeManagerPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGeneralSetupPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kSpecPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kMachineSetupPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kCommonParamPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kUsersManagerPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kComponentPage)).EndInit();
            this.kComponentPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kStateMachinePage)).EndInit();
            this.kStateMachinePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kNavigatorStateMachine)).EndInit();
            this.kNavigatorStateMachine.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.KSemiStateMachinePage)).EndInit();
            this.KSemiStateMachinePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kNavigatorSemiStateMachine)).EndInit();
            this.kNavigatorSemiStateMachine.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kUtilityPage)).EndInit();
            this.kUtilityPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kGroupUtility.Panel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kGroupUtility)).EndInit();
            this.kGroupUtility.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kHitoryPage)).EndInit();
            this.kHitoryPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kGroupHistory.Panel)).EndInit();
            this.kGroupHistory.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kGroupHistory)).EndInit();
            this.kGroupHistory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Navigator.KryptonNavigator kNavigatorMain;
        private ComponentFactory.Krypton.Navigator.KryptonPage kProductionPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kAllSetupPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kComponentPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kStateMachinePage;
        private ComponentFactory.Krypton.Navigator.KryptonPage KSemiStateMachinePage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kUtilityPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kHitoryPage;
        private ComponentFactory.Krypton.Navigator.KryptonNavigator kNavigatorMainSetup;
        private ComponentFactory.Krypton.Navigator.KryptonPage kRecipeManagerPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kGeneralSetupPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kSpecPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kMachineSetupPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kCommonParamPage;
        private ComponentFactory.Krypton.Navigator.KryptonPage kUsersManagerPage;
        private MCore.Controls.ComponentBrowser componentBrowser;
        private ComponentFactory.Krypton.Navigator.KryptonNavigator kNavigatorStateMachine;
        private ComponentFactory.Krypton.Navigator.KryptonNavigator kNavigatorSemiStateMachine;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kGroupUtility;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kGroupHistory;
        private ComponentFactory.Krypton.Toolkit.KryptonRichTextBox kRichTbHistory;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kGroupOperation;

    }
}
