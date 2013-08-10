﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers; 
#endregion

namespace SmartHome.Communications.SerialManager
{
    public class SerialManager
    {
        private Timer checkTimer;

        private List<NodeConnection> validNodes;
        private List<NodeConnection> unidentifiedNodes;

        public event EventHandler<NodeConnection> NodeConnectionAdded;
        public event EventHandler<NodeConnection> NodeConnectionRemoved;

        public SerialManager()
        {
            validNodes = new List<NodeConnection>();
            unidentifiedNodes = new List<NodeConnection>();

            checkTimer = new Timer()
            {
                Interval = 1000,
                AutoReset = false,
            };
            checkTimer.Elapsed += checkTimer_Elapsed;
            checkTimer.Start();
        }

        private void checkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var portNames = SerialPort.GetPortNames();

            //Remove the nodes that are not phisically connected
            for (int i = unidentifiedNodes.Count - 1; i >= 0; i--)
            {
                if (!portNames.Contains(unidentifiedNodes[i].PortName))
                {
                    unidentifiedNodes.RemoveAt(i);
                }
            }

            for (int i = validNodes.Count - 1; i >= 0; i--)
            {
                if (!portNames.Contains(validNodes[i].PortName))
                {
                    OnNodeConnectionRemoved(validNodes[i]);

                    validNodes.RemoveAt(i);
                }
            }

            //Add the new nodes to the unidentified nodes list, until it was successfully identified
            foreach (string portName in portNames.Where(p => !unidentifiedNodes.Union(validNodes).Any(n => n.PortName == p)))
            {
                var newNode = new NodeConnection(portName);
                newNode.ConnectionStateChanged += newNode_ConnectionStateChanged;
                newNode.Identify();
                unidentifiedNodes.Add(newNode);
            }

            checkTimer.Start();
        }

        void newNode_ConnectionStateChanged(object sender, NodeConnection.ConnectionStates e)
        {
            NodeConnection node = ((NodeConnection)sender);

            if (node.ConnectionState != NodeConnection.ConnectionStates.Connected)
                return;

            node.ConnectionStateChanged -= newNode_ConnectionStateChanged;

            unidentifiedNodes.Remove(node);
            validNodes.Add(node);

            OnNodeConnectionAdded(node);
        }

        private void OnNodeConnectionAdded(NodeConnection node)
        {
            if (NodeConnectionAdded != null)
                NodeConnectionAdded(this, node);
        }

        private void OnNodeConnectionRemoved(NodeConnection node)
        {
            if (NodeConnectionRemoved != null)
                NodeConnectionRemoved(this, node);
        }

        public NodeConnection GetNodeConnection(ushort nodeAddress)
        {
            return validNodes.FirstOrDefault(n => n.NodeAddress == nodeAddress);
        }

        public IEnumerable<NodeConnection> GetAllConections()
        {
            return validNodes.Where(n => n.Identified);
        }
    }
}
