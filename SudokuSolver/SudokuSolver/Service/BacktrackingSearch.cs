using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SudokuSolver.ServiceLayer
{
    public class BacktrackingSearch
    {
        public static Node Search(Node startingNode)
        {
            //Count for tracking the first 3 nodes
            int count = 0;
            List<string> output = new List<string>{"First 3 Steps:"};

            //Initialize and start stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Intiailize stack, and push initial node onto stack
            //making sure constraints are good
            Stack<Node> stack = new Stack<Node>();
            startingNode.ApplyConstraints();
            stack.Push(startingNode);


            while(stack.Count != 0)
            {
                //Get top node off stack
                Node node = stack.Pop();

                //Output action if it's one of the first 3
                if (count > 0 && count < 4)
                    output.Add(node.output);
                
                if (node.IsGoalNode())
                {
                    //Stop stopwatch and add results to output
                    stopwatch.Stop();
                    output.Add("Execution Time: " + stopwatch.ElapsedMilliseconds + " ms");

                    //Return the end node and the outout
                    return node;
                }
                else if(!node.IsTerminalNode())
                {
                    //Get the node's children (Heuristic already applied in method)
                    var children = node.GetChildren();

                    //Push children onto the stack
                    foreach (Node child in children)
                        stack.Push(child);
                }

                count++;
            }

            //If no result is found, this will never happen hopefully
            return null;
        }
    }
}
