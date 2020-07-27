using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Util.Tree
{
    public class PrefixTree : Tree
    {
        string[] strings;

        public PrefixTree(string[] strings)
        {
            this.strings = strings;
            rootNode = new PrefixNode();
            foreach (string s in strings)
            {
                AddString(s);
            }
        }

        public PrefixNode AddLeafNode(PrefixNode parentNode, int chainLen)
        {
            PrefixNode leafNode = new PrefixNode(parentNode, chainLen, parentNode.uBound + 1, parentNode.uBound + 1);
            parentNode.children.Add(leafNode);
            //nodes.Add(leafNode);
            while (parentNode != null)
            {
                parentNode.uBound++;
                parentNode = (PrefixNode)parentNode.parent;
            }
            return leafNode;
        }

        public PrefixNode InsertMiddleNode(PrefixNode parentNode, PrefixNode childNode, int pos)
        {
            PrefixNode middleNode = new PrefixNode(parentNode, pos, parentNode.lBound, parentNode.uBound);
            //nodes.Add(middleNode);
            parentNode.children.Add(middleNode);
            parentNode.children.Remove(childNode);

            middleNode.children.Add(childNode);
            childNode.parent = middleNode;

            childNode.edgeLen -= pos;
            return middleNode;
        }

        public void AddString(string str)
        {
            //Console.WriteLine("add string: " + str);
            PrefixNode matchNode;
            int pathLen = MatchLen(str, out matchNode);
            if (pathLen == matchNode.pathLen)
            {
                AddLeafNode(matchNode, str.Length - pathLen);
            }
            else
            {
                PrefixNode middleNode = InsertMiddleNode((PrefixNode)matchNode.parent, matchNode, pathLen - ((PrefixNode)matchNode.parent).pathLen);
                AddLeafNode(middleNode, str.Length - pathLen);
            }
        }

        public int MatchLen(string str, out PrefixNode matchNode)
        {
            matchNode = (PrefixNode)rootNode;
            int edgePos = 0;
            int pathLen = 0;
            while (pathLen < str.Length)
            {
                //Console.WriteLine("char:" + str[pathLen] + " pos:" + pathLen + " node:" + currentNode.ToString());
                // if still inside current edge
                if (edgePos < matchNode.edgeLen)
                {
                    // char matches, move 1 further
                    if (strings[matchNode.lBound][pathLen] == str[pathLen])
                    {
                        //Console.WriteLine("match, incrementing");
                        edgePos++;
                        pathLen++;
                    }
                    // char does not match, break edge and create leaf
                    else
                    {
                        return pathLen;
                    }
                }
                // reached end of current edge
                else
                {
                    bool foundMatch = false;
                    foreach (PrefixNode n in matchNode.children)
                    {
                        if (strings[n.lBound][pathLen] == str[pathLen])
                        {
                            matchNode = n;
                            edgePos = 1;
                            pathLen++;
                            foundMatch = true;
                            //Console.WriteLine("next edge, incrementing");
                            break;
                        }
                    }
                    if (!foundMatch)
                    {
                        //Console.WriteLine("no matching edge, making leaf node");
                        return pathLen;
                    }
                }
            }
            return pathLen;
        }

    }

    public class PrefixNode : Node
    {
        // length from parent node to this node
        public int edgeLen;
        // total length of path to this node (for leaf = string length)
        public int pathLen;
        public int lBound;
        public int uBound;

        public PrefixNode()
        {
            lBound = 0;
            uBound = -1;
        }

        public PrefixNode(PrefixNode parent, int edgeLen, int lBound, int uBound)
        {
            this.parent = parent;
            this.edgeLen = edgeLen;
            this.pathLen = parent.pathLen + edgeLen;
            this.lBound = lBound;
            this.uBound = uBound;
        }

        public override string ToString()
        {
            return lBound + " " + uBound + " " + edgeLen;
        }
    }
}
