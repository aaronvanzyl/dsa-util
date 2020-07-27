using System;
using System.Collections.Generic;
using System.Text;

namespace Util.Tree
{
    public class Tree
    {
        //public List<Node> nodes = new List<Node>();
        public Node rootNode;

        public Tree() {
            rootNode = new Node();

        }

        public List<Node> DFS()
        {
            List<Node> nodesInDFS = new List<Node>();
            Stack<Node> stack = new Stack<Node>();
            stack.Push(rootNode);
            while (stack.Count > 0) {
                Node n = stack.Pop();
                nodesInDFS.Add(n);
                foreach (Node child in n.children) {
                    stack.Push(child);
                }
            }
            return nodesInDFS;
        }

        public void PrintTree()
        {
            List<Node> dfs = DFS();
            foreach (Node n in dfs)
            {
                Console.WriteLine(n);
            }
        }
    }

    public class Node {
        public List<Node> children = new List<Node>();
        public Node parent;
    }

    
}
