using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using MCore.Comp;

namespace MCore.Controls
{
    public partial class ComponentBrowser : UserControl
    {

        private CompBase _rootComp = null;
        public TabControl TabPages = null;

        public ComponentBrowser()
        {
            InitializeComponent();
            TabPages = tabPages;
        }
        /// <summary>
        /// Rebuild all the tree components
        /// </summary>
        public void Rebuild()
        {
            treeComponents.Nodes.Clear();
            tabPages.Controls.Clear();
            try
            {
                _rootComp = U.RootComp;
                treeComponents.BeginUpdate();
                BuildTree(treeComponents.Nodes, U.RootComp);
                treeComponents.EndUpdate();
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Could not rebuild component tree.");    
            }
            //treeComponents.ExpandAll();
            CompBase.OnChangedName += new CompBase.NameChangedEventHandler(OnChangedCompName);
            CompBase.OnRemovingComponent += new CompBase.ComponentEventHandler(OnRemovingComponent);
            CompBase.OnAddedComponent += new CompBase.ComponentEventHandler(OnAddedComponent);
            CompBase.OnSortedComponentChildren += new CompBase.ComponentEventHandler(OnSortedComponentChildren);
        }

        public void Rebuild(CompBase comp)
        {
            treeComponents.Nodes.Clear();
            tabPages.Controls.Clear();

            try
            {
                _rootComp = comp;
                treeComponents.BeginUpdate();
                BuildTree(treeComponents.Nodes, comp);
                treeComponents.EndUpdate();
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Could not rebuild component tree.");
            }
            //treeComponents.ExpandAll();
            CompBase.OnChangedName += new CompBase.NameChangedEventHandler(OnChangedCompName);
            CompBase.OnRemovingComponent += new CompBase.ComponentEventHandler(OnRemovingComponent);
            CompBase.OnAddedComponent += new CompBase.ComponentEventHandler(OnAddedComponent);
            CompBase.OnSortedComponentChildren += new CompBase.ComponentEventHandler(OnSortedComponentChildren);
        }

        private void OnChangedCompName(CompBase comp, string oldID)
        {
            if (string.IsNullOrEmpty(oldID) || comp.ID == oldID)
            {
                // This is the case where we arwe trying to change the class type.  It is handled elsewhere.
                return;
            }
            if (InvokeRequired)
            {
                BeginInvoke(new CompBase.NameChangedEventHandler(OnChangedCompName), comp, oldID);
                return;
            }
            if (!comp.IsRooted || string.IsNullOrEmpty(oldID))
            {
                return;
            }
            try
            {
                treeComponents.BeginUpdate();
                TreeNode[] matches = treeComponents.Nodes.Find(oldID, true);
                if (matches.Length == 0)
                {
                    return;
                }
                if (matches.Length > 1)
                {
                    throw new MCoreException(LogSeverity.Popup, "Found more than one components with same ID: '{0}'", oldID);
                }
                TreeNode nodeMatch = matches[0];
                nodeMatch.Text = comp.Name;
                TreeNode nodeParent = nodeMatch.Parent;
                if (nodeParent == null)
                {
                    throw new MCoreException(LogSeverity.Popup, "Expected to find a parent for '{0}'", oldID);
                }
                int index = nodeMatch.Index;
                treeComponents.SelectedNode = null;
                nodeParent.Nodes.Remove(nodeMatch);
                TreeNode newNode = nodeParent.Nodes.Insert(index, comp.ID, comp.Name);
                newNode.Tag = comp;
                if (comp.HasChildren)
                {
                    foreach (CompBase compChild in comp.ChildArray)
                    {
                        BuildTree(newNode.Nodes, compChild);
                    }
                }

            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to update Component Tree at: '{0}'", oldID);
            }
            finally
            {
                treeComponents.EndUpdate();
            }
        }

        public void SuspendGUI()
        {
            treeComponents.BeginUpdate();
        }

        public void ResumeGUI()
        {
            treeComponents.EndUpdate();
        }

