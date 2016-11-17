﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeSharpPlus {
    public class Randomm : NodeGroupWeighted {
        public Randomm(params NodeWeight[] weightedchildren)
            : base(weightedchildren) {
        }

        public Randomm(params Node[] children)
            : base(children) {
        }

        public override void Start() {
            this.Shuffle();
            base.Start();

        }

        public override IEnumerable<RunStatus> Execute() {
            // Proceed as we do with the original sequence
            Node node = this.Children.First();

            // Move to the next node
            this.Selection = node;
            node.Start();

            // If the current node is still running, report that. Don't 'break' the enumerator
            RunStatus result;
            while ((result = this.TickNode(node)) == RunStatus.Running)
                yield return RunStatus.Running;

            // Call Stop to allow the node to clean anything up.
            node.Stop();

            // Clear the selection
            this.Selection.ClearLastStatus();
            this.Selection = null;

            if (result == RunStatus.Failure) {
                yield return RunStatus.Failure;
                yield break;
            }

            yield return RunStatus.Running;

            yield return RunStatus.Success;
            yield break;
        }
    }
}