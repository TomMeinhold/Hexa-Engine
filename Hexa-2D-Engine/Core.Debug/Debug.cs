using HexaEngine.Core.Common;
using HexaEngine.Core.Objects;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexaEngine.Core.Debug
{
    public partial class Debug : Form
    {
        public Engine Engine;

        public Debug(Engine engine)
        {
            Engine = engine;
            InitializeComponent();
            LoadObjects();
        }

        private void LoadObjects()
        {
            ObjectSystem system = Engine.ObjectSystem;
            foreach (BaseObject baseObject in system.ObjectList)
            {
                TreeNode node = new TreeNode
                {
                    Name = baseObject.Name,
                    Text = baseObject.Name
                };
                TreeNode node1 = GenerateTreeNode(baseObject.Speed);
                node1.Name = "speed";
                node1.Text = "speed";
                node.Nodes.Add(node1);
                TreeNode node2 = GenerateTreeNode(baseObject.Acceleration);
                node2.Name = "acceleration";
                node2.Text = "acceleration";
                node.Nodes.Add(node2);
                treeView1.Nodes.Add(node);
            }
        }

        private void UpdateObjects()
        {
            treeView1.BeginUpdate();
            int i = 0;
            ObjectSystem system = Engine.ObjectSystem;
            foreach (TreeNode nodeBase in treeView1.Nodes)
            {
                BaseObject baseObject = (BaseObject)system.ObjectList[i];
                foreach (TreeNode node in nodeBase.Nodes)
                {
                    UpdateTreeNode(node, baseObject.Speed);
                    UpdateTreeNode(node, baseObject.Acceleration);
                }
                i++;
            } 
            treeView1.EndUpdate();
        }

        private void Debug_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateObjects();
        }

        private TreeNode GenerateTreeNode(RawVector3 vector3)
        {
            TreeNode node = new TreeNode();
            TreeNode nodeX = new TreeNode
            {
                Text = vector3.X.ToString()
            };
            TreeNode nodeY = new TreeNode
            {
                Text = vector3.Y.ToString()
            };
            TreeNode nodeZ = new TreeNode
            {
                Text = vector3.Z.ToString()
            };
            node.Nodes.AddRange(new TreeNode[] { nodeX, nodeY, nodeZ });
            return node;
        }
        private TreeNode UpdateTreeNode(TreeNode node,RawVector3 vector3)
        {
            node.Nodes.Clear();
            TreeNode nodeX = new TreeNode
            {
                Text = vector3.X.ToString()
            };
            TreeNode nodeY = new TreeNode
            {
                Text = vector3.Y.ToString()
            };
            TreeNode nodeZ = new TreeNode
            {
                Text = vector3.Z.ToString()
            };
            node.Nodes.AddRange(new TreeNode[] { nodeX, nodeY, nodeZ });
            return node;
        }
    }
}