        private void OnSortedComponentChildren(CompBase compParent)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CompBase.ComponentEventHandler(OnSortedComponentChildren), compParent);
                return;
            }
            if (!compParent.IsRooted || string.IsNullOrEmpty(compParent.ID))
            {
                return;
            }
            try
            {
                TreeNode[] matches = treeComponents.Nodes.Find(compParent.ID, true);
                if (matches.Length != 1)
                {
                    return;
                }
                TreeNode nodeParent = matches[0];
                int treeCount = nodeParent.Nodes.Count;
                int compCount = compParent.Count;
                if (treeCount != compCount)
                {
                    U.LogPopup("number of component children of '{0}' does not match number of Component Browser children", compParent.Nickname);
                    return;
                }
                if (treeCount == 0)
                {
                    return;
                }
                SuspendGUI();
                CompBase[] children = compParent.ChildArray;
                for (int i=0; i<compCount; i++)
                {
                    // Find the treerNode
                    CompBase compChild = compParent[i];
                    TreeNode treeChild = nodeParent.Nodes[compChild.ID];
                    nodeParent.Nodes.RemoveByKey(compChild.ID);
                    nodeParent.Nodes.Insert(i, treeChild);
                }
                ResumeGUI();
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to Sort Component Tree at: '{0}'", compParent.ID);
            }
        }
        private void OnRemovingComponent(CompBase comp)
        { 
            if (comp.Parent == _rootComp)
            {
                return;
            }

            string id = comp.ID;
            if (comp.IsRooted && !string.IsNullOrEmpty(id))
            {
                DoRemovingComponent(id);
            }
        }
        private void DoRemovingComponent(string id)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new D.delVoid_String(DoRemovingComponent), id);
                return;
            }
            try
            {
                TreeNode[] matches = treeComponents.Nodes.Find(id, true);

                if (matches.Length != 1)
                {
                    return;
                }
                TreeNode nodeMatch = matches[0];
                TreeNode nodeParent = nodeMatch.Parent;
                if (nodeParent == null)
                {
                    throw new MCoreException(LogSeverity.Popup, "Expected to find a parent for '{0}'", id);
                }
                int index = nodeMatch.Index;
                nodeParent.Nodes.Remove(nodeMatch);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to delete Component Tree at: '{0}'", id);
            }
        }

        private void OnAddedComponent(CompBase compChild)
        {
            if (compChild.IsRooted && !string.IsNullOrEmpty(compChild.ID))
            {
                DoAddedComponent(compChild);
            }
        }
        private void DoAddedComponent(CompBase compChild)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CompBase.ComponentEventHandler(DoAddedComponent), compChild);
                return;
            }
            try
            {
                TreeNode[] matches = treeComponents.Nodes.Find(compChild.Parent.ID, true);

                if(matches.Length == 0 && compChild.Parent != U.RootComp )
                {
                    return;
                }

                if (matches.Length != 1)
                {
                    throw new MCoreException(LogSeverity.Popup, "Expected to find a parent for '{0}'", compChild.ID);
                }
                TreeNode nodeParent = matches[0];

                TreeNode nd = null;
                if (nodeParent.Nodes.ContainsKey(compChild.ID))
                {
                    nd = nodeParent.Nodes.Find(compChild.ID, false)[0];
                    nd.Tag = compChild;
                    BuildTree(nd.Nodes, compChild);
                }
                else
                {
                    nd = new TreeNode(compChild.Name) { Name = compChild.ID, Tag = compChild };
                    BuildTree(nd.Nodes, compChild);
                    nodeParent.Nodes.Add(nd);
                }

            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to add Component Tree at: '{0}'", compChild.ID);
            }
        }
        private void BuildTree(TreeNodeCollection treeNode, CompBase compParent)
        {
            if (compParent.HasChildren)
            {
                foreach (CompBase compChild in compParent.ChildArray)
                {
                    TreeNode nd = null;
                    if (treeNode.ContainsKey(compChild.ID))
                    {
                        nd = treeNode.Find(compChild.ID, false)[0];
                        nd.Tag = compChild;
                    }
                    else
                    {
                        nd = treeNode.Add(compChild.ID, compChild.Name);
                        nd.Tag = compChild;
                    }

                    BuildTree(nd.Nodes, compChild);
                }
            }
        }

        private void OnNewSelection(object sender, TreeViewEventArgs e)
        {
            if (tabPages.Controls.Count > 0)
            {
                foreach (TabPage page in tabPages.Controls)
                {
                    foreach (Control control in page.Controls)
                    {
                        control.Dispose();
                    }
                    //page.Dispose();
                }
                tabPages.Controls.Clear();
            }
            CompBase comp = null;
            Type tyComp = null;

            try
            {
                comp = e.Node.Tag as CompBase;
                tyComp = comp.GetType();
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Problem with selection");
                return;
            }
            List<Control> controls = new List<Control>();
            try
            {
                do
                {
                    Type[] controlTypes = CompRoot.GetControlPages(tyComp);
                    if (controlTypes != null)
                    {
                        for (int i = 0; i < controlTypes.Length; i++ )
                        {
                            Type controlType = controlTypes[i];
                            if (comp.IgnorePageList.Contains(controlType))
                            {
                                continue;
                            }
                            Control control = Activator.CreateInstance(controlType) as Control;
                            PropertyInfo pi = controlType.GetProperty("Bind");
                            pi.SetValue(control, comp, null);
                            controls.Insert(0, control);
                        }
                    }

                    tyComp = tyComp.BaseType;
                }
                while (tyComp != null && !Type.Equals(tyComp, typeof(Object)));
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Problem with selection");
            }


            try
            {

                for (int i = 0; i < controls.Count; i++)
                {
                    Control control = controls[i];
                    TabPage tabPage = new TabPage(control.ToString());
                    tabPage.Location = new System.Drawing.Point(4, 22);
                    tabPage.Name = string.Format("tabPage{0}", i);
                    tabPage.Padding = new System.Windows.Forms.Padding(3);
                    tabPage.TabIndex = i;
                    tabPage.UseVisualStyleBackColor = true;
                    tabPage.Controls.Add(control);
                    tabPages.Controls.Add(tabPage);
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Problem with selection");
            }
            OnSizedChanged(null, null);
        }

        private void OnSizedChanged(object sender, EventArgs e)
        {
            Size tabPagesSize = new Size(tabPages.Width - 8, tabPages.Height - 26);
            foreach (TabPage tabPage in tabPages.Controls)
            {
                tabPage.Size = tabPagesSize;
                Control control = tabPage.Controls[0];
                control.Size = tabPagesSize;
            }
        }
    }
}
